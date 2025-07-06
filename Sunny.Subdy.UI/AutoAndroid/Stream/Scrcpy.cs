using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Buffers;
using System.Buffers.Binary;

namespace AutoAndroid.Stream
{


    public class Scrcpy
    {
        public int DeviceWidth { get; set; }
        public int DeviceHeight { get; set; }
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public long Bitrate { get; set; } = 8000000;
        public string ScrcpyServerFile { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libs\\Scrcpy\\scrcpy-server.jar");
        public bool Connected { get; private set; }
        public VideoStreamDecoder VideoStreamDecoder { get; set; }
        private TcpClient? videoClient;
        private TcpClient? controlClient;
        private TcpListener? listener;
        private CancellationTokenSource? cts;
        private DeviceModel _device;
        private readonly Channel<IControlMessage> controlChannel = Channel.CreateUnbounded<IControlMessage>();
        private static readonly ArrayPool<byte> pool = ArrayPool<byte>.Shared;
        private Process? _Process;
        private ScrcpyDisplay scrcpyDisplay;

        public Scrcpy(ScrcpyDisplay control)
        {
            _device = control.Device;
            scrcpyDisplay = control;
            VideoStreamDecoder = new VideoStreamDecoder();
            VideoStreamDecoder.Scrcpy = this;
            _device.PropertyChanged += Device_PropertyChanged;
        }

        public static int GetPort(string deviceId)
        {
            try
            {
                string text = Command($"adb -s {deviceId} reverse --list");
                if (string.IsNullOrEmpty(text)) return GetNewAvailablePort();

                var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (line.Contains("localabstract:scrcpy"))
                    {
                        var match = Regex.Match(line, @"tcp:(\d+)");
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int port))
                        {
                            return port;
                        }
                    }
                }
                return GetNewAvailablePort();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting port: {ex.Message}");
                return GetNewAvailablePort();
            }
        }

        private static int GetNewAvailablePort()
        {
            TcpListener? listener = null;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    int port = new Random().Next(27183, 65536);
                    listener = new TcpListener(IPAddress.Loopback, port);
                    listener.Start();
                    return port;
                }
                throw new Exception("No available ports found.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error finding available port: {ex.Message}");
                throw;
            }
            finally
            {
                listener?.Stop();
            }
        }

        private void Device_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DeviceModel.IsScrcpy) && !_device.IsScrcpy)
            {
                Close();
            }
        }

        public async Task Start()
        {
            try
            {
                await Connect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to start scrcpy: {ex.Message}");
                throw;
            }
        }

        public void SendControlCommand(IControlMessage msg)
        {
            try
            {
                if (controlClient != null)
                {
                    controlChannel.Writer.TryWrite(msg);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending control command: {ex.Message}");
            }
        }

        private async Task Connect()
        {
            cts = new CancellationTokenSource();
            int retryCounter = 0;
            int delayMs = 1000;
            bool rebooted = false;

            while (!cts.Token.IsCancellationRequested && retryCounter < 5)
            {
                try
                {
                    retryCounter++;
                    Debug.WriteLine($"Connection attempt {retryCounter}");

                    if (!await EnsureDeviceIsConnected(retryCounter, cts.Token))
                    {
                        continue;
                    }

                    // Clean up resources before starting
                    await CleanScrcpyResources();

                    await ResetAdbPortForwarding();
                    MobileServerSetup();

                    if (!StartListener(_device.PortScrcpy))
                    {
                        Debug.WriteLine($"Port {_device.PortScrcpy} is not available, trying new port...");
                        _device.PortScrcpy = GetNewAvailablePort();
                        continue;
                    }

                    RunServer();

                    if (!await WaitForClientConnections())
                    {
                        continue;
                    }
                    await StartProcessing();
                    Connected = true;
                    Debug.WriteLine("Scrcpy connected successfully.");
                    return; // Success
                }
                catch (TimeoutException ex)
                {
                    Debug.WriteLine($"Timeout waiting for client connections: {ex.Message}");
                    if (!rebooted && retryCounter >= 3)
                    {
                        Debug.WriteLine("Persistent failure, rebooting device...");
                        Command($"adb -s {_device.Serial} reboot");
                        await Task.Delay(30000, cts.Token); // Wait for reboot
                        rebooted = true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Connect attempt {retryCounter} failed: {ex.Message}");
                }
                finally
                {
                    CleanUp();
                    LoadLogo();
                }

                await Task.Delay(delayMs, cts.Token);
                delayMs = Math.Min(delayMs * 2, 10000);
            }

            throw new Exception("Failed to connect to scrcpy after 5 attempts.");
        }

        private async Task<bool> EnsureDeviceIsConnected(int retryCounter, CancellationToken token)
        {
            if (!IsDeviceConnected())
            {
                Debug.WriteLine("Device not connected.");
                LoadLogo();
                await Task.Delay(1000, token).ConfigureAwait(false);
                return false;
            }
            return true;
        }

        private async Task CleanScrcpyResources()
        {
            try
            {
                // Remove local socket (ignore "No such file" error)
                string rmOutput = Command($"adb -s {_device.Serial} shell rm /data/local/tmp/scrcpy");
                if (!rmOutput.Contains("No such file"))
                {
                    Debug.WriteLine("Removed local socket /data/local/tmp/scrcpy");
                }

                // Check and kill scrcpy or app_process processes
                string psOutput = Command($"adb -s {_device.Serial} shell ps");
                var pids = ExtractPids(psOutput);
                foreach (var pid in pids)
                {
                    try
                    {
                        Command($"adb -s {_device.Serial} shell kill {pid}");
                        Debug.WriteLine($"Killed process with PID: {pid}");
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("No such process"))
                        {
                            Debug.WriteLine($"PID {pid} no longer exists, skipping.");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                // Check and kill process using port
                CheckAndKillPort(_device.PortScrcpy);

                // Remove stale adb reverse mappings
                Command($"adb -s {_device.Serial} reverse --remove-all");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error cleaning scrcpy resources: {ex.Message}");
            }
        }

        private List<string> ExtractPids(string psOutput)
        {
            var pids = new List<string>();
            var lines = psOutput.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.Contains("scrcpy", StringComparison.OrdinalIgnoreCase) ||
                    line.Contains("app_process", StringComparison.OrdinalIgnoreCase))
                {
                    var columns = Regex.Split(line.Trim(), @"\s+");
                    if (columns.Length >= 2 && columns[1].All(char.IsDigit))
                    {
                        pids.Add(columns[1]);
                    }
                }
            }
            return pids;
        }

        private async Task ResetAdbPortForwarding()
        {
            Command($"adb -s {_device.Serial} reverse --remove-all");
            Command($"adb -s {_device.Serial} reverse localabstract:scrcpy tcp:{_device.PortScrcpy}");
        }

        private bool StartListener(int port)
        {
            if (!IsPortAvailable(port))
            {
                Debug.WriteLine($"Port {port} is not available.");
                return false;
            }

            try
            {
                listener = new TcpListener(IPAddress.Loopback, port);
                listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                listener.Start();
                Debug.WriteLine($"Started TCP listener on port {port}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error starting listener on port {port}: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> WaitForClientConnections()
        {
            try
            {
                videoClient = await AcceptClientWithTimeout(listener, 5000, cts.Token);
                videoClient.NoDelay = true;
                videoClient.ReceiveBufferSize = 1024 * 1024;
                controlClient = await AcceptClientWithTimeout(listener, 5000, cts.Token);
                controlClient.NoDelay = true;
                ReadDeviceInfo();
                Debug.WriteLine("Client connections established.");
                return true;
            }
            catch (TimeoutException ex)
            {
                Debug.WriteLine($"Timeout waiting for client connections: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error waiting for client connections: {ex.Message}");
                return false;
            }
        }

        private async Task StartProcessing()
        {
            var videoTask = Task.Run(VideoMain, cts.Token);
            var controlTask = ControllerMain();
            await Task.WhenAll(videoTask, controlTask);
        }

        private void VideoMain()
        {
            if (videoClient == null || cts == null) return;

            var videoStream = videoClient.GetStream();
            videoStream.ReadTimeout = 2000;

            int bytesRead;
            var metaBuf = pool.Rent(12);

            Stopwatch sw = new();

            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    bytesRead = videoStream.Read(metaBuf, 0, 12);
                    if (bytesRead != 12)
                    {
                        cts.Cancel();
                        return;
                    }

                    sw.Restart();

                    var metaSpan = metaBuf.AsSpan();
                    long presentationTimeUs = BinaryPrimitives.ReadInt64BigEndian(metaSpan);
                    int packetSize = BinaryPrimitives.ReadInt32BigEndian(metaSpan.Slice(8, 4));

                    byte[] packetBuf = pool.Rent(packetSize);
                    int pos = 0;
                    int bytesToRead = packetSize;

                    while (bytesToRead != 0 && !cts.Token.IsCancellationRequested)
                    {
                        bytesRead = videoStream.Read(packetBuf, pos, bytesToRead);
                        if (bytesRead == 0) throw new Exception("Unable to read any bytes.");

                        pos += bytesRead;
                        bytesToRead -= bytesRead;
                    }

                    if (!cts.Token.IsCancellationRequested)
                        VideoStreamDecoder?.Decode(packetBuf, presentationTimeUs);

                    sw.Stop();
                    pool.Return(packetBuf);
                }
                catch (IOException ex) when (ex.InnerException is SocketException socketEx &&
                                            socketEx.SocketErrorCode == SocketError.TimedOut)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in VideoMain: {ex.Message}");
                    break;
                }
            }

            pool.Return(metaBuf);
        }

        private async Task ControllerMain()
        {
            try
            {
                if (controlClient == null) return;
                if (cts == null) return;
                var stream = controlClient.GetStream();
                try
                {
                    await foreach (var cmd in controlChannel.Reader.ReadAllAsync(cts.Token))
                    {
                        ControllerSend(stream, cmd);
                    }
                }
                catch
                {
                    cts.Cancel();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error in ControllerMain: {ex.Message}");
                cts.Cancel();
            }
        }

        private void ControllerSend(NetworkStream stream, IControlMessage cmd)
        {
            try
            {
                var bytes = cmd.ToBytes();
                stream.Write(bytes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending control message: {ex.Message}");
            }
        }

        private void MobileServerSetup()
        {
            UploadMobileServer();
            Command($"adb -s {_device.Serial} shell chmod 644 /data/local/tmp/scrcpy-server.jar");
            Command($"adb -s {_device.Serial} reverse localabstract:scrcpy tcp:{_device.PortScrcpy}");
            Command($"adb -s {_device.Serial} shell input keyevent 82");
            Command($"adb -s {_device.Serial} shell input keyevent 82");
        }

        private void UploadMobileServer()
        {
            string remotePath = "/data/local/tmp/scrcpy-server.jar";
            Push(ScrcpyServerFile, remotePath);
        }

        private void Push(string filePath, string remotePath)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    string command = $"adb -s {_device.Serial} push \"{filePath}\" {remotePath}";
                    string text = Command(command);
                    if (string.IsNullOrEmpty(text)) continue;
                    string value = Shell("ls /data/local/tmp/scrcpy-server.jar").Trim();
                    if (value == remotePath)
                    {
                        Debug.WriteLine("Successfully pushed scrcpy-server.jar");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Push attempt {i + 1} failed: {ex.Message}");
                }
            }
            throw new Exception("Failed to push scrcpy-server.jar after multiple attempts.");
        }

        
        private void RunServer()
        {
            CancellationToken ct = cts.Token;
            string cmdShell = "CLASSPATH=/data/local/tmp/scrcpy-server.jar app_process / com.genymobile.scrcpy.Server 1.23  bit_rate=2000000 max_fps=6 tunnel_forward=false display_id=0 show_touches=false stay_awake=false power_off_on_close=false downsize_on_error=true cleanup=true lock_video_orientation=0";
            _ = SendAdbShellCommand(_device.Serial, cmdShell, ct);
        }
        public async Task SendAdbShellCommand(string serial, string shellCommand, CancellationToken token = default)
        {
            using var client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 5037, token);
            using var stream = client.GetStream();

            // 1. Gửi yêu cầu chuyển transport đến thiết bị
            await SendAdbRequest(stream, $"host:transport:{serial}");
            string response = await ReadAdbResponse(stream);
            if (response != "OKAY")
                throw new Exception("Failed to switch transport");

            // 2. Gửi lệnh shell command
            await SendAdbRequest(stream, $"shell:{shellCommand}");
            response = await ReadAdbResponse(stream);
            if (response != "OKAY")
                throw new Exception("Failed to start shell command");

            // 3. Đọc output shell command liên tục (đến khi token cancel hoặc stream đóng)
            var buffer = new byte[4096];
            while (!token.IsCancellationRequested)
            {
                int bytesRead;
                try
                {
                    bytesRead = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                if (bytesRead == 0)
                {
                    // Stream đóng
                    break;
                }
                string output = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.WriteLine($"ADB shell output: {output}");
            }
        }

        private async Task SendAdbRequest(NetworkStream stream, string request)
        {
            string lengthHex = request.Length.ToString("X4");
            string fullRequest = lengthHex + request;
            byte[] requestBytes = Encoding.ASCII.GetBytes(fullRequest);
            await stream.WriteAsync(requestBytes, 0, requestBytes.Length);
        }

        private async Task<string> ReadAdbResponse(NetworkStream stream)
        {
            var buffer = new byte[4];
            int read = 0;
            while (read < 4)
            {
                int n = await stream.ReadAsync(buffer, read, 4 - read);
                if (n == 0) break; // closed
                read += n;
            }
            return Encoding.ASCII.GetString(buffer, 0, read);
        }
        public static string Command(string cmd, int timeout = 10)
        {
            string output = "";
            string error = "";
            int retryCount = 0;
            int maxRetries = 3;

            while (retryCount < maxRetries)
            {
                try
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = $"/C \"{cmd}\"";
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                        process.StartInfo.StandardErrorEncoding = Encoding.UTF8;

                        output = "";
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                output += e.Data + "\n";
                            }
                        };
                        error = "";
                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                error += e.Data + "\n";
                            }
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        bool exited = process.WaitForExit(timeout * 1000);

                        if (!exited)
                        {
                            process.Kill();
                            Debug.WriteLine($"Command timed out after {timeout} seconds");
                            retryCount++;
                            continue;
                        }

                        // Allow "No such file" or "No such process" errors
                        if (error.Contains("No such file") || error.Contains("No such process"))
                        {
                            return output.Trim();
                        }

                        if (!string.IsNullOrEmpty(error))
                        {
                            throw new Exception($"Command failed: {error}");
                        }

                        return output.Trim();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Command attempt {retryCount + 1} failed: {ex.Message}");
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        throw new Exception($"Command failed after {maxRetries} retries: {error}");
                    }
                }
            }
            return output;
        }

        public static string CommandBatch(string[] commands, int timeout = 10)
        {
            string cmd = string.Join(" && ", commands);
            return Command(cmd, timeout);
        }

        public string Shell(string command)
        {
            return Command($"adb -s {_device.Serial} shell {command}");
        }
        public static (int Width, int Height) GetDeviceScreenSize(string deviceSerial)
        {
            try
            {
                ProcessStartInfo psi = new()
                {
                    FileName = "adb",
                    Arguments = $"-s {deviceSerial} shell wm size",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Ví dụ đầu ra: "Physical size: 720x1600"
                var match = Regex.Match(output, @"Physical size:\s*(\d+)x(\d+)");
                if (match.Success)
                {
                    int width = int.Parse(match.Groups[1].Value);
                    int height = int.Parse(match.Groups[2].Value);
                    return (width, height);
                }

                throw new Exception("Không tìm thấy kích thước màn hình trong adb output.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Lỗi lấy screen size: {ex.Message}");
                return (720, 1280); // fallback mặc định nếu adb lỗi
            }
        }
        private void ReadDeviceInfo()
        {
            try
            {
                if (videoClient == null) return;

                var infoStream = videoClient.GetStream();
                infoStream.ReadTimeout = 2000;

                var deviceInfoBuf = pool.Rent(68);
                int bytesRead = infoStream.Read(deviceInfoBuf, 0, 68);

                if (bytesRead != 68)
                {
                    _device.IsScrcpy = false;
                    Debug.WriteLine("Failed to read device info: insufficient bytes read.");
                }

                var deviceInfoSpan = deviceInfoBuf.AsSpan();
                Width = BinaryPrimitives.ReadInt16BigEndian(deviceInfoSpan[64..]);
                Height = BinaryPrimitives.ReadInt16BigEndian(deviceInfoSpan[66..]);
                Debug.WriteLine($"Device resolution: {Width}x{Height}");

                pool.Return(deviceInfoBuf);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading device info: {ex.Message}");
            }
        }

        private void LoadLogo()
        {
            try
            {
                scrcpyDisplay.View.Image = Properties.Resources.LamTool;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading logo: {ex.Message}");
            }
        }

        private void CloseConnections()
        {
            try
            {
                listener?.Stop();
                videoClient?.Close();
                controlClient?.Close();
                Debug.WriteLine("Closed TCP connections.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error closing connections: {ex.Message}");
            }
        }

        private void CleanUp()
        {
            try
            {
                CloseConnections();
                if (_Process != null && !_Process.HasExited)
                {
                    _Process.Kill();
                    _Process.Dispose();
                    Debug.WriteLine("Terminated scrcpy process.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }

        private async Task<TcpClient> AcceptClientWithTimeout(TcpListener listener, int timeoutMs, CancellationToken token)
        {
            var acceptTask = listener.AcceptTcpClientAsync();
            if (await Task.WhenAny(acceptTask, Task.Delay(timeoutMs, token)) == acceptTask)
            {
                return await acceptTask;
            }
            throw new TimeoutException("Timed out waiting for TCP client connection.");
        }

        private bool IsPortAvailable(int port)
        {
            TcpListener? tempListener = null;
            try
            {
                tempListener = new TcpListener(IPAddress.Loopback, port);
                tempListener.Start();
                return true;
            }
            catch (SocketException ex)
            {
                Debug.WriteLine($"Port {port} is not available: {ex.Message}");
                return false;
            }
            finally
            {
                tempListener?.Stop();
            }
        }

        private bool CheckAndKillPort(int port)
        {
            try
            {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = "netstat";
                    process.StartInfo.Arguments = "-ano";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (line.Contains($":{port}"))
                        {
                            var columns = Regex.Split(line.Trim(), @"\s+");
                            if (columns.Length >= 5 && int.TryParse(columns[4], out int pid))
                            {
                                try
                                {
                                    Process.GetProcessById(pid).Kill();
                                    Debug.WriteLine($"Killed process with PID {pid} using port {port}.");
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Error killing PID {pid}: {ex.Message}");
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking/killing port {port}: {ex.Message}");
                return false;
            }
        }

        private bool IsDeviceConnected()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < 30000)
            {
                try
                {
                    if (Command($"adb -s {_device.Serial} shell echo ok").Trim() == "ok")
                    {
                        return true;
                    }
                }
                catch
                {
                }
                Command($"adb -s {_device.Serial} reconnect");
                Thread.Sleep(500);
            }
            Debug.WriteLine("Device connection check failed.");
            return false;
        }

        public void Close(bool isStop = true)
        {
            try
            {
                cts?.Cancel();
                CleanUp();
                CommandBatch(new[] { $"adb -s {_device.Serial} reverse --remove-all" });
                Command($"adb -s {_device.Serial} shell rm /data/local/tmp/scrcpy");
                Connected = false;
                Debug.WriteLine("Scrcpy closed successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error closing scrcpy: {ex.Message}");
            }
        }
    }
}
