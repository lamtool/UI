using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sunny.Subdy.Common.Helper;
using System.Security.Policy;

namespace Sunny.Subdy.Common.API.Jobs.GoLike
{
    public class GoLikeClient : IJobBase
    {
        public static string UrlGetJob = "https://gateway.golike.net/api/advertising/publishers/_private/get-jobs?fb_id=";
        public static string UrlReportJob = "https://gateway.golike.net/api/advertising/publishers/_private/complete-jobs";
        public static string UrlJobTypes = "https://gateway.golike.net/api/advertising/publishers/_private/get-config";

        public string GetCoin(string token)
        {
            string apiUrl = "https://gateway.golike.net/api/statistics/report";
            var headers = new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {token}",
                ["ref"] = "LamToolAutoPhone",
                ["Cookie"] = "tool=dtasoftware;",
                ["dta"] = ""
            };
            string json = HttpRequestHelper.GET(apiUrl, headers: headers);
            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("Không lấy được kết quả từ server.");

            try
            {
                var responseObj = JObject.Parse(json);

                bool isSuccess = responseObj["success"]?.ToString() == "true";
                if (isSuccess)
                {
                    return json;
                }

                throw new Exception(responseObj["message"]?.ToString() ?? "Phản hồi từ server không thành công:\n" + json);
            }
            catch (JsonReaderException ex)
            {
                throw new Exception("Lỗi phân tích JSON:\n" + ex.Message + "\nRaw:\n" + json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JToken GetFacebookJob(string uid, string token, string job_type = "")
        {
            string url = $"{UrlGetJob}{uid}";

            var headers = new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {token}",
                ["ref"] = "LamToolAutoPhone",
                ["Cookie"] = "tool=dtasoftware;",
                ["dta"] = ""
            };

            string json = HttpRequestHelper.GET(url, headers: headers);
            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("Không lấy được kết quả từ server.");

            try
            {
                var responseObj = JObject.Parse(json);

                bool isSuccess = responseObj["success"]?.ToString() == "true";
                if (isSuccess)
                {
                    var jobs = responseObj["data"] as JArray;
                    if (jobs != null && jobs.Any())
                        return responseObj;

                    var message = responseObj["message"]?.ToString();
                    throw new Exception(!string.IsNullOrEmpty(message) ? message : "Không có job nào được trả về.");
                }

                throw new Exception(responseObj["message"]?.ToString() ?? "Phản hồi từ server không thành công:\n" + json);
            }
            catch (JsonReaderException ex)
            {
                throw new Exception("Lỗi phân tích JSON:\n" + ex.Message + "\nRaw:\n" + json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetJobTypes(string token)
        {
            string url = UrlJobTypes;

            var headers = new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {token}",
                ["ref"] = "LamToolAutoPhone",
                ["Cookie"] = "tool=dtasoftware;",
                ["dta"] = ""
            };

            string json = HttpRequestHelper.GET(url, headers: headers);
            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("Không lấy được kết quả từ server.");

            try
            {
                var responseObj = JObject.Parse(json);
                bool isSuccess = responseObj["success"] != null && Convert.ToBoolean(responseObj["success"]);

                if (!isSuccess)
                    throw new Exception(responseObj["message"]?.ToString() ?? "Phản hồi không thành công:\n" + json);

             
                var packageArray = responseObj["data"]?["facebook"]?["package_name"] as JArray;

                if (packageArray == null)
                {
                    throw new Exception("Không tìm thấy loại job:\n" + json);
                }
                List<string> lines = new List<string>();
                foreach (var item in packageArray)
                {
                    var fixCoin = item["fix_coin_job"]?.ToString() ?? "0";
                    var name = item["package_name"]?.ToString() ?? "";
                    lines.Add($"(+{fixCoin}) {name}");
                }
                return lines;
            }
            catch (JsonReaderException ex)
            {
                throw new Exception("Lỗi phân tích JSON:\n" + ex.Message + "\nRaw:\n" + json);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public JToken ReportFacebookJob(string uid, string fullname, string token, JobModel job)
        {
            var headers = new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {token}",
                ["ref"] = "LamToolAutoPhone",
                ["Cookie"] = "tool=dtasoftware;",
                ["dta"] = ""
            };

            var body = new Dictionary<string, string>
            {
                ["job_id"] = job.JobId,
                ["uid"] = uid,
                ["success"] = job.Success.ToString(),
                ["fb_name"] = fullname,
                ["id_text"] = "",
                ["post_private"] = job.IsView.ToString(),
                ["note"] = job.Link
            };

            if (job.Type == JobTypes.Comment && !string.IsNullOrEmpty(job.CommentId))
                body["comment_id"] = job.CommentId;

            string jsonBody = JsonConvert.SerializeObject(body);
            string json = HttpRequestHelper.POST_JSON(UrlReportJob, headers: headers, jsonBody: jsonBody);

            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("Không lấy được kết quả từ server.");

            try
            {
                var responseObj = JObject.Parse(json);
                bool isSuccess = responseObj["success"]?.ToString() == "true";

                if (isSuccess)
                    return responseObj;

                throw new Exception(responseObj["message"]?.ToString() ?? "Phản hồi không thành công:\n" + json);
            }
            catch (JsonReaderException ex)
            {
                throw new Exception("Lỗi phân tích JSON:\n" + ex.Message + "\nRaw:\n" + json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
