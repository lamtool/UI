using AutoAndroid;
using Newtonsoft.Json.Linq;
using RestSharp;
using Sunny.Subdy.Common.API.Captchas;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Logs;
using Sunny.UI;
using System.Net;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Sunny.Subdy.Common.API.Jobs.TuongTacCheo
{
    public class TuongTacCheoClient
    {
        RestClient _restClient;
        string Token = string.Empty;

        public TuongTacCheoClient()
        {
            var options = new RestClientOptions("https://tuongtaccheo.com/")
            {

            };
            _restClient = new RestClient(options);
        }
        private async Task<string> GetSiteKey(string cookie)
        {
            var request = new RestRequest("https://tuongtaccheo.com/cauhinh/facebook.php", Method.Get);
            request.AddHeader("Cookie", cookie);

            var responsea = await _restClient.ExecuteAsync(request);
            string responseBody = responsea.Content;
            string pattern = @"data-sitekey=\""([^\""]*)\""";
            Match match = Regex.Match(responseBody, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }
        private async Task<string> AddAccount(string cookie, string uid, string token_Recaptcha)
        {
            string result = string.Empty;
            var request = new RestRequest("https://tuongtaccheo.com/cauhinh/nhapnick.php", Method.Post);
            request.AddHeader("Cookie", cookie);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("link", uid);
            request.AddParameter("loainick", "fb");
            request.AddParameter("recaptcha", token_Recaptcha);
            RestResponse response = await _restClient.ExecuteAsync(request);
            try
            {
                string responseBody = response.Content;
                if (!string.IsNullOrEmpty(responseBody) && responseBody == "1")
                {
                    return "success";
                }
                else if (!string.IsNullOrEmpty(responseBody) && responseBody == "3")
                {
                    result = $"ERROR: Tài khoản chưa đủ điều kiện để thêm vào tuongtaccheo.";
                }
                else
                {
                    result = $"ERROR: {responseBody}";
                }
            }
            catch (Exception ex)
            {
                result = $"ERROR: {ex.Message}";
            }
            return result;
        }
        public async Task<bool> AutoAddAccount(string cookie, string uid, string key_Captcha)
        {
            string sitekey = await GetSiteKey(cookie);
            if (string.IsNullOrEmpty(sitekey))
            {
                throw new Exception($"Không thể lấy Sitekey Captcha");
            }
            string id = await GuruCaptchaClient.GetIdCaptchaV2(key_Captcha, sitekey, $"https://tuongtaccheo.com/cauhinh/facebook.php");
            if (id.Contains("ERROR"))
            {
                throw new Exception($"TuongTacCheo: [{id}]");
            }
            string token = string.Empty;
            for (int i = 0; i < 120; i++)
            {
                token = await GuruCaptchaClient.GetTokenCaptchaV2(key_Captcha, id);
                if (token.Contains("ERROR"))
                {
                    await Task.Delay(1000);
                    continue;
                }
                else
                {
                    break;
                }
            }
            if (token.Contains("ERROR"))
            {
                throw new Exception($"TuongTacCheo: [{token}]");
            }
            string result = await AddAccount(cookie, uid, token);
            if (result == "success")
            {
                return true;
            }
            else
            {
                throw new Exception($"TuongTacCheo: [{result}]");
            }
        }
        public async Task<string> GetCookie(string token)
        {
            try
            {
                var options = new RestClientOptions("https://tuongtaccheo.com")
                {
                };
                var client = new RestClient(options);
                var request = new RestRequest("/logintoken.php", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("access_token", token);
                RestResponse response = await client.ExecuteAsync(request);
                var repont = response.Content;
                if (!repont.Contains("sodu"))
                {
                    return "";
                }

                var data = JObject.Parse(repont);
                try
                {
                    return response.Cookies![0].ToString();
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                }

                return "";
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return null;
        }
        public async Task<string> GetCoin(string token)
        {
            try
            {
                var options = new RestClientOptions("https://tuongtaccheo.com")
                {
                };
                var client = new RestClient(options);
                var request = new RestRequest("/logintoken.php", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("access_token", token);
                RestResponse response = await client.ExecuteAsync(request);
                var repont = response.Content;
                if (!repont.Contains("sodu"))
                {
                    throw new Exception(repont);
                }

                var data = JObject.Parse(repont);
                try
                {
                    return data["data"]["sodu"].ToString();
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                    throw ex;
                }

                return "";
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                throw ex;
            }
            return null;
        }
        public async Task<bool> DatNick(string cookie, string uid)
        {
            try
            {
                var options = new RestClientOptions("https://tuongtaccheo.com")
                {
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                };
                var client = new RestClient(options);
                var request = new RestRequest("/cauhinh/datnick.php", Method.Post);
                request.AddHeader("authority", "tuongtaccheo.com");
                request.AddHeader("accept", "*/*");
                request.AddHeader("accept-language", "vi,en;q=0.9,en-US;q=0.8,ja;q=0.7");
                request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
                request.AddHeader("cookie", cookie);
                request.AddHeader("origin", "https://tuongtaccheo.com");
                request.AddHeader("referer", "https://tuongtaccheo.com/cauhinh/facebook.php");
                request.AddHeader("sec-ch-ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("x-requested-with", "XMLHttpRequest");
                request.AddParameter("iddat[]", uid);
                request.AddParameter("loai", "fb");
                RestResponse response = await client.ExecuteAsync(request);
                return response.Content == "1";
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return false;
        }
        string GetJobPrefix(string job_type)
        {
            switch (job_type)
            {
                case JobTypes.Like: return SubdyHelper.GetStringRandom(new List<string> { "likepostvipre", "likepostvipcheo" });
                case JobTypes.Love:
                case JobTypes.Care:
                case JobTypes.Haha:
                case JobTypes.Wow:
                case JobTypes.Sad:
                case JobTypes.Angry:
                    {
                        return SubdyHelper.GetStringRandom(new List<string> { "camxucvipcheo", "camxucvipre" });
                    }
                case JobTypes.Share: return "sharecheo";
                case JobTypes.JoinGroup: return "thamgianhomcheo";
                case JobTypes.LikePage: return "likepagecheo";
                case JobTypes.LikeComment: return "camxuccheobinhluan";
                default: throw new Exception("TTC không hỗ trợ JobPrefix");
            }
        }
        public async Task<string> GetTokenByUsername(string username, string password)
        {
            var request = new RestRequest("https://tuongtaccheo.com/login.php", Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            request.AddParameter("submit", "ĐĂNG NHẬP");
            RestResponse response = await _restClient.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            string responseContent = response.Content;
            var cookieHeaders = response.Headers
                                        .Where(h => h.Name.Equals("Set-Cookie", StringComparison.OrdinalIgnoreCase))
                                        .Select(h => h.Value.ToString());

            string cookie = string.Join("; ", cookieHeaders);
            try
            {
                request = new RestRequest("https://tuongtaccheo.com/login.php", Method.Post);
                request.AddHeader("Cookie", cookie);
                request.AlwaysMultipartFormData = true;
                request.AddParameter("username", username);
                request.AddParameter("password", password);
                request.AddParameter("submit", "ĐĂNG NHẬP");
                response = await _restClient.ExecuteAsync(request);
                if (string.IsNullOrEmpty(cookie))
                {
                    throw new Exception($"Login TuongTacCheo ERROR: [Không thể get cookie {responseContent}]");
                }
                request = new RestRequest("https://tuongtaccheo.com/api/", Method.Get);
                request.AddHeader("Cookie", cookie);
                response = await _restClient.ExecuteAsync(request);
                string pattern = @"<input[^>]*\bname=""ttc_access_token""[^>]*\bvalue=""([^""]*)""";
                Match match = Regex.Match(response.Content, pattern);
                string token = string.Empty;
                if (match.Success)
                {
                    token = match.Groups[1].Value;
                }
                if (!string.IsNullOrEmpty(token))
                {
                    return token;
                }
                else
                {
                    throw new Exception($"TuongTacCheo Token: thất bại");
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                throw new Exception($"Login TuongTacCheo ERROR: [Không thể get cookie {ex.Message}]");
            }
            return null;
        }
        public async Task<JToken> GetFacebookJob(string cookie, string job_type = "")
        {
            try
            {

                string prefix = GetJobPrefix(job_type);

                var options = new RestClientOptions("https://tuongtaccheo.com")
                {
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                };
                var client = new RestClient(options);
                var request = new RestRequest($"/kiemtien/{prefix}/getpost.php", Method.Get);
                request.AddHeader("authority", "tuongtaccheo.com");
                request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
                request.AddHeader("accept-language", "vi,en;q=0.9,en-US;q=0.8,ja;q=0.7");
                request.AddHeader("cookie", cookie);
                request.AddHeader("referer", $"https://tuongtaccheo.com/kiemtien/{prefix}/");
                request.AddHeader("sec-ch-ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("x-requested-with", "XMLHttpRequest");
                RestResponse response = await client.ExecuteAsync(request);
                return JToken.Parse(response.Content!);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return null;
        }

        public async Task<JToken> ReportFacebookJob(string cookie, JobModel job)
        {
            try
            {
                string prefix = GetJobPrefix(job.Type);
                var options = new RestClientOptions("https://tuongtaccheo.com")
                {
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                };
                var client = new RestClient(options);
                var request = new RestRequest($"/kiemtien{prefix}/nhantien.php", Method.Post);
                request.AddHeader("authority", "tuongtaccheo.com");
                request.AddHeader("accept", "*/*");
                request.AddHeader("accept-language", "vi,en;q=0.9,en-US;q=0.8,ja;q=0.7");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("cookie", cookie);
                request.AddHeader("origin", "https://tuongtaccheo.com");
                request.AddHeader("referer", $"https://tuongtaccheo.com/kiemtien{prefix}/");
                request.AddHeader("sec-ch-ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("x-requested-with", "XMLHttpRequest");
                var body = $"id={job.ObjectId}";
                request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                RestResponse response = await client.ExecuteAsync(request);
                return JToken.Parse(response.Content!);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return null;
        }

    }
}
