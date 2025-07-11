using Newtonsoft.Json.Linq;
using Sunny.Subdy.Common.API.Jobs.GoLike;
using System.Text.Json;

namespace Sunny.Subdy.Common.API.Jobs
{
    interface IJobBase
    {
        public JToken GetFacebookJob(string uid, string token, string job_type = "");
        public JToken ReportFacebookJob(string uid, string fullname, string token, JobModel job);
        public List<string> GetJobTypes(string token);
        public string GetCoin(string token);
    }
    public class JobClient
    {
        public static List<JobModel> GetFacebookJob(string jobService, string uid, string token, string job_type = "")
        {
            switch (jobService)
            {
                case JobServices.GoLike:
                    {
                        var golikeClient = new GoLikeClient();
                        JToken jGolike = golikeClient.GetFacebookJob(uid, token, job_type);

                        if (jGolike == null)
                        {
                            throw new Exception("Không lấy được kết quả từ server.");
                        }

                        var dataToken = jGolike["data"];
                        if (dataToken == null || dataToken.Type != JTokenType.Array)
                        {
                            throw new Exception("Dữ liệu job không hợp lệ.");
                        }

                        var jJobs = (JArray)dataToken;
                        var jobs = jJobs
                            .OfType<JObject>()
                            .Select(j => new JobModel(j, jobService, job_type))
                            .ToList();

                        if (jobs.Count > 0)
                        {
                            return jobs;
                        }

                        throw new Exception("Không có job nào.");
                    }

                default:
                    throw new Exception("JobService không hợp lệ.");
            }
        }

        public static string ReportFacebookJob(string jobService, string uid, string fullname, string token, JobModel job)
        {
            switch (jobService)
            {
                case JobServices.GoLike:
                    {
                        var golikeClient = new GoLikeClient();
                        JToken jGolike = golikeClient.ReportFacebookJob(uid, token, token, job);

                        if (jGolike == null)
                        {
                            throw new Exception("Không lấy được kết quả từ server.");
                        }
                        if (!string.IsNullOrEmpty(jGolike["message"]?.ToString()))
                        {
                            return jGolike["message"]?.ToString();
                        }
                        throw new Exception(jGolike.ToString());
                    }

                default:
                    throw new Exception("JobService không hợp lệ.");
            }

        }

        public static List<string> GetJobTypes(string jobService, string token)
        {
            switch (jobService)
            {
                case JobServices.GoLike:
                    {
                        var golikeClient = new GoLikeClient();
                        List<string> lines = golikeClient.GetJobTypes(token);

                        if (lines == null || !lines.Any())
                        {
                            throw new Exception("Không lấy được kết quả từ server.");
                        }
                        return lines;
                    }

                default:
                    throw new Exception("JobService không hợp lệ.");
            }
        }
        public static (double current_coin, double pending_coin) GetCoin(string jobService, string token)
        {
            switch (jobService)
            {
                case JobServices.GoLike:
                    {
                        var golikeClient = new GoLikeClient();
                        string json = golikeClient.GetCoin(token);

                        if (string.IsNullOrWhiteSpace(json))
                            throw new Exception("Không lấy được kết quả từ server.");

                        using JsonDocument doc = JsonDocument.Parse(json);
                        JsonElement root = doc.RootElement;

                        double currentCoin = root.TryGetProperty("current_coin", out JsonElement coinEl) && coinEl.TryGetDouble(out double val)
                            ? val
                            : 0;

                        string[] platforms = new[]
                        {
                    "facebook", "instagram", "tiktok", "youtube", "twitter",
                    "shopee", "lazada", "review", "traffic", "threads",
                    "linkedin", "snapchat", "pinterest"
                };

                        double pendingTotal = 0;

                        foreach (string platform in platforms)
                        {
                            if (root.TryGetProperty(platform, out JsonElement platformEl) &&
                                platformEl.TryGetProperty("pending_coin", out JsonElement pendingEl) &&
                                pendingEl.TryGetDouble(out double pendingVal))
                            {
                                pendingTotal += pendingVal;
                            }
                        }

                        return (currentCoin, pendingTotal);
                    }

                default:
                    throw new Exception("JobService không hợp lệ.");
            }
        }
    }
}
