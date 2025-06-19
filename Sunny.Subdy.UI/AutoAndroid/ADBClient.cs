using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json.Linq;
using static AutoAndroid.AtxDeviceInfo;

namespace AutoAndroid
{
    public class ADBClient
    {
        Random random = new Random();
        Stopwatch stopwatch = new Stopwatch();
        public bool Running { get; set; } = true;
        public LogHelper LogHelper
        {
            get => _logHelper ??= new LogHelper(Device);
            set => _logHelper = value;
        }
        private LogHelper _logHelper;
        public ATXService ATX
        {
            get => _atx ??= new ATXService(this);
            set => _atx = value;
        }
        private ATXService _atx;
        public ADBHelper ADB
        {
            get => _adb ??= new ADBHelper(this);
            set => _adb = value;
        }
        private ADBHelper _adb;
        MaxChangeService maxChange;
        public DeviceModel Create(string serial)
        {
            DeviceModel model = new DeviceModel();
            model.Serial = serial;
            using var _client = new ADBSocket(serial);

            model.Port = _client.ForwardPort(7912);
            string name = ProcessHelper.RunAdbWithTimeout($"-s {serial} shell settings get global device_name");
            string version = ProcessHelper.RunAdbWithTimeout($"-s {serial} shell getprop ro.build.version.release");
            model.NameDevice = name;
            model.OS = version;
            model.TypeColor = 0;
            model.Check = false;

            return model;
        }
        public DeviceModel Device { get; set; }
        public ADBClient(DeviceModel model)
        {
            Device = model;
            _atx = new ATXService(this);
            _logHelper = new LogHelper(Device);
            maxChange = new MaxChangeService(this);
        }
        public void EnableWifi()
        {
            _logHelper.SUCCESS($"Bật wiffi");
            Shell("su -c 'svc wifi enable'");
            _logHelper.SUCCESS($"Đã bật wiffi");
        }
        public void DisableWifi()
        {
            _logHelper.SUCCESS($"Tắt wiffi");
            Shell("su -c 'svc wifi disable'");
            _logHelper.SUCCESS($"Đã tắt wiffi");
        }
        public ADBClient(string serial)
        {
            Device = Create(serial);
            _atx = new ATXService(this);
            _logHelper = new LogHelper(Device);
            maxChange = new MaxChangeService(this);
        }

        public int ForwardPort(int remote, int port)
        {
            using var adb = new ADBSocket(Device.Serial);
            return adb.ForwardPort(remote);
        }
        public bool IsRunningApp(string package)
        {
            string dump = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                dump = GetXMLSource();
                if (string.IsNullOrEmpty(dump)) continue;
                if (dump.Contains(package))
                {
                    return true;
                }
            }
            return false;
        }
        public bool RebootAndWaitForDeviceReady()
        {
            LogHelper.SUCCESS("Đang khởi động lại máy!");
            ADB.Shell("reboot");
            LogHelper.SUCCESS("Đang chờ khởi động máy!");
            Shell("wait-for-device", 120);
            while (!ADB.Shell("getprop sys.boot_completed").Equals("1"))
            {
                LogHelper.SUCCESS("Khởi động máy thành công!");
                Delay(1);
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < 15000)
            {

                if (Connect())
                {
                    return true;
                }
            }
            return false;
        }
        public void Enabel4G()
        {
            LogHelper.SUCCESS($"Đang bật 4G");
            Shell("svc data enable");
            LogHelper.SUCCESS($"Đã bật 4G");
        }
        public void Disable4G()
        {
            LogHelper.SUCCESS($"Đang tắt 4G");
            Shell("svc data disable");
            LogHelper.SUCCESS($"Đã tắt 4G");
        }
        public bool ChangInfo(string uid, bool backup, string brand)
        {
            if (Shell(" su -c \"whoami\"").Trim() != "root")
            {
                LogHelper.ERROR("Không phải root");
                return false;
            }
            LogHelper.SUCCESS($"Đang thay đổi thiết bị!");
            MaxChangeService maxChangeService = new MaxChangeService(this);
            return maxChangeService.Change(uid, backup, brand);
        }
        public string GetDeviceName()
        {
            MaxChangeService maxChangeService = new MaxChangeService(this);
            return maxChangeService.GetInfoDeviceName(10);
        }
        public bool ConnectProxy(string proxy)
        {
            LogHelper.SUCCESS($"Đang change proxy: {proxy}");
            VATProxyService proxyService = new VATProxyService(this);
            return proxyService.ConnectProxy(proxy);
        }
        private bool College_Proxy(string proxy)
        {
            if (string.IsNullOrEmpty(proxy)) return false;
            string[] proxyParts = proxy.Split(':');
            if (proxyParts.Length < 2) return false;



            List<string> proxyList = new List<string>
            {
                "//*[@text=\"OK\"]",
                "//*[@text=\"START PROXY SERVICE\"]",
                "//*[@text=\"STOP PROXY SERVICE\"]",

            };

            for (int i = 0; i < 5; i++)
            {
                AppClear("com.cell47.College_Proxy");
                AppStart("com.cell47.College_Proxy", true, true, wait: true);
                stopwatch.Restart();
                while (stopwatch.ElapsedMilliseconds < 60000)
                {

                    var element = FindElement("", proxyList, 10);
                    if (string.IsNullOrEmpty(element)) break;
                    switch (element)
                    {
                        case "//*[@text=\"STOP PROXY SERVICE\"]":
                            {

                                return true;
                            }
                        case "//*[@text=\"OK\"]":
                            {
                                ElementWithAttributes(element, 1, "", true);
                                break;
                            }
                        case "//*[@text=\"START PROXY SERVICE\"]":
                            {
                                SendTextADB("//*[@resource-id=\"com.cell47.College_Proxy:id/editText_address\"]", proxyParts[0], 1, "", true);
                                SendTextADB("//*[@resource-id=\"com.cell47.College_Proxy:id/editText_port\"]", proxyParts[1], 1, "", true);
                                ElementWithAttributes(element, 1, "", true);

                                break;
                            }
                    }

                }
            }


            return false;
        }





