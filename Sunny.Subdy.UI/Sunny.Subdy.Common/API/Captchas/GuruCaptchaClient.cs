using Newtonsoft.Json.Linq;
using RestSharp;

namespace Sunny.Subdy.Common.API.Captchas
{
    public class GuruCaptchaClient
    {
        public static async Task<string> GetIdCaptchaV2(string key, string sitekey, string siteurl)
        {
            var client = new RestClient("http://api2.cap.guru");
            var request = new RestRequest("in.php", Method.Get);
            request.AddParameter("key", key);
            request.AddParameter("method", "userrecaptcha");
            request.AddParameter("googlekey", sitekey);
            request.AddParameter("pageurl", siteurl);
            request.AddParameter("json", 1);

            var response = await client.ExecuteAsync(request);
            try
            {
                var result = response.Content;
                var data = JObject.Parse(result);
                if (data != null && (int)data["status"] == 1)
                {
                    return data["request"]!.ToString();
                }
                else
                {
                    return $"ERROR: {data?.ToString() ?? "Unknown error"}";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        public static async Task<string> GetTokenCaptchaV2(string key, string id)
        {
            var client = new RestClient("http://api2.cap.guru");
            var request = new RestRequest("res.php", Method.Post);
            request.AddHeader("Content-Type", "application/json");

            var body = new Dictionary<string, object>
    {
        { "key", key },
        { "action", "get" },
        { "id", id },
        { "json", 1 }
    };

            request.AddJsonBody(body); // OK với AOT vì Dictionary không cần reflection

            var response = await client.ExecuteAsync(request);
            try
            {
                var result = response.Content;
                var data = JObject.Parse(result);
                if (data != null && (int)data["status"] == 1 && !string.IsNullOrEmpty(data["request"]?.ToString()))
                {
                    return data["request"]!.ToString();
                }

                return $"ERROR: {result}";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }
    }
}
