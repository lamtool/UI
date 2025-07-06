using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Data.Models;
using System.Diagnostics;

namespace Sunny.Subd.Core.Facebook.ScriptActions
{
    internal class FbChangeAvatarHandler : IActionHandler
    {
        private List<string> xpaths = new List<string>
        {
                "//*[contains(@content-desc, 'Photo')]",
                "//*[@content-desc=\"Gallery\"]",
                "//*[contains(@content-desc, 'Profile picture')]"
        };
        private Stopwatch stopwatch = Stopwatch.StartNew();
        public string TypeAction => Sunny.Subdy.Common.Models.TypeAction.FB_ChangeAvatar;

        public async Task<SubdyExtension> ExecuteAsync(Account account, ADBClient device)
        {
            xpaths.AddRange(XpathManager.Combine(XpathType.CP282, XpathType.Captcha, XpathType.NavigationButton));
            device.StopApp(FacebookHander.Package());
            device.Shell("am start -n com.facebook.katana/.IntentUriHandler \"fb://profile_edit\"");
            stopwatch.Restart();
            while (stopwatch.ElapsedMilliseconds < 300000)
            {
                string _case = device.FindElement("", xpaths, 30);
                if (string.IsNullOrEmpty(_case))
                {
                    device.StopApp(FacebookHander.Package());
                    device.Shell("am start -a android.intent.action.VIEW -d \"fb://profile_edit\"");
                    device.Delay(5);
                    continue;
                }
                switch (_case)
                {
                    case "//*[@content-desc=\"Gallery\"]":
                        {
                            device.ElementWithAttributes("//*[@content-desc=\"Gallery\"]", 5);
                            device.ElementWithAttributes("//*[@text=\"LT\"]", 15);
                            device.Delay(2);
                            break;
                        }
                    case "//*[contains(@content-desc, 'Profile picture')]":
                    case var c when XpathManager.Get(XpathType.NavigationButton).Contains(c):
                        device.ElementWithAttributes(_case);
                        break;
                    case var c when XpathManager.Get(XpathType.CP282).Contains(c):
                    case var x when XpathManager.Get(XpathType.Captcha).Contains(x):
                        {
                            return new SubdyExtension(SubdyEnum.CP_282, $"Tài khoản bị. [{_case}]");
                        }
                    case "//*[contains(@content-desc, 'Photo')]":
                        {
                            device.ElementWithAttributes(_case, 5);
                            device.ElementWithAttributes("//*[@content-desc=\"SAVE\"]", 15);
                            device.Delay(10);
                            return new SubdyExtension(SubdyEnum.Success, "Thay avatar thành công.");
                        }
                }
            }
            return new SubdyExtension(SubdyEnum.Stop, "Đã xảy ra lỗi khi thay ảnh đại diện.");
        }
    }
}
