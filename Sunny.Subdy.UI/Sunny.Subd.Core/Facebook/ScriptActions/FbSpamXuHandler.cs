using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Services;
using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.API.Jobs;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook.ScriptActions
{
    public class FbSpamXuHandler : MainService
    {
        public FbSpamXuHandler(string platform, ADBClient device, ConfigModel config, CancellationToken ct) : base(platform, device, config, ct)
        {
        }
        private JsonHelper _settingScriptAction;
        private string _jobService;
        private string _token;
        private List<string> _job_types = new List<string>();
        private ManualResetEventSlim waitForDoJobStart = new(false); // cho ScrollNewFeed đợi

        private List<JobModel> GetJob()
        {



            return new List<JobModel>();
        }
        private void StopScriptAction()
        {

        }
        public async Task<SubdyExtension> ExecuteAsync(ScriptAction action, ScriptActionContext context)
        {
            _settingScriptAction = new JsonHelper(action.Json);
            _jobService = JobServices.Types[_settingScriptAction.GetIntType("txtType", 0)];
            _token = _settingScriptAction.GetValuesFromInputString("txtKey", "");
            var lines = JobClient.GetJobTypes(_jobService, _token);
            foreach (var line in lines)
            {
                string key = line.Split(")").Last().Trim();
                if (_settingScriptAction.GetBooleanValue(key, true)) _job_types.Add(key);
            }

            // Khởi chạy ScrollNewFeed song song
            var scrollTask = Task.Run(() => ScrollNewFeed());
            while (true)
            {
                try
                {
                    await Stop();
                    StopScriptAction();

                    var jobs = GetJob();

                    if (!jobs.Any()) continue;

                    foreach (var job in jobs)
                    {
                        await Stop();
                        StopScriptAction();


                        // ✅ Báo cho ScrollNewFeed biết DoJob sắp bắt đầu
                        waitForDoJobStart.Set();
                        await DoJob(job);
                        // Reset lại để ScrollNewFeed có thể đợi lần tới
                        waitForDoJobStart.Reset();

                        await ReportJob(job);

                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {

                }
               

            }

            // Ví dụ: json là nội dung comment đơn thuần


            // Gửi tap + nhập nội dung + gửi comment qua ADBClient


            return new SubdyExtension(SubdyEnum.None, $"Đã gửi comment: ");
        }
        private async Task ScrollNewFeed()
        {
            while (true)
            {
                // ⏸ Đợi khi DoJob bắt đầu
                waitForDoJobStart.Wait();

                Console.WriteLine("Scroll bắt đầu chạy song song với DoJob");

                // Giả lập thao tác cuộn news feed (chạy song song với DoJob)
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Scroll round {i + 1}");
                    await Task.Delay(500); // ví dụ: scroll mỗi 0.5s
                }
            }
        }
        private async Task DoJob(JobModel job)
        {

        }
        private async Task ReportJob(JobModel job)
        {

        }
    }
}
