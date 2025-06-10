using System.Buffers;
using System.Buffers.Binary;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using AutoAndroid;

namespace Sunny.Subdy.UI.View.DeviceControl
{
    public class Scrcpy
    {
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public long Bitrate { get; set; } = 2000000;
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
        Process? _Process;
        ScrcpyDisplay scrcpyDisplay;

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
                int result = -1;
                string text = Command($"adb  -s {deviceId}  reverse --list");
                if (string.IsNullOrEmpty(text)) return GetNewAvailablePort();

                var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    try
                    {
                        if (line.Contains("tcp:"))
                        {
                            // Sử dụng regex để tìm port sau "tcp:"
                            var match = Regex.Match(line, @"tcp:(\d+)");

                            if (match.Success)
                            {
                                string tcpPort = match.Groups[1].Value; // Lấy giá trị trong nhóm đầu tiên (sau "tcp:")

                                if (string.IsNullOrEmpty(tcpPort)) continue;

                                // Chuyển đổi tcpPort thành một số nguyên nếu cần
                                return Convert.ToInt32(tcpPort);
                            }
                        }
                    }
                    catch
                    {

                    }

                }
                return GetNewAvailablePort();
            }
            catch
            {

            }

            return GetNewAvailablePort();
        }
        private static int GetNewAvailablePort()
        {
            int port = 0;
            TcpListener? listener = null;

            while (true)
            {
                try
                {
                    port = new Random().Next(1024, 65536);
                    listener = new TcpListener(IPAddress.Loopback, port);
                    listener.Start();
                    break;
                }
                catch (SocketException)
                {
                    continue;
                }
                finally
                {
                    listener?.Stop();
                }
            }

            return port;
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
            await Connect();
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
            catch
            {
            }
        }

        private async Task Connect()
        {
            cts = new CancellationTokenSource();
            int retryCounter = 0;
            int delayMs = 1000; // Bắt đầu với 1 giây
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    CloseConnections();
                    retryCounter++;

                    if (!await EnsureDeviceIsConnected(retryCounter, cts.Token))
                    {
                        continue;
                    }

                    await ResetAdbPortForwarding();
                    MobileServerSetup();

                    if (!StartListener(_device.PortScrcpy))
                    {
                        continue;
                    }

                    MobileServerStart();

                    if (!await WaitForClientConnections())
                    {
                        continue;
                    }
                    await StartProcessing();
                }
                catch (OperationCanceledException)
                {
                }
                catch (TimeoutException ex)
                {
                }
                catch (SocketException ex)
                {
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    CleanUp();
                    LoadLogo(); // Đảm bảo logo hiển thị khi stream dừng
                }

                // Thêm delay trước khi thử lại
                await Task.Delay(delayMs);
                delayMs = Math.Min(delayMs * 2, 10000); // Tăng delay, tối đa 10 giây
            }
        }

        private async Task<bool> EnsureDeviceIsConnected(int retryCounter, CancellationToken token)
        {
            if (!IsDeviceConnected())
            {
                LoadLogo();
                await Task.Delay(1000, token).ConfigureAwait(false);
                return false;
            }
            return true;
        }

        private async Task ResetAdbPortForwarding()
        {
            CommandBatch(new[] { $"adb -s {_device.Serial} reverse --remove-all" });
        }

        private bool StartListener(int port)
        {
            if (!IsPortAvailable(port))
            {
                return false;
            }

            listener = new TcpListener(IPAddress.Loopback, port);
            listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            listener.Start();
            return true;
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
                return true;
            }
            catch (TimeoutException ex)
            {
                return false;
            }
        }

        private async Task StartProcessing()
        {
            var videoTask = VideoMain();
            var controlTask = ControllerMain();
            await Task.WhenAll(videoTask, controlTask);
        }
        private byte[] _lastFrameHash = null;
        private bool IsNewFrame(byte[] data, int length)
        {
            using (var sha1 = SHA1.Create())
            {
                var currentHash = sha1.ComputeHash(data, 0, length);
                if (_lastFrameHash != null && _lastFrameHash.SequenceEqual(currentHash))
                    return false;

                _lastFrameHash = currentHash;
                return true;
            }
        }
        private async Task VideoMain()
        {
            try
            {
                if (videoClient == null) return;

                var videoStream = videoClient.GetStream();
                videoStream.ReadTimeout = 2000;

                var metaBuf = pool.Rent(12);
                var packetBuf = pool.Rent(1024 * 1024);
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        // Đọc metadata (12 bytes)
                        int bytesRead = await ReadFullyAsync(videoStream, metaBuf, 12, cts.Token);
                        if (bytesRead != 12)
                        {
                            _device.IsScrcpy = false;
                            cts.Cancel();
                            break;
                        }
                        long presentationTimeUs = BitConverter.ToInt64(metaBuf.Take(8).Reverse().ToArray(), 0);
                        int packetSize = BitConverter.ToInt32(metaBuf.Skip(8).Take(4).Reverse().ToArray(), 0);

                        // Đảm bảo buffer đủ lớn để đọc frame
                        if (packetSize > packetBuf.Length)
                        {
                            pool.Return(packetBuf);
                            packetBuf = pool.Rent(packetSize);
                        }

                        // Đọc frame dữ liệu
                        bytesRead = await ReadFullyAsync(videoStream, packetBuf, packetSize, cts.Token);
                        if (bytesRead != packetSize)
                        {
                            _device.IsScrcpy = false;
                            cts.Cancel();
                            break;
                        }

                        if (!cts.Token.IsCancellationRequested)
                        {
                            VideoStreamDecoder?.Decode(packetBuf, presentationTimeUs);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        cts.Cancel();
                        break; // Dừng nếu nhận yêu cầu hủy
                    }
                    catch
                    {
                        cts.Cancel();
                        _device.IsScrcpy = false;
                        break; // Xử lý lỗi đọc dữ liệu
                    }
                }

                // Trả lại buffer về pool khi xong
                pool.Return(metaBuf);
                pool.Return(packetBuf);
            }
            catch
            {

                // Xử lý ngoại lệ cấp cao
            }
            return;
        }

        private async Task<int> ReadFullyAsync(Stream stream, byte[] buffer, int length, CancellationToken cancellationToken)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < length)
            {
                int bytesRead = await stream.ReadAsync(buffer, totalBytesRead, length - totalBytesRead, cancellationToken);
                if (bytesRead == 0)
                {
                    break; // Stream kết thúc
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }

        private async Task ControllerMain()
        {
            if (controlClient == null || cts == null) return;
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

        private void ControllerSend(NetworkStream stream, IControlMessage cmd)
        {
            try
            {
                var bytes = cmd.ToBytes();
                stream.Write(bytes);
            }
            catch
            {
            }
        }

        private void MobileServerSetup()
        {
            UploadMobileServer();
            CommandBatch(new[] { $"adb -s {_device.Serial} reverse localabstract:scrcpy tcp:{_device.PortScrcpy}" });
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
                    string command = $"adb -s {_device.Serial} push {filePath} {remotePath}";
                    string text = Command(command);
                    if (string.IsNullOrEmpty(text)) continue;
                    string value = Shell("ls /data/local/tmp/scrcpy-server.jar").Trim();
                    if (value == remotePath)
                    {
                        return;
                    }
                }
                catch
                {
                }
            }
        }

        private void MobileServerStart()
        {
            RunServer();
        }

        private void RunServer()
        {
            string cmd = $"adb -s {_device.Serial} shell CLASSPATH=/data/local/tmp/scrcpy-server.jar app_process / com.genymobile.scrcpy.Server 1.23 bit_rate=2000000 max_fps=30 tunnel_forward=false control=true display_id=0 show_touches=false stay_awake=false power_off_on_close=false downsize_on_error=true cleanup=true max_size=1280 lock_video_orientation=0";
            _Process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{cmd}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            _Process.Start();
        }

        public static string Command(string cmd, int timeout = 10)
        {
            string result = "";
            string output = "";
            string error = "";
            int retryCount = 0;
            int maxRetries = 3;

            while (retryCount < maxRetries)
            {
                result = "";
                try
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments =  $"/C \"{ProcessHelper.ADBPath}{cmd}\"";
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
                            retryCount++;
                            continue;
                        }
                        if (string.IsNullOrEmpty(error))
                        {
                            return output.Trim();
                        }
                        return output.Trim();
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
            }
            return result;
        }

        public static string CommandBatch(string[] commands, int timeout = 10)
        {
            string cmd = string.Join(" && ", commands);
            return Command(cmd, timeout);
        }

        public string Shell(string command)
        {
            command = $"adb -s {_device.Serial} shell {command}";
            return Command(command);
        }

        private void ReadDeviceInfo()
        {
            try
            {
                if (videoClient == null)
                    return;

                var infoStream = videoClient.GetStream();
                infoStream.ReadTimeout = 2000;

                // Read 68-byte header.
                var deviceInfoBuf = pool.Rent(68);
                int bytesRead = infoStream.Read(deviceInfoBuf, 0, 68);

                if (bytesRead != 68)
                {
                    _device.IsScrcpy = false;
                }
                var deviceInfoSpan = deviceInfoBuf.AsSpan();
                Width = BinaryPrimitives.ReadInt16BigEndian(deviceInfoSpan[64..]);
                Height = BinaryPrimitives.ReadInt16BigEndian(deviceInfoSpan[66..]);

                pool.Return(deviceInfoBuf);
            }
            catch
            {
            }
        }

        private void LoadLogo()
        {
            try
            {
                scrcpyDisplay.View.Image = Properties.Resources.LamTool_net;
            }
            catch (Exception ex)
            {
            }
        }

       

     
        private void CloseConnections()
        {
            listener?.Stop();
            videoClient?.Close();
            controlClient?.Close();
        }

        private void CleanUp()
        {
            CloseConnections();
        }

        private async Task<TcpClient> AcceptClientWithTimeout(TcpListener listener, int timeoutMs, CancellationToken token)
        {
            var acceptTask = listener.AcceptTcpClientAsync();
            if (await Task.WhenAny(acceptTask, Task.Delay(timeoutMs, token)) == acceptTask)
            {
                return acceptTask.Result;
            }
            throw new TimeoutException();
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
            catch (SocketException)
            {
                return false;
            }
            finally
            {
                tempListener?.Stop();
            }
        }

        private bool IsDeviceConnected()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < 30000)
            {
                if (Command($"adb -s {_device.Serial} shell echo ok").Trim() == "ok")
                {
                    return true;
                }
                Command($"adb -s {_device.Serial} reconnect");
                Command($"adb -s {_device.Serial} connect");
                Application.DoEvents();
                Thread.Sleep(500);
            }
            return Command($"adb -s {_device.Serial} shell echo ok").Trim() == "ok";
        }

        public void Close(bool isStop = true)
        {
            try
            {
                cts?.Cancel();
            }
            catch
            {
            }

            if (_Process != null)
            {
                try
                {
                    _Process.Dispose();
                }
                catch
                {
                }
            }

            if (videoClient != null)
            {
                try
                {
                    videoClient.Dispose();
                }
                catch
                {
                }
            }

            if (controlClient != null)
            {
                try
                {
                    controlClient.Dispose();
                }
                catch
                {
                }
            }

            if (isStop)
            {
                try
                {
                    cts?.Cancel();
                }
                catch
                {
                }
            }

            CommandBatch(new[] { $"adb -s {_device.Serial} reverse --remove-all" });
        }
    }

}
