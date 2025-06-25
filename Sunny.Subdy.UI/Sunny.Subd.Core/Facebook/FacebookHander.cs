using System.Net;
using System.Text.RegularExpressions;
using AutoAndroid;
using Newtonsoft.Json.Linq;
using OtpNet;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Utils;

namespace Sunny.Subd.Core.Facebook
{
    public class FacebookHander
    {
        public static List<string> TypeLogin = new List<string>
        {
            "Uid|Password",
            "Email|Password",
        };
        public static string Path()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "App\\Facebook.apk";
        }
        public static string Package(string platform = "facebook")
        {
            switch (platform.ToLower())
            {
                case "facebook":
                    return "com.facebook.katana";
                case "messenger":
                    return "com.facebook.orca";
                case "instagram":
                    return "com.instagram.android";
                default:
                    throw new ArgumentException("Unsupported platform: " + platform);
            }
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
        public static string GetCodeTowFA(string _2FA)
        {
            string string_FA = _2FA.Replace("\r", "").Replace("\n", "").Replace(" ", "")
                             .ToString();
            try
            {
                WebClient webClient = new WebClient();
                string input = webClient.DownloadString("http://2fa.live/tok/" + string_FA);
                return Regex.Match(input, "token\":\"(\\d+)\"").Groups[1].Value;
            }
            catch
            {

            }
            byte[] secretKeyBytes = Base32Encoding.ToBytes(string_FA);

            var totp = new Totp(secretKeyBytes);
            var two_fa = totp.ComputeTotp();
            if (Convert.ToInt64(two_fa) > 5)
            {
                return two_fa;
            }
            return string.Empty;
        }
        public static string GetAuthenticationInfo(ADBClient client)
        {
            string token = "";
            string cookie = "";
            string uid = "";
            string catData = "";
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    catData = client.Shell("su -c cat data/data/" + Package() + "/app_light_prefs/" + Package() + "/authentication");
                    if (!string.IsNullOrEmpty(catData))
                    {
                        try
                        {
                            string rawText = Regex.Replace(catData, "[^\\u0020-\\u007E]", "|");
                            token = "EAA" + Regex.Match(rawText, "EAA(.*?)\\|").Groups[1].Value;

                        }
                        catch
                        {
                        }
                        try
                        {
                            uid = Regex.Match(catData, @"\""name\"":\""c_user\"",\""value\"":\""(?<cuser>\d+)").Groups["cuser"].Value;
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (string.IsNullOrEmpty(uid))
                            {
                                string uidPattern = @"uid\s*\D*(\d{5,})";  // Bỏ qua ký tự lạ trước số UID

                                Match uidMatch = Regex.Match(catData, uidPattern);
                                if (uidMatch.Success)
                                {
                                    uid = uidMatch.Groups[1].Value;
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            //string jsonCookies = Regex.Match(catData, @"session_cookies_string♥�(?<json>\[.*?\])♣").Groups["json"].Value;
                            string jsonCookies = Regex.Match(catData, @"session_cookies_string.*?(\[.*?\])").Groups[1].Value;

                            // Parse JSON
                            JArray cookies = JArray.Parse(jsonCookies);

                            // Duyệt qua cookies và chọn các trường cần thiết
                            string cookieString = "";

                            foreach (var item in cookies)
                            {
                                string name = item["name"]?.ToString();
                                string value = item["value"]?.ToString();

                                // Chỉ giữ các cookie theo yêu cầu
                                if (name == "xs" || name == "fr" || name == "c_user" || name == "datr")
                                {
                                    cookieString += $"{name}={value}; ";
                                }
                            }
                            cookie = cookieString;
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (string.IsNullOrEmpty(cookie))
                            {
                                string xs = string.Empty, fr = string.Empty, datr = string.Empty;
                                // Loại bỏ ký tự đặc biệt
                                string cleanInput = Regex.Replace(catData, @"[\u0000-\u001F]", "").Trim();

                                // Regex lấy UID
                                Match match = Regex.Match(cleanInput, @"session_key\s*=\s*([^\s]+)");
                                if (match.Success)
                                {
                                    xs = match.Groups[1].Value;
                                }
                                match = Regex.Match(cleanInput, @"fr\s*=\s*([^\s]+)");
                                if (match.Success)
                                {
                                    fr = match.Groups[1].Value;
                                }
                                match = Regex.Match(cleanInput, @"datr\s+([^\s]+)");
                                if (match.Success)
                                {
                                    datr = match.Groups[1].Value;
                                }
                                // Kiểm tra dữ liệu cần thiết
                                if (string.IsNullOrEmpty(uid))
                                    continue;

                                // Tạo cookie Facebook chuẩn
                                string fbCookie = $"c_user={uid}; xs={xs};" +
                                                  (!string.IsNullOrEmpty(fr) ? $" fr={fr};" : "") +
                                                  (!string.IsNullOrEmpty(datr) ? $" datr={datr};" : "");

                                cookie = fbCookie;
                            }
                        }
                        catch
                        {


                        }

                        if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(cookie) && !string.IsNullOrEmpty(uid))
                        {
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            return uid + "|" + token + "|" + cookie;
        }


        public static List<string> Regsiner_Facebook()
        {
            var xpaths = XpathManager.Combine
                (
                    XpathType.CP282,
                    XpathType.Loading,
                    XpathType.Captcha,
                    XpathType.CP956,
                    XpathType.Logout,
                    XpathType.ExistEmail,
                    XpathType.Success,
                    XpathType.CashApp,
                    XpathType.Regsiner_Facebook,
                    XpathType.NavigationButton
                );
            return xpaths;
        }
    }
}
