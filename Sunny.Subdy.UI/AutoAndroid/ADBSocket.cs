﻿using System.Globalization;
using System.Net;
using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutoAndroid
{
    public class ADBSocket : IDisposable
    {
        private const string DefaultEncoding = "ISO-8859-1";
        public static Encoding Encoding { get; } = Encoding.GetEncoding(DefaultEncoding);

        private readonly static string OKAY = "OKAY";
        private readonly static string FAIL = "FAIL";
        private readonly Socket _socket;
        private readonly string _serial;
        private readonly int _port;
        public static int ReceiveBufferSize { get; set; } = 40960;

        private bool IsDebug = false;

        public static ADBSocket Create(string serial)
        {
            return new ADBSocket(serial);
        }

        public ADBSocket(string serial, int port = 5037)
        {
            _serial = serial;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.ReceiveTimeout = 5000;
            _socket.SendTimeout = 5000;
            _port = port;
            SafeConnect();
        }

        public static ADBSocket Create()
        {
            return new ADBSocket();
        }

        internal ADBSocket(int port = 5037)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _port = port;
            SafeConnect();
        }

        #region Connect
        private void Connect()
        {
            _socket.Connect("127.0.0.1", _port);
        }

        private void SafeConnect()
        {
            try
            {
                Connect();
            }
            catch (Exception)
            {
                ADBHelper.StartServer();
                Connect();
            }
        }
        #endregion

        #region OpenTransport
        public void OpenTransport(params object[] argv)
        {
            string command = string.Join(":", new string[] { "host", "transport", _serial });
            if (argv.Length > 0)
            {
                var list = new List<object>() { "host-serial", _serial };
                list.AddRange(argv);
                command = string.Join(":", list);
            }
            string resultStr = string.Format("{0}{1}", command.Length.ToString("X4"), command);
            byte[] result = Encoding.GetBytes(resultStr);
            _socket.Send(result);
            CheckOkay();
        }
        #endregion

        #region Forward
        public void Forward(string local, string remote, bool norebind = false)
        {
            var list = new List<string> {
                "forward"
            };
            if (norebind)
            {
                list.Add("norebind");
            }
            list.Add($"{local};{remote}");
            using (ADBSocket socket = Create(_serial))
            {
                socket.OpenTransport(list.ToArray());
            }
        }

        public int ForwardPort(int remote)
        {
            var list = ForwardList();
            foreach (var item in list)
            {
                if (item.Remote == remote && _serial == item.Serial)
                {
                    return item.Local;
                }
            }
            int port = GetFreePort();
            Forward($"tcp:{port}", $"tcp:{remote}");
            return port;
        }

        private List<ForwardItem> ForwardList()
        {
            var list = new List<ForwardItem>();
            OpenTransport("list-forward");
            var result = ReadToEnd();
            if (string.IsNullOrWhiteSpace(result))
            {
                return list;
            }
            var sp = result.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in sp)
            {
                var ls = line.Split(' ');
                if (ls.Length != 3)
                {
                    continue;
                }
                var item = new ForwardItem()
                {
                    Serial = ls[0],
                    Local = int.TryParse(ls[1].Split(':')[1], out int local) ? local : -1,
                    Remote = ls[2].Contains(':') ? (int.TryParse(ls[2].Split(':')[1], out int remote) ? remote : -1) : int.Parse(ls[2]),
                };
                list.Add(item);
            }
            return list;
        }

        #endregion

        #region Shell
        public static string Shell(string serial, params object[] argv)
        {
            using (ADBSocket socket = new ADBSocket(serial))
            {
                socket.OpenTransport();
                return socket.Shell(argv);
            }
        }

        private string Shell(params object[] argv)
        {
            string command = "shell:";
            command += string.Join(" ", argv);
            string resultStr = string.Format("{0}{1}", command.Length.ToString("X4"), command);
            byte[] result = Encoding.GetBytes(resultStr);
            _socket.Send(result);
            CheckOkay();
            return ReadToClose().Strip();
        }
        #endregion

        #region Push
        public static bool Push(string serial, string file, string path, int mode)
        {
            if (!File.Exists(file))
            {
                return false;
            }
            using (ADBSocket socket = new ADBSocket(serial))
            {
                socket.OpenTransport();
                socket.Sync();
                mode = 32768 | mode;
                string temp = $"{path},{mode}";
                socket._socket.Send(Encoding.GetBytes("SEND"));
                socket._socket.Send(BitConverter.GetBytes(temp.Length));
                socket._socket.Send(Encoding.GetBytes(temp));
                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] buffer = new byte[4096];
                    int size = stream.Read(buffer, 0, buffer.Length);
                    while (size > 0)
                    {
                        socket._socket.Send(Encoding.GetBytes("DATA"));
                        socket._socket.Send(BitConverter.GetBytes(size));
                        socket._socket.Send(buffer, size, SocketFlags.None);
                        size = stream.Read(buffer, 0, buffer.Length);
                    }
                    int time = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
                    socket._socket.Send(Encoding.GetBytes("DONE"));
                    socket._socket.Send(BitConverter.GetBytes(time));
                    byte[] buf = new byte[4];
                    socket._socket.Receive(buf, 0, buf.Length, SocketFlags.None);
                    string state = Encoding.GetString(buf);
                    if (state == FAIL)
                    {
                        string errorMessage = socket.ReadToClose(); // Read error details for potential handling
                        return false;
                    }
                    return state == OKAY;
                }
            }
        }
        #endregion

        #region Sync
        private void Sync()
        {
            string command = "sync:";
            string resultStr = string.Format("{0}{1}", command.Length.ToString("X4"), command);
            byte[] result = Encoding.GetBytes(resultStr);
            _socket.Send(result);
            CheckOkay();
        }
        #endregion

        #region GetState
        public string GetState()
        {
            OpenTransport("get-state");
            return ReadToEnd();
        }
        #endregion

        #region Command

        public string Command(params object[] argv)
        {
            string command = string.Join(":", argv);
            string resultStr = string.Format("{0}{1}", command.Length.ToString("X4"), command);
            byte[] result = Encoding.GetBytes(resultStr);
            _socket.Send(result);
            CheckOkay();
            return ReadToEnd();
        }

        #endregion

        #region CheckOkay
        private void CheckOkay()
        {
            byte[] buf = new byte[4];
            _socket.Receive(buf, 0, 4, SocketFlags.None);
            string state = Encoding.GetString(buf);
            if (state == OKAY)
            {
                return;
            }
            else if (state == FAIL)
            {
                throw new Exception($"FAIL: {ReadToEnd()}");
            }
            else
            {
                throw new Exception("未知错误");
            }
        }
        #endregion

        #region ReadToEnd
        private string ReadToEnd()
        {
            byte[] reply = new byte[4];
            var message = Read(reply);
            if (message == 0)
            {
                return null;
            }
            string lenHex = Encoding.GetString(reply);
            int len = int.Parse(lenHex, NumberStyles.HexNumber);
            reply = new byte[len];
            Read(reply);
            string value = Encoding.GetString(reply);
            return value;
        }

        private string ReadToClose()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] buffer = new byte[4096];
                int len = _socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                while (len > 0)
                {
                    stream.Write(buffer, 0, len);
                    len = _socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                }
                buffer = stream.ToArray();
                return Encoding.GetString(buffer);
            }
        }

        private int Read(byte[] data)
        {
            return Read(data, data.Length);
        }

        private int Read(byte[] data, int length)
        {
            int expLen = length != -1 ? length : data.Length;
            int count = -1;
            int totalRead = 0;
            while (count != 0 && totalRead < expLen)
            {
                try
                {
                    int left = expLen - totalRead;
                    int len = left < ReceiveBufferSize ? left : ReceiveBufferSize;
                    byte[] buffer = new byte[len];
                    count = _socket.Receive(buffer, len, SocketFlags.None);
                    if (count < 0)
                    {
                        throw new Exception("EOF");
                    }
                    else if (count == 0)
                    {

                    }
                    else
                    {
                        Array.Copy(buffer, 0, data, totalRead, count);
                        totalRead += count;
                    }
                }
                catch (SocketException sex)
                {
                    throw new Exception(string.Format("No Data to read: {0}", sex.Message));
                }
            }
            return totalRead;
        }
        #endregion

        #region Port
        private bool CheckPortUse(int port)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socket.Connect("127.0.0.1", port);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private int GetFreePort()
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0));
                    return (socket.LocalEndPoint as IPEndPoint).Port;
                }
            }
            catch (Exception)
            {
                for (int i = 0; i < 20; i++)
                {
                    int port = new Random().Next(10000, 20000);
                    if (!CheckPortUse(port))
                    {
                        return port;
                    }
                }
            }
            throw new Exception("No free port found");
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            try { _socket.Shutdown(SocketShutdown.Both); } catch { }
            try { _socket.Close(); } catch { }
            try { _socket.Dispose(); } catch { }
        }
        #endregion
    }


}
