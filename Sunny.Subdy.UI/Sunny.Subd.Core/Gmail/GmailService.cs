using System.Diagnostics;
using System.Text.RegularExpressions;
using AutoAndroid;

namespace Sunny.Subd.Core.Gmail
{
    public class GmailService
    {
        public static List<string> Gmails = new List<string>();
        private ADBClient _client;
        public static string PackageGmail = "com.google.android.gm";
        public GmailService(ADBClient client)
        {
            _client = client;
        }
        public List<string> GetAccount()
        {
            try
            {
                string input = string.Empty;
                for (int i = 0; i < 10; i++)
                {
                    input = _client.Shell("dumpsys account");
                    if (!string.IsNullOrEmpty(input)) break;
                }

                string pattern = @"Account\s+\{name=([^\s,]+),\s*type=com\.google\}";

                MatchCollection matches = Regex.Matches(input, pattern);
                List<string> emails = new List<string>();

                foreach (Match match in matches)
                {
                    emails.Add(match.Groups[1].Value);
                }
                return emails;
            }
            catch
            {

            }
            return new List<string>();
        }
        public bool RemoveAccount()
        {

            var value = _client.Shell("dumpsys account");
            if (!value.Contains(", type=com.google}"))
            {
                return false;
            }
            bool check = false;
            _client.Shell("am start -a android.settings.SYNC_SETTINGS");
            _client.Delay(3);
            _client.Shell("am start -a android.settings.SYNC_SETTINGS");
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < 60000)
            {
                if (!_client.ElementWithAttributes("//*[@text=\"Google\"]", 10, click: false))
                {
                    break;
                }
                if (_client.ElementWithAttributes("//*[@text=\"Google\"]", 10))
                {
                    _client.ElementWithAttributes(new List<string> { "//*[@text=\"Xóa tài khoản\"]", "//*[@text=\"Remove account\"]" }, 10);
                    _client.ElementWithAttributes("//*[@resource-id=\"android:id/button1\"]", 10);
                }
                if (!GetAccount().Any())
                {
                    break;
                }
            }
            return true;
        }
        public string GetCode()
        {
            _client.AppStart(PackageGmail, true, true, true);

            Stopwatch stopwatch = Stopwatch.StartNew();
            _client.Delay(2);

            string[] patterns =
            {
        @"your security code is:\s*(\d+)",
        @"facebook,\s*(\d+)\s+is",
        @"\s*(\d+)\s+is",
        @"(\d+)\s+là mã bảo mật của bạn",
        @"mã bảo mật của bạn là:\s*(\d+)",
         @"\b(\d{5})\b.*mã xác nhận"
    };

            while (stopwatch.ElapsedMilliseconds < 60000)
            {
                try
                {
                    _client.Swipe(500, 300, 500, 1800, 20, 3);
                    _client.Delay(5);

                    var nodes = _client.FindElements(5, "", "//*[contains(@text, 'Facebook')]");
                    if (!nodes.Any())
                    {
                        nodes = _client.FindElements(5, "", "//*[contains(@content-desc, 'Facebook')]");
                    }
                    if (!nodes.Any())
                    {
                        continue;
                    }


                    foreach (var item in nodes)
                    {
                        string value = item.OuterXml;
                        if (string.IsNullOrEmpty(value)) continue;

                        foreach (var pattern in patterns)
                        {
                            Match match = Regex.Match(value, pattern);
                            if (match.Success)
                            {
                                return match.Groups[1].Value;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetCode: {ex.Message}");
                }
            }

            return string.Empty;
        }
        public bool Login(string email, string pass)
        {
            bool check = false;
            var emails = GetAccount();
            var list = new List<string>
{
    "//*[@text=\"Accept\"]",
    "//*[@text=\"Later\"]",
    "//*[@text=\"Turn on backup\"]",
    "//*[@text=\"I agree\"]",
    "//*[@text=\"Add phone number?\"]",
    "//*[@resource-id=\"password\"]",
    "//*[@text=\"Create account\"]",
    "//*[@text=\"Welcome\"]",
    "//*[@text=\"Google\"]",
    "//*[@text=\"Add account\"]",
};
            list.Add($"//*[@text=\"{email}\"]");
            for (int i = 0; i < 5; i++)
            {
                _client.Delay(3);
                _client.Shell("am start -a android.settings.SYNC_SETTINGS");
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < 180000)
                {
                    _client.LogHelper.Sate = $"Đang đăng nhập tài khoản google [{i + 1}]";
                    string _case = _client.FindElement("", list, 30);
                    if (string.IsNullOrEmpty(_case))
                    {
                        break;
                    }
                    if (_case == $"//*[@text=\"{email}\"]")
                    {
                        break;
                    }
                    switch (_case)
                    {
                        case "//*[@text=\"This account already exists on your device\"]":
                            {
                                break;
                            }
                        case "//*[@text=\"Accept\"]":
                        case "//*[@text=\"Later\"]":
                        case "//*[@text=\"Turn on backup\"]":
                        case "//*[@text=\"I agree\"]":
                            {
                                _client.ElementWithAttributes(_case);
                                _client.Delay(5);
                                break;
                            }
                        case "//*[@text=\"Add account\"]":
                        case "//*[@text=\"Google\"]":
                            {
                                _client.ElementWithAttributes(_case);
                                _client.Delay(5);
                                break;
                            }
                        case "//*[@text=\"Create account\"]":
                            {
                                _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", email);
                                _client.ElementWithAttributes(new List<string> { "//*[@text=\"Next\"]" });
                                _client.Delay(5);
                                break;
                            }
                        case "//*[@resource-id=\"password\"]":
                            {
                                _client.SendTextSlow("//*[@resource-id=\"password\"]", pass, timeout: 10);
                                _client.ElementWithAttributes(new List<string> { "//*[@text=\"Next\"]" }, click: true);
                                _client.Delay(5);
                                break;
                            }
                        case "//*[@text=\"Welcome\"]":
                            {
                                _client.Swipe(478, 1399, 531, 784, 10, 5);
                                _client.ElementWithAttributes(new List<string> { "//*[@text=\"I UNDERSTAND\"]" });


                                _client.Delay(5);
                                break;
                            }
                        case "//*[@text=\"Add phone number?\"]":
                            {
                                _client.Swipe(529, 1464, 606, 787, 100, 5);
                                _client.ElementWithAttributes(new List<string> { "//*[@text=\"I agree\"]" }, click: true);
                                _client.Delay(5);
                                break;
                            }
                    }
                    emails = GetAccount();
                    if (emails.Any())
                    {
                        break;
                    }
                }
                emails = GetAccount();
                if (emails.Any() && emails.Contains(email.ToLower()))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                _client.AppStart("com.google.android.gm", true, true, true);
                _client.Delay(5);
                return true;

            }

            return false;
        }
    }
}
