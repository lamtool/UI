using AutoAndroid;
using Sunny.Subd.Core.Facebook;
using Sunny.Subd.Core.Facebook.ScriptActions;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Proxies;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.UI;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sunny.Subd.Core.Services
{
    public class MainService
    {
        // Các trường dữ liệu private
        public readonly ADBClient _client; // Đối tượng điều khiển thiết bị Android qua ADB
        public Account _account; // Tài khoản đang xử lý
        public readonly ConfigModel _config; // Cấu hình của dịch vụ
        public readonly CancellationToken _ct; // Token để hủy tác vụ
        public readonly IFacebookService _facebookService; // Dịch vụ xử lý Facebook
        public readonly AccountContext _accountContext = new(); // Context để quản lý tài khoản
        public readonly string _platform; // Nền tảng đang sử dụng
        public readonly Stopwatch _stopwatch = new(); // Đồng hồ bấm giờ để theo dõi thời gian
        public int Timeout = 0; // Thời gian tối đa cho thao tác
        public readonly JsonHelper _settingGeneral; // Cấu hình chung từ JSON
        public string _sate = string.Empty; // Trạng thái hiện tại của quá trình
        public Stopwatch _swTotal = new Stopwatch();


        // Constructor khởi tạo dịch vụ
        public MainService(string platform, ADBClient device, ConfigModel config, CancellationToken ct)
        {
            _client = device ?? throw new ArgumentNullException(nameof(device));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _platform = platform ?? throw new ArgumentNullException(nameof(platform));
            _ct = ct;
            _facebookService = new FacebookService();
            _settingGeneral = config.SettingGeneral;
        }

        // Phương thức trì hoãn với thông báo trạng thái
        public async Task DelayMessageAsync(int second, string message, int color)
        {
            for (int i = 1; i <= second; i++)
            {
                SetStatus($"[{i}/{second}]... {message}", color);
                await Task.Delay(1000);
            }
        }

        public void SetStatus(string status, int color)
        {
            if (!string.IsNullOrEmpty(_sate))
            {
                status = $"[{_sate}] - ({status})";
            }
            if (_account != null)
            {
                _account.Status = status;
                _account.RecentInteraction = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _account.ColorType = color;
            }
            if (_client?.Device != null)
            {
                _client.Device.Status = status;
                _client.Device.TypeColor = color;
            }
        }

        // Kiểm tra và dừng tác vụ nếu cần
        public async Task Stop()
        {
            if (_ct.IsCancellationRequested)
            {
                throw new OperationCanceledException("Bạn đã dừng tài khoản.");
            }
            if (Timeout != 0 && _stopwatch.IsRunning && _stopwatch.ElapsedMilliseconds > Timeout)
            {
                _stopwatch.Restart();
                SetStatus("Đã quá thời gian thực hiện thao tác, dừng tài khoản.", 1);
                throw new TimeoutException("Đã quá thời gian thực hiện thao tác.");
            }
            await SleepAuto();

        }

        public async Task SleepAuto()
        {
            if (!_settingGeneral.GetBooleanValue("checkBox15", false))
            {
                return;
            }

            DateTime? startDateTime = _settingGeneral.GetValueDateTime("uiTimePicker1");
            DateTime? endDateTime = _settingGeneral.GetValueDateTime("uiTimePicker2");
            if (startDateTime == null && endDateTime == null) return;
            TimeSpan now = DateTime.Now.TimeOfDay;
            TimeSpan startTime = startDateTime.Value.TimeOfDay;
            TimeSpan endTime = endDateTime.Value.TimeOfDay;

            if (startTime <= endTime && now >= startTime && now <= endTime)
            {
                TimeSpan remaining = endTime - now;
                int totalSecondsInt = (int)remaining.TotalSeconds;
                await DelayMessageAsync(totalSecondsInt, "Đã tới giờ nghỉ giải lao, phần mềm sẽ ngủ đông.", 2);
            }


        }


        // Trích xuất và cập nhật thông tin xác thực
        private async Task ExtractAndUpdateAuthenticationInfoAsync()
        {
            if (!_client.IsRoot()) return;

            string value = FacebookHander.GetAuthenticationInfo(_client);
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("Không thể lấy thông tin xác thực.");

            var parts = value.Split('|');
            if (parts.Length < 3)
                throw new Exception("Chuỗi xác thực không hợp lệ.");

            _account.Uid ??= parts[0];
            _account.Cookie ??= parts[2];
            _account.Token ??= parts[1];

            if (_settingGeneral.GetBooleanValue("checkBox3", true))
            {
                string profileDir = _settingGeneral.GetValuesFromInputString("textBox3", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup", "Profiles"));
                Directory.CreateDirectory(profileDir);
                string fileProfile = Path.Combine(profileDir, $"{_account.Uid}.tar.gz");
                new BackupRestoreHelper(_client.Device).BackupFacebook(fileProfile);
            }

            _account.State = "LIVE";
            _accountContext.Update(_account);
        }

        // Thay đổi thông tin thiết bị
        private async Task ChangeInfoAsync()
        {
            if (!_settingGeneral.GetBooleanValue("checkBox1", true)) return;
            _sate = "Thay đổi thiết bị";
            try
            {
                SetStatus("Đang làm", 2);
                string filezip = string.Empty;
                List<string> brands = _settingGeneral.GetValuesFromInputString("textBox1", DeviceServices.Brands).Split('|').ToList();
                bool backup = _settingGeneral.GetBooleanValue("checkBox2", true);
                if (backup)
                {
                    string folder = _settingGeneral.GetValuesFromInputString("textBox2", Path.Combine("Backup", "Devices"));
                    Directory.CreateDirectory(folder);
                    filezip = Path.Combine(folder, $"{_account.Uid}.tar.gz");
                }
                if (await Task.Run(() => _client.ChangInfo(filezip, backup, "", "VN")))
                {
                    SetStatus($"Thành công. [{_client.GetDeviceName()}]", 2);
                }
                else
                {
                    SetStatus($"Thất bại. [{_client.GetDeviceName()}]", 1);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                SetStatus(ex.Message, 1);
            }
        }

        // Thay đổi proxy
        private async Task ChangeProxyAsync()
        {
            _sate = "Thay đổi IP";
            SetStatus("Đang làm", 2);
            _client.Shell("settings put global http_proxy :0");
            _client.StopApp(VATProxyService.Package_Proxy);
            string proxy = string.Empty;
            var proxyType = GetProxyType();
            SetStatus($"Loại: [{proxyType}] - ", 2);
            switch (proxyType)
            {
                case ProxyService.NoIP:
                    return;
                case ProxyService.Mobile4G:
                    await HandleMobile4GProxyAsync();
                    break;
                case ProxyService.KiotProxy:
                    proxy = await GetProxyFromServiceAsync(ProxyKiot.NewProxy, ProxyKiot.GetProxy);
                    break;
                case ProxyService.WWProxy:
                    proxy = await GetProxyFromServiceAsync(ProxyWWW.NewProxy, ProxyWWW.GetProxy);
                    break;
                case ProxyService.ProxyMart:
                    proxy = await GetProxyFromServiceAsync(ProxyMart.NewProxy, ProxyMart.GetProxy);
                    break;
                case ProxyService.CustomProxy:
                    proxy = await GetCustomProxyAsync();
                    break;
                case ProxyService.ProxyAssigned:
                    proxy = _account.Proxy;
                    break;
            }
            SetStatus($"Loại: [{proxyType}] - [{proxy}]", 2);
            if (!string.IsNullOrEmpty(proxy))
            {
                _client.ConnectProxy(proxy);
            }
            int timeDelay = _settingGeneral.GetIntType("numericUpDown3", 10);
            await DelayMessageAsync(timeDelay, "Delay kết nối.", 2);
            if (proxyType == ProxyService.Mobile4G)
            {
                _client.DisablePlane();
                _client.Enabel4G();
                await DelayMessageAsync(5, "Delay kết nối 4G.", 2);
            }
        }

        // Lấy loại proxy từ cấu hình
        private string GetProxyType()
        {
            try
            {
                int index = _settingGeneral.GetIntType("cbb_ListTypeProxy", 0);
                return index >= 0 ? ProxyService.ProxyTypes[index] : ProxyService.NoIP;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                SetStatus(ex.Message, 1);
                return ProxyService.NoIP;
            }
        }

        // Xử lý proxy 4G
        private async Task HandleMobile4GProxyAsync()
        {
            _client.EnabePlane();
            _client.Disable4G();
        }

        // Lấy proxy từ dịch vụ
        private async Task<string> GetProxyFromServiceAsync(Func<string, Task<string>> newProxyFunc, Func<string, Task<string>> getProxyFunc)
        {
            string key = ProxyService.GetProxy();
            return await newProxyFunc(key) ?? await getProxyFunc(key);
        }

        // Lấy proxy tùy chỉnh
        private async Task<string> GetCustomProxyAsync()
        {
            string line = ProxyService.GetProxy();
            if (string.IsNullOrEmpty(line)) return string.Empty;
            var parts = line.Split('|');
            string proxy = parts[0].Trim();
            string link = parts.Length > 1 ? parts[1].Trim() : string.Empty;
            if (!string.IsNullOrEmpty(link)) await RequestService.Get(link);
            return proxy;
        }

        // Kiểm tra kết nối internet
        private async Task<bool> IsInternetAsync()
        {
            _sate = "Kiểm tra tín hiệu internet";
            int attempts = _settingGeneral.GetIntType("nud_IndexFailProxy", 5);
            for (int i = 1; i <= attempts; i++)
            {
                string ip = _client.GetIp();
                if (string.IsNullOrEmpty(ip))
                {
                    SetStatus($"[{i}/{attempts}] Không có internet.", 1);
                    continue;
                }
                SetStatus($"[{i}/{attempts}] IP:[{ip}].", 2);
                if (_account != null)
                {
                    _account.IP = ip;
                    _account.Serial = $"[{_client.Device.NameDevice} - {_client.Device.Serial}]";
                }
                return true;
            }
            return false;
        }

        // Mở ứng dụng Facebook
        private async Task<bool> OpenFacebookAsync()
        {
            _sate = "Mở ứng dụng facebook";
            string fileAPK = string.Empty;
            if (_settingGeneral.GetBooleanValue("checkBox8", true))
            {
                fileAPK = _settingGeneral.GetValuesFromInputString("check RadiatBox8", FacebookHander.FilePath());
            }
            for (int i = 1; i <= 10; i++)
            {
                SetStatus($"[{i}/{10}] Đang làm", 2);
                _client.AppStart(FacebookHander.Package(), true, true, true);
                if (_client.ElementWithAttributes("", 5, click: false))
                {
                    SetStatus($"[{i}/{10}] Bị crash. Cài lại ứng dụng facebook.", 1);
                    if (!File.Exists(fileAPK))
                    {
                        SetStatus($"[{i}/{10}] Bị crash. Cài lại ứng dụng facebook. Không tìm thấy apk [{fileAPK}]", 1);
                        return false;
                    }
                    _client.UninstallApp(FacebookHander.Package());
                    _client.InstallApp(fileAPK);
                    continue;
                }
                if (_client.AppWait(FacebookHander.Package())) return true;
            }
            return _client.AppWait(FacebookHander.Package());
        }

        // Kết nối và chuẩn bị thiết bị
        private async Task<bool> ConnectAndPrepareDeviceAsync(bool changeProxy)
        {
            if (!await ConnectDeviceAsync())
                return false;

            await PrepareDeviceAsync();

            if (changeProxy)
            {
                await ChangeInfoAsync();
                await ChangeProxyAsync();
            }

            if (_settingGeneral.GetBooleanValue("checkBox4", false))
            {
                int retryCount = _settingGeneral.GetIntType("numericUpDown1", 1);
                for (int i = 0; i < retryCount; i++)
                {
                    if (await IsInternetAsync())
                        return true;

                    // Có thể thêm delay giữa các lần thử nếu cần:
                    // await Task.Delay(1000);
                }

                SetStatus($"Reboot khi mất mạng quá {retryCount} lần", 2);
                _client.RebootAndWaitForDeviceReady();
                return false;
            }

            return await IsInternetAsync();
        }

        // Kết nối thiết bị
        private async Task<bool> ConnectDeviceAsync()
        {
            return await Task.Run(() => _client.Connect());
        }

        // Chuẩn bị thiết bị
        private async Task PrepareDeviceAsync()
        {
            if (_settingGeneral.GetBooleanValue("checkBox12", false))
            {
                _client.AppClear(FacebookHander.Package());
                _client.GrantAppPermissions(FacebookHander.Package());
            }
            if (!await OpenFacebookAsync()) throw new Exception("Không thể mở Facebook.");
            _client.SetSize();
        }

        // Kiểm tra trạng thái tài khoản
        private async Task<bool> CheckLiveAsync()
        {
            if (_settingGeneral.GetBooleanValue("checkBox9", true)) return true;
            _sate = "Kiểm tra tài khoản";
            var status = await FacebookRequest.CheckLive(_account.Uid);
            if (status)
            {
                _account.State = "LIVE";
                SetStatus("Tài khoản facebook: LIVE", 2);
            }
            else
            {
                _account.State = "DIE";
                SetStatus("Tài khoản facebook: DIE", 1);
            }
            return status;
        }

        // Khôi phục dữ liệu Facebook
        private async Task<bool> RestoreFacebookAsync()
        {
            _client.StopApp(FacebookHander.Package());
            if (!_settingGeneral.GetBooleanValue("checkBox3", true) || !_client.IsRoot()) return await OpenFacebookAsync();
            string filezip = string.Empty;
            string folder = _settingGeneral.GetValuesFromInputString("textBox2", Path.Combine("Backup", "Profiles"));
            Directory.CreateDirectory(folder);
            filezip = Path.Combine(folder, $"{_account.Uid}.tar.gz");
            if (!File.Exists(filezip)) return await OpenFacebookAsync();
            _sate = "Restore facebook";
            try
            {
                SetStatus("Đang làm", 2);
                new BackupRestoreHelper(_client.Device).RestoreFacebook(filezip);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                SetStatus(ex.Message, 1);
            }
            return await OpenFacebookAsync();
        }

        // Xử lý các trường hợp ngoại lệ
        private void HanderCase(Exception ex)
        {
            SubdyExtension subdyExtension = ex as SubdyExtension ?? new SubdyExtension(SubdyEnum.Error, ex.Message);
            switch (subdyExtension.SubdyEnum)
            {
                case SubdyEnum.Stop:
                    _account.Status = "Đã dừng lại.";
                    break;
                case SubdyEnum.Error:
                    _account.Status = "Lỗi: " + subdyExtension.Message;
                    break;
                case SubdyEnum.CP_282:
                    _account.Status = "Lỗi CP_282: " + subdyExtension.Message;
                    _account.State = "CP_282";
                    break;
                case SubdyEnum.CP_956:
                    _account.Status = "Lỗi CP_956: " + subdyExtension.Message;
                    _account.State = "CP_956";
                    break;
                case SubdyEnum.LogOut:
                    _account.Status = "Đăng xuất: " + subdyExtension.Message;
                    _account.State = "Logout";
                    break;
                case SubdyEnum.Captcha:
                    _account.Status = "Captcha: " + subdyExtension.Message;
                    _account.State = "Captcha";
                    break;
                case SubdyEnum.Block:
                    _account.Status = "Tài khoản bị chặn: " + subdyExtension.Message;
                    _account.State = "Block";
                    break;
            }
            new AccountContext().Update(_account);
        }

        private bool IsReboot()
        {
            bool isReboot = _settingGeneral.GetBooleanValue("checkBox5", false);
            if (!isReboot) return false;
            double timeout = _settingGeneral.GetIntType("numericUpDown2", 30) * 60000;
            if (timeout != 0 && _swTotal.IsRunning && _swTotal.ElapsedMilliseconds > timeout)
            {
                SetStatus($"Tự reboot sau {_settingGeneral.GetIntType("numericUpDown2", 30)} phút.", 2);
                _client.RebootAndWaitForDeviceReady();
                _stopwatch.Restart();
                return isReboot;
            }
            return false;
        }

        // Chạy dịch vụ chính
        public async Task RunAsync()
        {
            _stopwatch.Start();
            _swTotal.Start();
            while (!_ct.IsCancellationRequested)
            {
                if (!AccountServices.Accounts.Any())
                {
                    _client.LogHelper.SUCCESS("Đã hoàn thành!");
                    break;
                }

                if (IsReboot()) continue;

                if (!await ConnectAndPrepareDeviceAsync(false)) continue;

                _account = AccountServices.GetAccount();

                if (_account == null) continue;

                try
                {
                    if (!await ConnectAndPrepareDeviceAsync(true)) continue;

                    if (!await CheckLiveAsync()) continue;

                    if (!await RestoreFacebookAsync()) continue;

                    int index = _settingGeneral.GetIntType("comboBox1", 0);

                    _account.Uid_Email = index == 1 ? _account.Email : _account.Uid;
                    _account.Uid_Email ??= _account.Email ?? _account.Uid;

                    await _facebookService.Login(_client, _account, _ct);

                    if (_settingGeneral.GetBooleanValue("", true))
                    {
                        int second = SubdyHelper.RandomValue(_settingGeneral.GetIntType("numericUpDown25", 10), _settingGeneral.GetIntType("numericUpDown24", 20));

                        await DelayMessageAsync(second, "Delay sau khi đăng nhập trước khi thực hiện thao tác.", 2);
                    }

                    await ExtractAndUpdateAuthenticationInfoAsync();

                    await DoScriptAsync();
                }
                catch (Exception ex)
                {
                    HanderCase(ex);
                }
                finally
                {

                }
            }
            _stopwatch.Stop();
        }

        // Thực thi kịch bản
        private async Task DoScriptAsync()
        {
            if (string.IsNullOrEmpty(_config.Script.Config))
                throw new SubdyExtension(SubdyEnum.None, "Không có tương tác nào để thực hiện.");

            var actionIds = SubdyHelper.Shuffle(_config.Script.Config.Split('|').Where(id => !string.IsNullOrWhiteSpace(id)).ToList());
            if (!actionIds.Any())
                throw new SubdyExtension(SubdyEnum.None, "Không có tương tác nào để thực hiện.");

            var scriptContext = new ScriptActionContext();
            foreach (var actionId in actionIds)
            {
                await ExecuteActionAsync(actionId, scriptContext);
            }
            await ScrollNewsFeedAsync();
        }

        // Thực thi hành động cụ thể trong kịch bản
        private async Task ExecuteActionAsync(string actionId, ScriptActionContext context)
        {
            ScriptAction action = context.GetById(Guid.Parse(actionId));
            if (action == null) return;
            switch (action.Type)
            {
                case Sunny.Subdy.Common.Models.TypeAction.FB_SpamXu:
                    await new FbSpamXuHandler(_platform, _client, _config, _ct).ExecuteAsync(action, context);
                    break;
                default:
                    throw new NotImplementedException($"Hành động {actionId} chưa được triển khai.");
            }
        }

        // Cuộn news feed
        private async Task ScrollNewsFeedAsync()
        {
            string doneScript = GetDoneScriptFlag();
            if (string.IsNullOrEmpty(doneScript)) return;
            int maxScrolls = 10;
            for (int i = 0; i < maxScrolls; i++)
            {
                await Stop();
            }
        }

        // Lấy cờ hoàn thành kịch bản
        private string GetDoneScriptFlag()
        {
            var scriptHelper = new JsonHelper(_config.Script.JsonData, true);
            if (scriptHelper.GetBooleanValue("check_Interaction_2"))
                return "Swipe";
            if (scriptHelper.GetBooleanValue("check_Interaction_3"))
                return "LIKE";
            return string.Empty;
        }
    }
}