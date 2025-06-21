using System.Diagnostics;
using AutoAndroid;
using OpenCvSharp.Text;
using Sunny.Subd.Core.Facebook;
using Sunny.Subd.Core.Facebook.ScriptActions;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Services
{
    public class MainService
    {
        private readonly ADBClient _device;
        private readonly Account _account;
        private readonly ConfigModel _config;
        private readonly CancellationToken _ct;
        private readonly IFacebookService _facebookService;
        private readonly AccountContext _accountContext = new();
        private readonly string _platform;
        private readonly Stopwatch _stopwatch = new();
        private JsonHelper _jsonHelper;
        private int Timeout = 0;

        private readonly ActionExecutor _executor = new ActionExecutor(new List<IActionHandler>
{
    new FbSpamXuHandler(),
});

        public MainService(string platform, ADBClient device, Account account, ConfigModel config, CancellationToken ct)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _account = account ?? throw new ArgumentNullException(nameof(account));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _platform = platform ?? throw new ArgumentNullException(nameof(platform));
            _ct = ct;
            _facebookService = new FacebookService();
            _jsonHelper = new JsonHelper(_config.JsonSetting);
            if (_jsonHelper.GetBooleanValue("ckbTimeoutAccount"))
            {
                Timeout = SubdyHelper.RandomValue(_jsonHelper.GetIntType("numericUpDown2", 40), _jsonHelper.GetIntType("numericUpDown3", 60)) * 60000;
            }
        }

        private void SetStatus(string status)
        {
            if (_account != null) _account.Status = status;
            if (_device?.Device != null) _device.Device.Status = status;
        }

        private async Task Stop()
        {
            if (_ct.IsCancellationRequested)
            {
                throw new SubdyExtension(SubdyEnum.Stop, "Bạn đã dừng tài khoản.");
            }
            if (Timeout != 0)
            {
                if (_stopwatch.ElapsedMilliseconds > Timeout)
                {
                    _stopwatch.Restart();
                    SetStatus("Đã quá thời gian thực hiện thao tác, dừng tài khoản.");
                    throw new SubdyExtension(SubdyEnum.Stop, "Đã quá thời gian thực hiện thao tác, dừng tài khoản.");
                }
            }
        }



        private void ExtractAndUpdateAuthenticationInfo()
        {
            string value = FacebookHander.GetAuthenticationInfo(_device);
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("Không thể lấy thông tin xác thực.");

            var parts = value.Split('|');
            if (parts.Length < 3)
                throw new Exception("Chuỗi xác thực không hợp lệ.");

            _account.Uid ??= parts[0];
            _account.Cookie ??= parts[2];
            _account.Token ??= parts[1];

            if (_jsonHelper.GetBooleanValue("checkBox3", true))
            {
                string profileDir = _jsonHelper.GetValuesFromInputString("textBox3", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup", "Profile"));
                Directory.CreateDirectory(profileDir);
                string fileProfile = Path.Combine(profileDir, $"{_account.Uid}.tar.gz");
                new BackupRestoreHelper(_device.Device).BackupFacebook(fileProfile);
            }

            _account.State = "LIVE";
            _accountContext.Update(_account);
        }

        public async Task RunAsync()
        {

            _device.AppClear(FacebookHander.Package());

            string profileDir = _jsonHelper.GetValuesFromInputString("textBox3", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup", "Profile"));
            Directory.CreateDirectory(profileDir);
            string fileProfile = Path.Combine(profileDir, $"{_account.Uid}.tar.gz");

            if (File.Exists(fileProfile))
            {
                SetStatus("Đang restore tài khoản.");
                new BackupRestoreHelper(_device.Device).RestoreFacebook(fileProfile);
            }
            else
            {
                _device.AppStart(FacebookHander.Package(_platform), true, true, true);
            }

            var loginResult = await _facebookService.Login(_device, _account, _jsonHelper.GetValuesFromInputString("textBox1"), _ct);
            if (loginResult.SubdyEnum != SubdyEnum.Success)
                throw loginResult;

            SetStatus("Đăng nhập thành công.");
            ExtractAndUpdateAuthenticationInfo();

            await DoScript();
        }

        private async Task DoScript()
        {

            if (string.IsNullOrEmpty(_config.Script.Config))
                throw new SubdyExtension(SubdyEnum.None, "Không có tương tác nào để thực hiện.");

            var actionIds = SubdyHelper.Shuffle(_config.Script.Config.Split('|').Where(id => !string.IsNullOrWhiteSpace(id)).ToList());
            if (!actionIds.Any())
                throw new SubdyExtension(SubdyEnum.None, "Không có tương tác nào để thực hiện.");

            var scriptContext = new ScriptActionContext();




            foreach (var id in actionIds)
            {
                if (!Guid.TryParse(id, out var scriptGuid))
                    continue;

                var action = scriptContext.GetById(scriptGuid);
                if (action == null)
                    continue;

                SetStatus($"Đang thực hiện kịch bản [{_config.Script.Name}] -> [{action.Name}]");

                await Stop();

                await _executor.ExecuteAsync(action.Type, action.Json, _account, _device);
            }

            await ScrollNewsFeedAsync();

        }

        private string GetDoneScriptFlag()
        {
            var scriptHelper = new JsonHelper(_config.Script.JsonData);
            if (scriptHelper.GetBooleanValue("check_Interaction_2"))
                return "Swipe";
            if (scriptHelper.GetBooleanValue("check_Interaction_3"))
                return "LIKE";
            return string.Empty;
        }


        private async Task ScrollNewsFeedAsync()
        {
            string doneScript = GetDoneScriptFlag();
            if (string.IsNullOrEmpty(doneScript))
            {
                return;
            }
            while (true)
            {
                await Stop();

            }
        }
    }
}
