using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Utils;

namespace Sunny.Subd.Core.Facebook
{
    public class FacebookHander
    {
        public static string Path()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "App\\Facebook.apk";
        }
        public static string Package()
        {
            return "com.facebook.katana";
        }
        public static List<string> GetActiAccount()
        {
            var xpaths = XpathManager.Combine
                (
                    XpathType.CP282,
                    XpathType.Loading,
                    XpathType.Captcha,
                    XpathType.CP956,
                    XpathType.Logout,
                    XpathType.Block,
                    XpathType.Success,
                    XpathType.CashApp,
                    XpathType.TowFA,
                    XpathType.InputUserName,
                    XpathType.InputPassword,
                    XpathType.NavigationButton
                );
            return xpaths;
        }
        public static List<string> GetTowFA()
        {
            var towFA = new List<string>
         {
             "//*[@content-desc='Try another way']",
             "//*[@content-desc='Authentication app, Get a code from your authentication app.']",
             "//*[@text=\"OK\"]",
             "//*[@class='android.widget.EditText']",

         };
            var xpaths = XpathManager.Combine
                (
                    towFA,
                    XpathType.Loading,
                    XpathType.Captcha,
                    XpathType.CP282,
                    XpathType.CP956,
                    XpathType.Logout,
                    XpathType.Block,
                    XpathType.Success,
                    XpathType.CashApp,
                    XpathType.InputUserName,
                    XpathType.InputPassword,
                    XpathType.NavigationButton
                );
            return xpaths;
        }

    }
}
