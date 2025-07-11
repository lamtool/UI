using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sunny.Subdy.Common.API.Model;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Logs;

namespace Sunny.Subdy.Common.API
{
    public class LamToolClient
    {
        private const string Token = "sk_db7f51b03102275fcf5669ab70e2875dd624d686e3b96575d42058f550cd04b9";
        public static User Authentication(string username, string password)
        {
            try
            {
                string url = "https://lamtool.net/api/auth/login";
                var body = new Dictionary<string, string>
                {
                    ["username"] = username,
                    ["password"] = password
                };
                string resurl = HttpRequestHelper.POST(url, body: body);
                if (string.IsNullOrEmpty(resurl))
                {
                    throw new Exception("Đã xảy ra lỗi server. Vui lòng thử lại hoặc liên hệ admin.");
                }
                JObject jObject = JObject.Parse(resurl);
                if (!Convert.ToBoolean(jObject["success"])) throw new Exception(jObject["error"].ToString());
                User user = new User();
                user.UserName = username;
                user.Password = password;
                user.Id = jObject["data"]["user"]["_id"].ToString();
                user.Token = jObject["data"]["token"].ToString();
                user.Balance = double.Parse(jObject["data"]["user"]["balance"].ToString());
                user.Email = jObject["data"]["user"]["email"].ToString();
                return user;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static string AddBalance(string username, double amount, string description)
        {
            try
            {
                string url = "https://lamtool.net/api/balance/add";
                var header = new Dictionary<string, string>
                {
                    ["X-API-Key"] = Token,
                    ["Content-Type"] = "application/json"
                };
                var body = new Dictionary<string, string>
                {
                    ["username"] = username,
                    ["amount"] = amount.ToString(),
                    ["description"] = description,
                };
                string resurl = HttpRequestHelper.POST(url, body: body);
                if (string.IsNullOrEmpty(resurl))
                {
                    throw new Exception("Đã xảy ra lỗi server. Vui lòng thử lại hoặc liên hệ admin.");
                }
                JObject jObject = JObject.Parse(resurl);
                if (!Convert.ToBoolean(jObject["success"])) return jObject["error"].ToString();

                return jObject["message"].ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public static string EscapeJsonString(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
        }
        public static string SubtractBalance(string username, double amount, string description)
        {
            try
            {
                string url = "https://lamtool.net/api/balance/subtract";
                var header = new Dictionary<string, string>
                {
                    ["X-API-Key"] = Token,
                    ["Content-Type"] = "application/json"
                };

                string json = $"{{\"username\":\"{EscapeJsonString(username)}\",\"amount\":{amount},\"description\":\"{EscapeJsonString(description)}\"}}";

                string resurl = HttpRequestHelper.POST_JSON(url, headers: header, jsonBody: json);
                if (string.IsNullOrEmpty(resurl))
                {
                    throw new Exception("Đã xảy ra lỗi server. Vui lòng thử lại hoặc liên hệ admin.");
                }

                JObject jObject = JObject.Parse(resurl);
                if (!resurl.Contains("success")) return "error: " + jObject["error"]?.ToString();
                if (!Convert.ToBoolean(jObject["success"])) return "error: " + jObject["error"]?.ToString();

                return jObject["message"].ToString();
            }
            catch (Exception ex)
            {
                return "error: " + ex.Message;
            }
        }
        public static (bool success, string newVersion, string urlUpdate) GetApiResponseAsync(string key, string nameApp, string version)
        {
            try
            {
                string url = $"https://lamtool.net/api/license/check?tool_slug={nameApp}&device_code={key}";
                string json =  HttpRequestHelper.GET(url);

                var obj = JObject.Parse(json);

                bool success = obj["success"]?.Value<bool>() ?? false;
                string newVersion = obj["license"]?["tool"]?["version"]?.ToString() ?? "";
                string updateUrl = obj["license"]?["tool"]?["updateUrl"]?.ToString() ?? "";

                return (success, newVersion, updateUrl);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return (false, string.Empty, string.Empty);
            }
        }
        public static bool IsNewerVersion(string oldVersion, string newVersion)
        {
            string[] currentVersionParts = oldVersion.Split('.');
            string[] newVersionParts = newVersion.Split('.');

            Array.Reverse(currentVersionParts);
            Array.Reverse(newVersionParts);

            int len = Math.Max(currentVersionParts.Length, newVersionParts.Length);

            for (int i = 0; i < len; i++)
            {
                int currentPart = i < currentVersionParts.Length ? int.Parse(currentVersionParts[i]) : 0;
                int newPart = i < newVersionParts.Length ? int.Parse(newVersionParts[i]) : 0;

                if (currentPart < newPart)
                    return true;
                if (currentPart > newPart)
                    return false;
            }

            return false;
        }
    }
}
