using System.Diagnostics;
using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook
{
    public class FacebookService : IFacebookService
    {
        private Stopwatch Stopwatch = new Stopwatch();
        private CancellationToken _ct;
        private ADBClient _device;
        private Account _account;
        private void CheckStop(int second)
        {
            if (Stopwatch.ElapsedMilliseconds > second * 1000)
            {
                throw new SubdyExtension(SubdyEnum.Stop, "Đã quá thời gian thực hiện thao tác.");
            }
            if (_ct.IsCancellationRequested)
            {
                throw new SubdyExtension(SubdyEnum.Stop, "Bạn đã dừng thực hiện việc thao tác.");
            }
        }
        private void SetStatus(string status)
        {
            if (_account == null || _device == null)
            {
                return;
            }
            _account.Status = status;
            _device.Device.Status = status;
        }
        public async Task<SubdyExtension> Login(ADBClient client, Account account, string json, CancellationToken ct)
        {
            _device = client ?? throw new ArgumentNullException(nameof(client), "ADBClient cannot be null");
            _account = account ?? throw new ArgumentNullException(nameof(account), "Account cannot be null");
            JsonHelper jsonHelper = new JsonHelper(json);
            _ct = ct;
            SubdyEnum subyEnum = SubdyEnum.None;
            string message = "Đã xảy ra lỗi đang nhặp tài khoản!";
            Stopwatch.Restart();
            string _case = string.Empty;
            while (true)
            {
                CheckStop(180);
                SetStatus($"Đang đăng nhập.");
                _case = client.FindElement("", FacebookHander.GetActiAccount(), 120);
                if (string.IsNullOrEmpty(_case))
                {
                    client.AppStart(FacebookHander.Package(), true, true, true);
                    continue;
                }
                SetStatus($"Xử lý case [{_case}]...");
                switch (_case)
                {
                    case var c when XpathManager.Get(XpathType.Loading).Contains(c): continue;
                    case var c when XpathManager.Get(XpathType.CP282).Contains(c):
                        subyEnum = SubdyEnum.CP_282;
                        message = $"Tài khoản bị 282. [{c}]";
                        throw new SubdyExtension(subyEnum, message);
                    case var c when XpathManager.Get(XpathType.CP956).Contains(c):
                        subyEnum = SubdyEnum.CP_956;
                        message = $"Tài khoản bị 956. [{c}]";
                        throw new SubdyExtension(subyEnum, message);
                    case var c when XpathManager.Get(XpathType.Captcha).Contains(c):
                        subyEnum = SubdyEnum.Captcha;
                        message = $"Tài khoản dính captcha. [{c}]";
                        return new SubdyExtension(subyEnum, message);
                    case var c when XpathManager.Get(XpathType.Block).Contains(c):
                        subyEnum = SubdyEnum.Block;
                        message = $"Tài khoản bị block. [{c}]";
                        return new SubdyExtension(subyEnum, message);
                    case var c when XpathManager.Get(XpathType.Logout).Contains(c):
                        subyEnum = SubdyEnum.LogOut;
                        message = $"Tài khoản bị đăng xuất. [{c}]";
                        return new SubdyExtension(subyEnum, message);
                    case var c when XpathManager.Get(XpathType.Success).Contains(c):
                        subyEnum = SubdyEnum.Success;
                        message = $"Tài khoản đăng nhập thành công. [{c}]";
                        return new SubdyExtension(subyEnum, message);
                    case var c when XpathManager.Get(XpathType.InputUserName).Contains(c):
                        await ImportUid();
                        break;
                    case var c when XpathManager.Get(XpathType.InputPassword).Contains(c):
                        await ImportPassword();
                        break;
                    case var c when XpathManager.Get(XpathType.TowFA).Contains(c):
                        await Import2FA();
                        break;
                    case var c when XpathManager.Get(XpathType.NavigationButton).Contains(c):
                        client.ElementWithAttributes(c, 1);
                        break;
                }

            }



            throw new SubdyExtension(subyEnum, message);
        }
        private async Task ImportUid()
        {

            string uid = _account.Uid;
            var elements = _device.FindElements(10, "", "//*[@class='android.widget.EditText']");
            if (!elements.Any() || elements.Count != 2) return;
            SetStatus($"Đang nhập {uid}...");
            _device.SendTextSlow("//*[@class='android.widget.EditText']", uid, xml: elements[0].OuterXml);
            SetStatus($"Đang nhập {_account.Password}...");
            _device.SendTextSlow("//*[@class='android.widget.EditText']", _account.Password, xml: elements[1].OuterXml);
            _device.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton));
            return;
        }
        private async Task ImportPassword()
        {
            SetStatus($"Đang nhập {_account.Password}...");
            _device.SendTextSlow("//*[@class='android.widget.EditText']", _account.Password);
            _device.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton));
            return;
        }
        private async Task Import2FA()
        {
            if (string.IsNullOrEmpty(_account.TowFA))
            {
                SetStatus("Không có mã 2FA để nhập.");
                throw new SubdyExtension(SubdyEnum.LogOut, "Tài khoản không có 2fa...");
            }
            SetStatus($"Đang nhập 2FA {_account.TowFA}...");

            string element = _device.FindElement("", new List<string> { "//*[@content-desc='Try another way']", "//*[@text=\"OK\"]", "//*[@class='android.widget.EditText']" }, 10);
            if (element == "//*[@content-desc='Try another way']")
            {
                _device.ElementWithAttributes("//*[@content-desc='Authentication app, Get a code from your authentication app.']", 10);
                _device.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton));
            }
            else if (element == "//*[@text=\"OK\"]")
            {
                _device.ElementWithAttributes("//*[@text=\"OK\"]", 10);
            }
            string code = FacebookHander.GetCodeTowFA(_account.TowFA);
            _device.SendTextSlow("//*[@class='android.widget.EditText']", code);
            _device.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton));
            return;
        }

    }
}
