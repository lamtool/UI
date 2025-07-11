using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subdy.Common.API
{
    public class JobModel
    {
        public string JobId { get; set; } = "";
        public string FromId { get; set; } = "";
        public string ObjectId { get; set; } = "";
        public string Link { get; set; } = "";
        public List<string> Contents { get; set; } = new();
        public string Reaction = "";
        public string Type { get; set; } = "";
        public double Coin { get; set; } = 0;
        public string CommentId = "";
        public bool IsView { get; set; } = false;
        public bool Success { get; set; } = false;
        public JobModel()
        {
        }
        public JobModel(JObject job, string jobService, string job_type = "")
        {
            switch (jobService)
            {
                case JobServices.GoLike:
                    if (job.ContainsKey("id"))
                    {
                        JobId = job["id"]!.ToString();
                    }
                    if (job.ContainsKey("object_id"))
                    {
                        ObjectId = job["object_id"]!.ToString();
                    }
                    if (job.ContainsKey("type"))
                    {
                        Type = job["type"]!.ToString();
                    }
                    if (job.ContainsKey("fix_coin_job"))
                    {
                        Coin = Convert.ToDouble(job["fix_coin_job"]);
                    }
                    if (job.ContainsKey("data_comment"))
                    {
                        Contents = new List<string> { job["data_comment"]!["comment"]!.ToString() };
                        CommentId = job["data_comment"]!["id"]!.ToString();
                    }
                    if (job.ContainsKey("link"))
                    {
                        Link = job["link"]!.ToString();
                    }
                    break;

            }
        }
    }
}