        public bool Connect()
        {
            try
            {
                if (!ConnectAdb())
                {
                    Device.TypeColor = 1;
                    LogHelper.Log("Không thể kết nối với thiết bị");
                    return false;
                }

                if (!_atx.Connect())
                {
                    Device.TypeColor = 1;
                    LogHelper.Log("Không thể kết nối với thiết bị với UI2");
                    return false;
                }
                LogHelper.Log($"adb start [{Device.Port}]");
                Device.TypeColor = 2;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log($"Connect Exception: {ex.Message}");
                return Connect(); // gọi lại, và phải return kết quả
            }
        }
        public bool ConnectAdb()
        {
            stopwatch.Start();
            LogHelper.Log($"Đang connect");
            while (Running)
            {
                string text = ProcessHelper.RunAdbWithTimeout($"-s {Device.Serial} shell service check settings", 1);
                if (!text.Contains("not found"))
                {
                    stopwatch.Stop();
                    LogHelper.Log($"Đã connect [{stopwatch.ElapsedMilliseconds} ms]");
                    return true;
                }
                LogHelper.Log($"Mất kết nối, chờ ExecuteAdb [{stopwatch.ElapsedMilliseconds / 1000.0:0.00}] cmd: Can't find service: settings");
                ProcessHelper.RunAdbCommand($"-s {Device.Serial} shell reconnect");
            }
            return false;
        }
        public void AppClear(string package)
        {
            try
            {
                LogHelper.SUCCESS($"Xóa dữ liệu app [{package}]");
                for (int i = 0; i < 5; i++)
                {
                    if (Shell("pm clear " + package, 3).Contains("Success"))
                    {
                        break;
                    }
                }
            }
            catch
            {

            }

        }
        public string Shell(params object[] argv)
        {
            const int maxRetry = 3;
            int retry = 0;
            string result = "";

            while (retry < maxRetry)
            {
                try
                {
                    return ADBSocket.Shell(Device.Serial, argv);


                }
                catch (Exception ex)
                {
                    LogHelper.Log($"[Shell] Exception lần {retry + 1}: {ex.Message}");
                    Connect();
                }

                retry++;
                Thread.Sleep(200); // nghỉ giữa các lần thử
            }

            LogHelper.Log("[Shell] Thất bại sau 3 lần thử");
            return result;
        }
        public AndroidAppInfo AppInfo(string package)
        {
            string result = ADBSocket.Shell(Device.Serial, "pm", "path", package);

            if (string.IsNullOrWhiteSpace(result) || !result.StartsWith("package:"))
                return null;

            var lines = result.Split('\n');
            var info = new AndroidAppInfo
            {
                PackageName = package,
                Path = lines[0].Split(':')[1].Strip()
            };

            if (lines.Length > 1)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    info.SubApkPaths.Add(lines[i].Split(':')[1].Strip());
                }
            }

            info.Dumpsys = ADBSocket.Shell(Device.Serial, "dumpsys", "package", package);
            if (string.IsNullOrWhiteSpace(info.Dumpsys))
                return null;

            Match match;

            match = Regex.Match(info.Dumpsys, "versionName=(?<name>[^\\s]+)");
            if (match.Success) info.VersionName = match.Groups["name"].Value;

            match = Regex.Match(info.Dumpsys, "versionCode=(?<code>\\d+)");
            if (match.Success) info.VersionCode = match.Groups["code"].Value;

            match = Regex.Match(info.Dumpsys, "PackageSignatures\\{.*?\\[(?<signatures>.*?)\\]");
            if (match.Success) info.Signature = match.Groups["signatures"].Value;

            match = Regex.Match(info.Dumpsys, "firstInstallTime=(?<time>[-\\d]+\\s[:\\d]+)");
            if (match.Success) info.FirstInstallTime = match.Groups["time"].Value;

            match = Regex.Match(info.Dumpsys, "lastUpdateTime=(?<time>[-\\d]+\\s[:\\d]+)");
            if (match.Success) info.LastUpdateTime = match.Groups["time"].Value;

            match = Regex.Match(info.Dumpsys, "pkgFlags=\\[(?<flags>\\s.*?\\s*)\\]");
            if (match.Success) info.Flags = match.Groups["flags"].Value.Strip();

            return info;
        }

        public List<AndroidProcessItem> GetProcessList()
        {
            string result = ADB.Shell("ps"); // <-- Hoặc thử "ps -ef"

            var list = new List<AndroidProcessItem>();
            if (string.IsNullOrWhiteSpace(result)) return list;

            var lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var pids = new HashSet<string>();

            foreach (var line in lines)
            {
                if (line.StartsWith("USER")) continue; // Bỏ dòng tiêu đề

                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 9) continue; // Tránh lỗi

                string user = parts[0];
                string pidStr = parts[1];
                string name = parts.Last(); // Tên process là cột cuối

                if (!pids.Add(pidStr)) continue;

                list.Add(new AndroidProcessItem
                {
                    User = user,
                    Pid = int.TryParse(pidStr, out var pid) ? pid : -1,
                    Name = name
                });
            }

            return list;
        }
        public void KillProcessByName(string name)
        {
            var list = GetProcessList();
            foreach (var proc in list)
            {
                if (proc.Name.Equals(name) && proc.User == "shell")
                {
                    Shell("kill", "-9", proc.Pid.ToString());
                }
            }
        }

        public string GetProp(string prop)
        {
            return Shell("getprop", prop);
        }

        public bool Push(string file, string path, int mode = 493)
        {
            return ADBSocket.Push(Device.Serial, file, path, mode);
        }

        public bool IsScreenOn()
        {
            return Shell("dumpsys", "power").Contains("mHoldingDisplaySuspendBlocker=true");
        }

        public List<string> AppList(string filter = "")
        {
            var result = Shell("pm", "list", "packages", filter);
            var list = new List<string>();

            if (!string.IsNullOrWhiteSpace(result))
            {
                var lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (line.StartsWith("package:"))
                        list.Add(line.Split(':')[1]);
                }
            }

            return list;
        }
        public void SetSize(int width = 1440, int height = 2560, int density = 560)
        {
            LogHelper.Log($"Thiết lập kích thước màn hình {width}x{height}, DPI: {density}");
            Shell("settings put system accelerometer_rotation 0");
            Shell("settings put system user_rotation 0");
            Shell("wm size", $"{width}x{height}");
            Shell("wm density", density.ToString());
        }

        public List<string> AppRunningList()
        {
            var running = new List<string>();
            var apps = AppList();
            var processes = GetProcessList();

            foreach (var app in apps)
            {
                if (processes.Any(p => p.Name.Contains(app)))
                {
                    running.Add(app);
                }
            }

            return running;
        }

        public void Swipe(int dx, int dy, int ux, int uy, int durationMs = 55, int repeat = 1, int delayBetweenSwipes = 100)
        {
            for (int i = 0; i < repeat; i++)
            {
                try
                {
                    LogHelper.SUCCESS($"Đang vuốt [{i + 1}]");
                    _atx.Swipe(dx, dy, ux, uy, durationMs);
                }
                catch
                {
                    Connect();
                    Swipe(dx, dy, ux, uy, durationMs, repeat, delayBetweenSwipes);
                    return;
                }
                if (i < repeat - 1 && delayBetweenSwipes > 0)
                {
                    Thread.Sleep(delayBetweenSwipes); // Nghỉ giữa các lần vuốt
                }
            }
        }
        public bool Click(float x, float y)
        {
            LogHelper.SUCCESS($"Đang click");
            try
            {
                return _atx.Click(x, y);
            }
            catch
            {
                Connect();
                return Click(x, y);
            }

        }

        public void LongClick(float x, float y, int durationMs = 1000)
        {
            LogHelper.SUCCESS($"Đang long click");
            try
            {
                _atx.LongClick(x, y, durationMs);
            }
            catch
            {
                Connect();
                LongClick(x, y, durationMs);
            }

        }

        public void CLearText()
        {
            LogHelper.SUCCESS($"Đang clear text");
            ImeWait();
            Shell("am", "broadcast", "-a", "ADB_CLEAR_TEXT");
        }
        public void ImeWait(int timeout = 5000)
        {
            long deadline = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeout;
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < deadline)
            {
                bool show = ImeCurrent(out string ime);
                if (!ime.StartsWith("mCurMethodId=com.github.uiautomator/.FastInputIME"))
                {
                    ImeSet(true);
                    Thread.Sleep(500);
                    continue;
                }
                if (show)
                {
                    return;
                }
                Shell("input", "keyevent", "11"); // ESCr
                Thread.Sleep(200);
            }
        }

        public void ImeSet(bool fastime)
        {
            string fast_ime = "com.github.uiautomator/.FastInputIME";
            if (fastime)
            {
                Shell("ime", "enable", fast_ime);
                Shell("ime", "set", fast_ime);
            }
            else
            {
                Shell("ime", "disable", fast_ime);
            }
        }

        public bool ImeCurrent(out string ime)
        {
            var result = Shell("dumpsys", "input_method");
            Regex regex = new Regex("mCurMethodId=([-_./\\w]+)");
            Match match = regex.Match(result);
            if (match.Success)
            {
                ime = match.Groups[0].Value;
            }
            else
            {
                ime = "";
            }
            return result.Contains("mInputShown=true");
        }

        public void SendText(string xpath, string text, int timeout = 10, string xml = "", bool clear = true)
        {
            if (string.IsNullOrEmpty(text)) return;
            ElementWithAttributes(xpath, timeout, xml, true);
            if (clear)
            {
                CLearText(); // Xóa text trước khi nhập
            }
            LogHelper.SUCCESS($"Đang send text : {text}");
            string data = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
            string type = "ADB_INPUT_TEXT";
            Shell("am", "broadcast", "-a", type, "--es", "text", data);
        }
        public void SendTextADB(string xpath, string text, int timeout = 10, string xml = "", bool clear = true)
        {
            if (string.IsNullOrEmpty(text)) return;
            ElementWithAttributes(xpath, timeout, xml, true);
            if (clear)
            {
                CLearText();
            }
            LogHelper.SUCCESS($"Đang send text : {text}");
            Shell("input", "text", text);
        }

        public void SetEnableModuleMaxChange()
        {
            maxChange.SetEnableModule();
        }
        public Bitmap Screenshot()
        {
            return _atx.Screenshot();
        }

        /// <summary>
        /// Gửi từng ký tự trong chuỗi một cách chậm rãi, hỗ trợ tiếng Việt, emoji, v.v.
        /// </summary>
        /// <param name="text">Chuỗi cần nhập</param>
        /// <param name="min">Thời gian delay tối thiểu (ms)</param>
        /// <param name="max">Thời gian delay tối đa (ms)</param>
        public void SendTextSlow(string xpath, string text, int min = 10, int max = 300, int timeout = 10, string xml = "", bool clear = true)
        {
            if (string.IsNullOrEmpty(text)) return;
            ElementWithAttributes(xpath, timeout, xml, true);
            if (clear)
            {
                CLearText(); // Xóa text trước khi nhập
            }
            LogHelper.SUCCESS($"Đang send text : {text}");
            foreach (char c in text)
            {
                string data = Convert.ToBase64String(Encoding.UTF8.GetBytes(c.ToString()));
                string type = "ADB_INPUT_TEXT";
                var s = Shell("am", "broadcast", "-a", type, "--es", "text", data);  // Gửi từng ký tự Unicode
                Thread.Sleep(random.Next(min, max)); // Tạo độ trễ ngẫu nhiên
            }
        }
        public string GetTextFromScreenShotByATX(int timeout = 60000)
        {
            try
            {
                _logHelper.Log("Lấy nội dung từ ảnh chụp màn hình");
                stopwatch.Start();
                while (true)
                {
                    try
                    {
                        // 1. Dùng ATX để chụp ảnh màn hình
                        Bitmap screen = _atx.Screenshot();
                        // 2. Dùng OpenCV OCR để trích xuất text từ ảnh
                        string text = ImageScanOpenCV.GetTextFromImage(screen);

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            stopwatch.Stop();
                            return text;
                        }

                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log(ex.Message);
                        LogHelper.WriteFile(Device.Serial, ex.ToString());
                    }

                    if (stopwatch.ElapsedMilliseconds > timeout)
                    {
                        stopwatch.Stop();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);
                LogHelper.WriteFile(Device.Serial, ex.ToString());
            }

            return string.Empty;
        }
        public string Devices()
        {
            using var adb = new ADBSocket(Device.Serial);
            return adb.Command("host", "devices");
        }

        public T RunTime<T>(string message, Func<T> action, bool showResult = true)
        {
            LogHelper.Log($"{message}");
            int ms;
            T result = RunTimeHelper.Time(action, out ms);
            if (showResult)
                LogHelper.Log($"{message}: {result} ({ms}ms)");
            else
                LogHelper.Log($"{message}: ({ms}ms)");
            LogHelper.Log($"{message}: {result} ({ms}ms)");
            return result;
        }

        public void RunTime(string message, Action action)
        {
            LogHelper.Log($"{message}");
            int ms;
            RunTimeHelper.Time(action, out ms);
            LogHelper.Log($"{message}: ({ms}ms)");
            return;
        }
        public string GetXMLSource()
        {
            try
            {
                LogHelper.SUCCESS($"Đang lấy XML source");
                string value = _atx.DumpHierarchy();
                if (string.IsNullOrEmpty(value))
                {
                    Connect();
                    return GetXMLSource();
                }
                return value;
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);
                Connect();
                return GetXMLSource();
            }
        }

        public List<XmlNode> FindElements(int timeout, string xmlContent, string xpath)
        {
            LogHelper.Log($"Find elements [{xpath}]");
            List<XmlNode> attributeValues = new List<XmlNode>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < timeout * 1000)
            {
                try
                {
                    if (string.IsNullOrEmpty(xmlContent))
                    {
                        xmlContent = GetXMLSource();
                    }
                    if (!string.IsNullOrEmpty(xmlContent))
                    {
                        xmlContent = xmlContent.ToLower();
                        xpath = xpath.ToLower();

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlContent);
                        XmlNodeList nodeList = xmlDoc.SelectNodes(xpath);
                        if (nodeList == null || nodeList.Count == 0)
                        {
                            xmlContent = string.Empty;
                            continue;
                        }
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            try
                            {
                                attributeValues.Add(nodeList[i]);
                            }
                            catch
                            {
                            }
                        }
                        if (attributeValues.Any())
                        {
                            return attributeValues;
                        }
                        xmlContent = string.Empty;
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex.Message);
                }
            }


            return attributeValues;
        }
        public List<XmlNode> FindElements(int timeout, string xmlContent, List<string> xpaths)
        {
            LogHelper.Log($"Find elements");
            List<XmlNode> attributeValues = new List<XmlNode>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < timeout * 1000)
            {
                try
                {
                    if (string.IsNullOrEmpty(xmlContent))
                    {
                        xmlContent = GetXMLSource();
                    }
                    if (!string.IsNullOrEmpty(xmlContent))
                    {
                        xmlContent = xmlContent.ToLower();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlContent);
                        foreach (string xpath in xpaths)
                        {
                            if (string.IsNullOrEmpty(xpath)) continue;

                            string xpathValue = xpath.ToLower();
                            XmlNodeList nodeList = xmlDoc.SelectNodes(xpathValue);
                            if (nodeList == null || nodeList.Count == 0)
                            {
                                continue;
                            }
                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                try
                                {
                                    attributeValues.Add(nodeList[i]);
                                }
                                catch
                                {
                                }
                            }

                        }
                        if (attributeValues.Any())
                        {
                            return attributeValues;
                        }
                    }
                    xmlContent = string.Empty;
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex.Message);
                }
            }


            return attributeValues;
        }
        public List<string> FindElements(string xmlContent, string xpath, int timout = 10)
        {
            List<string> attributeValues = new List<string>();
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < timout * 1000)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(xmlContent))
                        {
                            xmlContent = GetXMLSource();
                        }
                        if (!string.IsNullOrEmpty(xmlContent))
                        {
                            xmlContent = xmlContent.ToLower();
                            xpath = xpath.ToLower();

                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(xmlContent);
                            //Appium
                            XmlNodeList nodeList = xmlDoc.SelectNodes(xpath);
                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                try

                                {
                                    var s = nodeList[i];
                                    attributeValues.Add(nodeList[i].OuterXml);
                                }
                                catch
                                {
                                }
                            }
                        }
                        if (attributeValues.Count > 0)
                        {
                            return attributeValues;
                        }
                        xmlContent = string.Empty;
                    }
                    catch
                    {

                    }

                }


            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);
            }
            return attributeValues;
        }
        public bool ElementWithAttributes(string element, int timeoutInSeconds = 5, string xmlsoucre = "", bool click = true)
        {
            try
            {
                if (string.IsNullOrEmpty(element)) return false;
                LogHelper.Log("Find " + element + "");
                string attributeValue = GetBoundsValues(timeoutInSeconds, xmlsoucre, element).FirstOrDefault();
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    if (click)
                    {
                        var point = new RectangleArea(attributeValue).GetCenterPoint();
                        return Click(Convert.ToSingle(point.X), Convert.ToSingle(point.Y));
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);
            }

            return false;
        }
        public bool ElementWithAttributes(List<string> elements, int timeoutInSeconds = 5, string xmlsoucre = "", bool click = true)
        {
            try
            {
                for (int i = 0; i < timeoutInSeconds; i++)
                {
                    foreach (string element in elements)
                    {
                        LogHelper.SUCCESS("Tap " + element + "");
                        string attributeValue = GetBoundsValues(1, xmlsoucre, element).FirstOrDefault();
                        if (!string.IsNullOrEmpty(attributeValue))
                        {
                            if (click)
                            {
                                var point = new RectangleArea(attributeValue).GetCenterPoint();
                                return Click(Convert.ToSingle(point.X), Convert.ToSingle(point.Y));
                            }
                            return true;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                LogHelper.ERROR(ex.Message);
            }

            return false;
        }
        public List<string> GetBoundsValues(int timeoutInSeconds, string XMLString, string xpath)
        {
            List<string> list = new List<string>();
            try
            {
                int tickCount = Environment.TickCount;
                while (true)
                {
                    if (string.IsNullOrEmpty(XMLString))
                    {
                        XMLString = GetXMLSource();
                    }
                    list = GetAttributeValuesFromXmlNodes(XMLString, xpath);
                    if (list.Count <= 0 && timeoutInSeconds != 0)
                    {
                        XMLString = "";
                        if (Environment.TickCount - tickCount >= timeoutInSeconds * 1000)
                        {
                            break;
                        }
                        continue;
                    }
                    break;
                }
            }
            catch (Exception exception_)
            {
                LogHelper.ERROR(exception_.Message);
            }
            return list.Distinct().ToList();
        }
        public List<string> GetAttributeValuesFromXmlNodes(string xmlContent, string xpath, string attributeName = "bounds")
        {
            List<string> attributeValues = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(xmlContent))
                {
                    xmlContent = GetXMLSource();
                }
                if (!string.IsNullOrEmpty(xmlContent))
                {
                    xmlContent = xmlContent.ToLower();
                    xpath = xpath.ToLower();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlContent);
                    //Appium
                    XmlNodeList nodeList = xmlDoc.SelectNodes(xpath);
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        try
                        {
                            attributeValues.Add(nodeList[i].Attributes[attributeName].Value);
                        }
                        catch
                        {
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.ERROR(ex.Message);
            }
            return attributeValues;
        }
        public bool UninstallApp(string packageName)
        {
            try
            {
                string result = RunTime($"Gỡ cài đặt [{packageName}]", () =>
                {
                    return Shell("pm", "uninstall", packageName);
                });

                if (result.Contains("Success"))
                {
                    LogHelper.Log($"Đã gỡ cài đặt {packageName}");
                    return true;
                }
                else
                {
                    LogHelper.Log($"Gỡ cài đặt thất bại: [{packageName}] - Kết quả: {result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log($"Lỗi khi gỡ cài đặt {packageName}: {ex.Message}");
                Connect();

                return UninstallApp(packageName);
            }
            return false;
        }
        public void StopApp(string packageName)
        {
            try
            {
                RunTime($"Dừng ứng dụng [{packageName}]", () =>
                {
                    string result = Shell("am", "force-stop", packageName);

                    if (!string.IsNullOrWhiteSpace(result) &&
                        (result.Contains("Error") || result.Contains("Unknown package")))
                    {
                        LogHelper.Log($"Không thể dừng app: {result}");
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Log($"Lỗi khi dừng app {packageName}: {ex.Message}");
            }
        }

        public bool InstallApp(string path_APK)
        {
            string fileName = Path.GetFileName(path_APK);
            string remotePath = InitHelper.ANDROID_LOCAL_TMP_PATH + fileName;

            LogHelper.SUCCESS($"Cài đặt [{fileName}]");

            // Tắt xác minh cài đặt nếu có thể (tùy thiết bị)
            Shell("settings", "put", "global", "verifier_verify_adb_installs", "0");

            for (int i = 0; i < 5; i++)
            {
                string result = "";
                if (Push(path_APK, remotePath))
                {
                    result = Shell("pm", "install", remotePath);

                    if (result.Contains("Success"))
                    {
                        return true;
                    }
                    else
                    {
                        result = ProcessHelper.RunAdbCommand($"-s {Device.Serial} install '{path_APK}'", 60);
                    }
                    if (result.Contains("success"))
                        return true;
                }
                else
                {
                    result = ProcessHelper.RunAdbCommand($"-s {Device.Serial} install '{path_APK}'", 60);
                }
                if (result.Contains("success"))
                    return true;
                Delay(3);
            }

            return false;
        }
        public bool FindElementIsExistOrClickByPackage(string name, string package, int timeout = 3, bool isClick = false, string XMLString = "")
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < timeout * 1000)
                {
                    if (string.IsNullOrEmpty(XMLString))
                    {
                        XMLString = GetXMLSource();
                    }
                    if (!string.IsNullOrEmpty(XMLString))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(XMLString);
                        //Appium
                        XmlNodeList elements = xmlDoc.SelectNodes($"//node[@package='{package}']");

                        if (elements != null && elements.Count > 0)
                        {
                            foreach (XmlNode element in elements)
                            {
                                // Kiểm tra thuộc tính "text" hoặc "content-desc"
                                string textValue = element.Attributes["text"]?.Value;
                                string contentDescValue = element.Attributes["content-desc"]?.Value;
                                string resourceId = element.Attributes["resource-id"]?.Value;
                                if (textValue != null && textValue.Contains(name) ||
                                    contentDescValue != null && contentDescValue.Contains(name) ||
                                    resourceId != null && resourceId.Contains(name))
                                {
                                    if (isClick)
                                    {
                                        if (element.Attributes["bounds"] != null)
                                        {
                                            string bounds = element.Attributes["bounds"].Value;
                                            var point = new RectangleArea(bounds).GetCenterPoint();
                                            Click(point.X, point.Y);
                                        }
                                        else
                                        {
                                            LogHelper.ERROR("Bounds attribute not found for element.");
                                        }
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                    if (timeout != 0)
                    {
                        Delay(1);
                        XMLString = "";
                        continue;
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ERROR($"{nameof(FindElementIsExistOrClickByPackage)}, Error; {ex.Message}, Exception; {ex}");

            }
            return false;

        }
        public void AppStart(string package, bool monkey = false, bool stop = false, bool wait = false, string activity = null)
        {
            if (stop)
            {
                StopApp(package);
            }
            if (monkey)
            {
                Shell("monkey", "-p", package, "-c", "android.intent.category.LAUNCHER", "1");
                if (wait)
                {
                    AppWait(package);
                }
                return;
            }
            if (string.IsNullOrWhiteSpace(activity))
            {
                var info = _atx.GetAppInfo(package);
                if (info.Success)
                {
                    activity = info.Data.MainActivity;
                    if (activity.IndexOf('.') == -1)
                    {
                        activity = "." + activity;
                    }
                }
            }
            LogHelper.Log($" AppStart: {package}/{activity}");
            Shell("am", "start", "-a", "android.intent.action.MAIN", "-c", "android.intent.category.LAUNCHER", "-n", $"{package}/{activity}");
            if (wait)
            {
                AppWait(package);
            }
        }
        public AppCurrentInfo AppCurrent()
        {
            AppCurrentInfo info = new AppCurrentInfo();
            var result = Shell("dumpsys", "window", "windows");
            Regex focus = new Regex("mCurrentFocus=Window\\{.*?\\s+(?<package>[^\\s]+)/(?<activity>[^\\s]+)\\}");
            Match match = focus.Match(result);
            if (match.Success)
            {
                info.Package = match.Groups["package"].Value;
                info.Activity = match.Groups["activity"].Value;
                return info;
            }
            result = Shell("dumpsys", "activity", "activities");
            Regex record = new Regex("mResumedActivity: ActivityRecord\\{.*?\\s+(?<package>[^\\s]+)/(?<activity>[^\\s]+)\\s.*?\\}");
            match = record.Match(result);
            if (match.Success)
            {
                info.Package = match.Groups["package"].Value;
                result = Shell("dumpsys", "activity", "top");
                Regex activity = new Regex("ACTIVITY (?<package>[^\\s]+)/(?<activity>[^/\\s]+) \\w+ pid=(?<pid>\\d+)");
                var matchs = activity.Matches(result);
                if (matchs.Count > 0)
                {
                    for (int i = 0; i < matchs.Count; i++)
                    {
                        if (matchs[i].Groups["package"].Value == info.Package)
                        {
                            info.Activity = matchs[i].Groups["activity"].Value;
                            info.Pid = int.TryParse(matchs[i].Groups["pid"].Value, out int pid) ? pid : 0;
                            return info;
                        }
                    }
                }
            }
            return null;
        }
        public bool AppWait(string package, int timeout = 20000, string activity = null, bool front = false)
        {
            LogHelper.SUCCESS($"Đang chờ ứng dụng [{package}]");
            long deadline = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeout;
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < deadline)
            {
                try
                {
                    if (front)
                    {
                        var info = AppCurrent();
                        if (info == null)
                        {
                            continue;
                        }
                        if (info.Package == package)
                        {
                            if (!string.IsNullOrWhiteSpace(activity))
                            {
                                if (activity == info.Activity)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        var list = Shell("pidof", package);
                        if (!string.IsNullOrEmpty(list))
                        {
                            return true;
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                }
            }
            return false;
        }
        public string FindElement(string xmlContent, List<string> xpaths, int timeoutInSeconds)
        {
            try
            {
                int startTick = Environment.TickCount;
                while (true)
                {
                    // Lấy nội dung XML nếu chưa có
                    if (string.IsNullOrEmpty(xmlContent))
                    {
                        xmlContent = GetXMLSource();
                    }
                    if (!string.IsNullOrEmpty(xmlContent) && xpaths != null && xpaths.Count > 0)
                    {
                        xmlContent = xmlContent.ToLower();

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlContent);

                        // Duyệt qua từng xpath trong danh sách
                        foreach (var xpath in xpaths)
                        {
                            try
                            {
                                XmlNodeList nodeList = xmlDoc.SelectNodes(xpath.ToLower());
                                if (nodeList != null && nodeList.Count > 0)
                                {
                                    // Duyệt qua từng node và kiểm tra thuộc tính "bounds"
                                    foreach (XmlNode node in nodeList)
                                    {
                                        if (node.Attributes["bounds"] != null)
                                        {
                                            return xpath; // Trả về xpath đầu tiên thỏa mãn
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // Bỏ qua lỗi nếu xpath không hợp lệ
                            }
                        }
                    }

                    // Kiểm tra nếu đã hết thời gian chờ
                    if (Environment.TickCount - startTick >= timeoutInSeconds * 1000)
                    {
                        break;
                    }

                    // Làm mới nội dung XML nếu cần
                    xmlContent = "";
                }
            }
            catch (Exception ex)
            {
                LogHelper.ERROR(ex.Message);
            }

            return null; // Không tìm thấy phần tử phù hợp
        }
        public void AppStopAll(params string[] excludes)
        {
            List<string> list = new List<string>() {
                "com.cell47.College_Proxy",
                "com.github.uiautomator",
                    "com.android.shell",
                     "com.android.systemui",
                "com.github.uiautomator.test",
            };
            string its = string.Empty;
            list.AddRange(excludes);
            List<string> apps = AppRunningList();
            foreach (string app in apps)
            {
                if (list.Contains(app))
                {
                    continue;
                }
                its = its + app + "\n";
                StopApp(app);
            }
            Connect();

        }
        public Point FindPoint(string element, int timeoutInSeconds = 5, string xmlsoucre = "")
        {
            try
            {
                LogHelper.SUCCESS("FindElement " + element + "");
                string attributeValue = GetBoundsValues(timeoutInSeconds, xmlsoucre, element).FirstOrDefault();
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    return new RectangleArea(attributeValue).GetCenterPoint();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ERROR(ex.Message);
            }

            return new Point();
        }
        /// <summary>
        /// Kiểm tra xem thiết bị Android có kết nối Internet không (bằng cách ping 8.8.8.8).
        /// </summary>
        /// <param name="deviceId">ID của thiết bị ADB (ví dụ: "ce031603b4f5a13703")</param>
        /// <returns>true nếu có mạng, false nếu không</returns>
        public bool IsDeviceConnectedToInternet()
        {
            try
            {

                return maxChange.CheckInternet();
            }
            catch
            {
                return false;
            }
        }
        public string GetIp()
        {
            try
            {

                return maxChange.GetIP();
            }
            catch
            {
                return "";
            }
        }
        public void Delay(int delay)
        {
            stopwatch.Restart();
            while (stopwatch.ElapsedMilliseconds < delay * 1000)
            {
                if (!Running) break;
                LogHelper.Log($"Đang chờ {stopwatch.ElapsedMilliseconds} ms");
            }
            stopwatch.Stop();
        }
        public void Delay(int min, int max)
        {
            int value = random.Next(min, max);
            stopwatch.Restart();
            while (stopwatch.ElapsedMilliseconds < value * 1000)
            {
                if (!Running) break;
                LogHelper.Log($"Đang chờ {stopwatch.ElapsedMilliseconds} ms");
            }
            stopwatch.Stop();
        }
    }
}
