using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Services;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.API.Jobs;
using Sunny.Subdy.Common.API.Jobs.GoLike;
using Sunny.Subdy.Common.API.Jobs.TuongTacCheo;
using Sunny.Subdy.Common.API.Jobs.VipIG;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.UI;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;

namespace Sunny.Subd.Core.Facebook.ScriptActions
{
    public class FbSpamXuHandler : MainService
    {
        public FbSpamXuHandler(string platform, ADBClient device, ConfigModel config, CancellationToken ct, JsonHelper settingScriptAction, Account account)
            : base(platform, device, config, ct)
        {
            _settingScriptAction = settingScriptAction;
            _account = account;

        }
        private Dictionary<string, int> _doJobInfo = new Dictionary<string, int>();
        private string _jobService;
        private HashSet<string> _job_types = new();
        private Stopwatch _stopwatchScriptAction = new();
        private ManualResetEventSlim waitForDoJobStart = new(false);
        private int _timeoutScriptAction, _stopJobTatolAccount, _stopJobBlockTuongTac, _stopJobFail, _removeJob, _stopJobToday;
        private int _blockJobTuongTac, _jobFail_LienTiep, _countJob;
        private string _typeJob = string.Empty;
        private string _tokenJobService = string.Empty;
        private bool _isStop = false;
        private JobHistoryContext _jobHistoryContext = new JobHistoryContext();
        private Dictionary<string, string> _infoAccountService = new Dictionary<string, string>();
        private string _cookieService = string.Empty;
        private async Task<List<JobModel>> GetJob()
        {
            List<JobModel> jobs = new List<JobModel>();
            string error = string.Empty;
            int index = _settingScriptAction.GetIntType("numericUpDown48", 100);
            int delay = _settingScriptAction.GetIntType("numericUpDown46", 3);
            if (_platform == PlatformModel.Instagram && _jobService == JobServices.VipIG)
            {
                var vipigClient = new VipIGClient(_cookieService);
                for (int i = 0; i < 10; i++)
                {
                    _sate = $"{i + 1}/10 Đăng nhập vipig";
                    var coin = await vipigClient.GetBalance();
                    if (!string.IsNullOrEmpty(coin))
                    {
                        _account.Result = coin;
                        break;
                    }
                    var accVipIG = await vipigClient.LoginByToken(_account.TokenJob);
                    if (string.IsNullOrEmpty(accVipIG))
                        throw new Exception("Đăng nhập vipig.net lỗi.");
                    if (string.IsNullOrEmpty(accVipIG.Split('|')[0]))
                    {
                        await DelayMessageAsync(150, accVipIG.Split('|')[1], 2);
                        continue;
                    }
                    _cookieService = accVipIG.Split('|')[2];
                    _account.Result = accVipIG.Split('|')[1];
                    break;
                }


            }
            for (int i = 1; i <= index; i++)
            {
                _sate = $"Lấy job {i}/{index}";
                try
                {
                    _typeJob = SubdyHelper.GetStringRandom(_job_types.ToList());

                    if (_platform == PlatformModel.Facebook)
                    {
                        jobs = await JobClient.GetFacebookJob(_jobService, _account.Uid, _tokenJobService, _typeJob);
                    }
                    else if (_platform == PlatformModel.Instagram && _jobService == JobServices.GoLike)
                    {
                        jobs = await new GoLikeClient().GetInstagramJob(_infoAccountService["id"], _account.TokenJob);
                    }
                    else if (_platform == PlatformModel.Instagram && _jobService == JobServices.VipIG)
                    {
                        var vipigClient = new VipIGClient(_cookieService);
                        jobs = await vipigClient.GetJobInstagram(_typeJob);
                    }
                    var jobsResult = jobs.FindAll(x => _job_types.Contains(x.Type));
                    if (!jobsResult.Any())
                    {
                        throw new Exception("Không có job phù hợp theo yêu cầu.");
                    }
                    return jobsResult;
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                    error = ex.Message;
                }
                await DelayMessageAsync(delay, error, 2);
            }

            throw new Exception($"Kết thúc hành động khi get job thất bại liên tiếp {index} lần");
        }

        private async Task StopScriptAction()
        {
            await Stop();

            if (_timeoutScriptAction > 0 && _stopwatchScriptAction.IsRunning &&
                _stopwatchScriptAction.ElapsedMilliseconds > _timeoutScriptAction)
            {
                _stopwatch.Restart();
                SetStatus("Đã quá thời gian thực hiện hành động, dừng tài khoản.", 1);
                throw new TimeoutException();
            }

            if (ShouldStopByJobLimit(_account.JobTotal, _stopJobTatolAccount, "tổng số job")) return;
            if (ShouldStopByJobLimit(_blockJobTuongTac, _stopJobBlockTuongTac, "bị chặn tương tác")) return;
            if (ShouldStopByJobLimit(_jobFail_LienTiep, _stopJobFail, "job thất bại liên tiếp")) return;

            if (_doJobInfo.ContainsKey($"{_typeJob}_faillientiep") && _removeJob > 0 && _doJobInfo[$"{_typeJob}_faillientiep"] >= _removeJob)
            {
                await DelayMessageAsync(_settingScriptAction.GetIntType("numericUpDown20", 30),
                    $"Xóa loại job [{_typeJob}] thất bại liên tiếp {_removeJob}", 2);
                _job_types.Remove(_typeJob);
                _jobFail_LienTiep = 0;
            }

            if (ShouldStopByJobLimit(Convert.ToInt32(_account.JobToday.Split("/")?.First()), _stopJobToday, "job/ngày")) return;

            await HandleBreakTime();

            RemoveOverLimitReactions();
        }

        private bool ShouldStopByJobLimit(int current, int limit, string reason)
        {
            if (limit > 0 && current >= limit)
            {
                DelayMessageAsync(_settingScriptAction.GetIntType("numericUpDown17", 30),
                    $"Kết thúc hành động khi {reason} đạt {limit}", 2).Wait();
                throw new Exception($"Dừng khi {reason} đạt {limit}");
            }
            return false;
        }

        private async Task HandleBreakTime()
        {
            int delayCount = SubdyHelper.RandomValue(
                _settingScriptAction.GetIntType("numericUpDown10", 5),
                _settingScriptAction.GetIntType("numericUpDown9", 10));

            if (_settingScriptAction.GetBooleanValue("checkBox2", true) && _countJob > 0 && _countJob % delayCount == 0)
            {
                waitForDoJobStart.Reset();
                int restTime = SubdyHelper.RandomValue(
                    _settingScriptAction.GetIntType("numericUpDown8", 60),
                    _settingScriptAction.GetIntType("numericUpDown7", 120));

                if (_settingScriptAction.GetBooleanValue("radioButton1", true))
                {
                    await DelayMessageAsync(restTime, $"Làm {delayCount} job liên tiếp, nghỉ giải lao", 2);
                }
                else if (_settingScriptAction.GetBooleanValue("radioButton3", false))
                {
                    waitForDoJobStart.Set();
                    await DelayMessageAsync(restTime, $"Làm {delayCount} job liên tiếp, lướt newfeed", 2);
                }
                else if (_settingScriptAction.GetBooleanValue("radioButton2", false))
                {
                    _client.ADB.Shell("am start -n com.facebook.katana/.IntentUriHandler \"fb://watch\"");
                    await DelayMessageAsync(restTime, $"Làm {delayCount} job liên tiếp, xem video", 2);
                    _client.Shell("input keyevent 4");

                }

                waitForDoJobStart.Set();
            }
        }

        private void RemoveOverLimitReactions()
        {
            var checks = new (string Type, string Low, string High)[]
            {
            (JobTypes.Like, "numericUpDown3", "numericUpDown4"),
            (JobTypes.Love, "numericUpDown6", "numericUpDown5"),
            (JobTypes.Care, "numericUpDown27", "numericUpDown26"),
            (JobTypes.Haha, "numericUpDown31", "numericUpDown30"),
            (JobTypes.Sad, "numericUpDown29", "numericUpDown28"),
            (JobTypes.Wow, "numericUpDown33", "numericUpDown32"),
            (JobTypes.Angry, "numericUpDown43", "numericUpDown42"),
            (JobTypes.LikePage, "numericUpDown45", "numericUpDown44"),
            (JobTypes.JoinGroup, "numericUpDown39", "numericUpDown38"),
            (JobTypes.Share, "numericUpDown37", "numericUpDown38"),
            (JobTypes.Follow, "numericUpDown35", "numericUpDown34"),
            (JobTypes.LikeComment, "numericUpDown41", "numericUpDown40")
            };

            foreach (var (type, minKey, maxKey) in checks)
            {
                if (_job_types.Contains(type) && _doJobInfo.ContainsKey(type) && _doJobInfo[type] >=
                    SubdyHelper.RandomValue(_settingScriptAction.GetIntType(minKey, 100),
                                            _settingScriptAction.GetIntType(maxKey, 500)))
                {
                    _job_types.Remove(type);
                }
            }
        }

        public async Task<SubdyExtension> ExecuteAsync(ScriptAction action, ScriptActionContext context)
        {
            try
            {
                await InitSettings();
                _client.AppStart(FacebookHander.Package(_platform), true, true, true);
                _ = Task.Run(ScrollNewFeed); // background task

                while (true)
                {
                    try
                    {
                        await StopScriptAction();

                        var jobs = await GetJob();

                        if (jobs == null || !jobs.Any())
                        {
                            continue;
                        }
                        List<string> listJob = new List<string>();
                        bool isClaim = true;
                        for (int index = 0; index < jobs.Count; index++)
                        {
                            var job = jobs[index];
                            if (job == null)
                            {
                                continue;
                            }
                            _typeJob = job.Type;
                            _sate = $"{_typeJob.ToUpper()} {index + 1}/{jobs.Count}";
                            if (_typeJob == JobTypes.Follow && jobs.Count < 5 && _jobService == JobServices.VipIG && _platform == PlatformModel.Instagram)
                            {
                                int second = SubdyHelper.RandomValue(
                                    _settingScriptAction.GetIntType("nudJobDelayFrom", 5),
                                    _settingScriptAction.GetIntType("nudJobDelayTo", 10)
                                );

                                await DelayMessageAsync(second, $"Không đủ trên {jobs.Count}/5 job follow.", 2);
                                break;
                            }


                            SubdyExtension subdy = null;
                            try
                            {
                                await StopScriptAction();

                                waitForDoJobStart.Reset();

                                string jobIdShort = job.ObjectId?.Length >= 3 ? job.ObjectId.Substring(0, 3) : job.ObjectId ?? "null";
                                string jobType = job.Type ?? "unknown";

                                await DelayMessageAsync(5, $"Chuẩn bị làm job {jobType} jobId: {jobIdShort}... ", 2);

                                subdy = await DoJob(job);

                                subdy = await HanderDoJob(job, subdy);

                                waitForDoJobStart.Set();

                                if (_jobService == JobServices.VipIG && job.Type == JobTypes.Follow)
                                {
                                    listJob.Add(job.JobId);
                                    isClaim = false;
                                    if (listJob.Count == jobs.Count)
                                    {
                                        isClaim = true;
                                        subdy.SubdyEnum = SubdyEnum.Success;
                                    }
                                }
                                if (isClaim)
                                {
                                    subdy = await ReportJob(job, subdy, listJob);
                                }

                                HanderJob(subdy, job);

                                int second = SubdyHelper.RandomValue(
                                    _settingScriptAction.GetIntType("nudJobDelayFrom", 5),
                                    _settingScriptAction.GetIntType("nudJobDelayTo", 10)
                                );

                                await DelayMessageAsync(second, $"{subdy.Message} - [Delay tương tác tiếp theo]", 2);
                            }
                            finally
                            {
                                UpdateJob(subdy, job);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        LogManager.Error(ex);
                        throw ex;
                    }

                }
            }
            finally
            {
                _isStop = true;

            }


            // Không bao giờ đến đây
            // return new SubdyExtension(SubdyEnum.None, "Done");
        }
        private void UpdateJob(SubdyExtension subdy, JobModel job)
        {
            try
            {
                var model = new JobHistory
                {
                    Id = Guid.NewGuid(),
                    IdJob = job.JobId,
                    IdObject = job.ObjectId,
                    Coin = job.Coin.ToString(),
                    Uid = _account.Uid,
                    Service = _jobService,
                    Method = job.Type,
                    Description = subdy.Message,
                    Platform = _platform,
                    Status = subdy?.SubdyEnum == SubdyEnum.Success
                      ? SubdyEnum.Success.ToString()
                      : SubdyEnum.JobFail.ToString()
                };

                _jobHistoryContext.Add(model);
                var result = _jobHistoryContext.GetHistorySummaryToDayByUid(_account.Uid, _platform, _jobService);
                _account.JobToday = $"{result["Success"]}/{result["XuSuccess"]}";
                string summary = string.Empty;
                string summary_Skip = string.Empty;
                foreach (var item in result)
                {
                    string key = item.Key;
                    if (key == "Total" || key == "Success" || key == "Fail" || key == "XuSuccess" || key == "XuFail")
                    {
                        continue;
                    }
                    if (key.Contains("_Skip"))
                    {
                        summary_Skip += $"{key.Replace("_Skip", "")}: {item.Value}, ";
                    }
                    else
                    {
                        summary += $"{key}: {item.Value}, ";
                    }

                }
                _account.Summary = summary;
                _account.Summary_Skip = summary_Skip;
                _accountContext.Update(_account);
                LoadControlModelHelper.LoadToolStripAccount(_platform, _jobService, "", LoadControlModelHelper.ToolStripAccount, _jobHistoryContext);
            }
            catch
            {

            }
        }
        private async Task InitSettings()
        {
            if (_settingScriptAction.GetBooleanValue("ckbTimeoutScript"))
            {
                _stopwatchScriptAction.Restart();
                _timeoutScriptAction = SubdyHelper.RandomValue(
                    _settingScript.GetIntType("numericUpDown5", 40),
                    _settingScript.GetIntType("numericUpDown4", 60)) * 60 * 1000;
            }

            if (_settingScriptAction.GetBooleanValue("checkBox3", true))
                _stopJobTatolAccount = SubdyHelper.RandomValue(_settingScriptAction.GetIntType("numericUpDown14", 100),
                                                               _settingScriptAction.GetIntType("numericUpDown13", 200));

            if (_settingScriptAction.GetBooleanValue("checkBox4", true))
                _stopJobBlockTuongTac = SubdyHelper.RandomValue(_settingScriptAction.GetIntType("numericUpDown12", 10),
                                                                 _settingScriptAction.GetIntType("numericUpDown11", 20));

            if (_settingScriptAction.GetBooleanValue("checkBox5", true))
                _stopJobFail = SubdyHelper.RandomValue(_settingScriptAction.GetIntType("numericUpDown16", 100),
                                                        _settingScriptAction.GetIntType("numericUpDown15", 200));

            if (_settingScriptAction.GetBooleanValue("checkBox6", true))
                _removeJob = SubdyHelper.RandomValue(_settingScriptAction.GetIntType("numericUpDown22", 100),
                                                     _settingScriptAction.GetIntType("numericUpDown21", 200));

            if (_settingScriptAction.GetBooleanValue("checkBox7", true))
                _stopJobToday = SubdyHelper.RandomValue(_settingScriptAction.GetIntType("numericUpDown25", 100),
                                                        _settingScriptAction.GetIntType("numericUpDown24", 200));

            int indexType = _settingScriptAction.GetIntType("txtType", 0);


            if (_platform == PlatformModel.Facebook)
            {
                _jobService = JobServices.TypesFacebook[indexType];
                switch (_jobService)
                {
                    case JobServices.TuongTacCheo:
                        {
                            if (string.IsNullOrEmpty(_account.TokenJob))
                            {
                                throw new Exception("Vui lòng thêm token tuongtaccheo.");
                            }
                            var tuongtaccheoclient = new TuongTacCheoClient();
                            _tokenJobService = await tuongtaccheoclient.GetCookie(_account.TokenJob);
                            if (!await tuongtaccheoclient.DatNick(_tokenJobService, _account.Uid))
                            {
                                string keyCaptcha = _settingScriptAction.GetValuesFromInputString("txtKey", "");
                                await tuongtaccheoclient.AutoAddAccount(_tokenJobService, _account.Uid, keyCaptcha);
                                if (!await tuongtaccheoclient.DatNick(_tokenJobService, _account.Uid))
                                {
                                    throw new Exception("Cấu hình tài khoản tuongtaccheo thất bại.");
                                }
                            }
                            break;
                        }
                    case JobServices.GoLike:
                        {
                            _tokenJobService = _settingScriptAction.GetValuesFromInputString("txtKey", "");
                            break;
                        }
                }
            }
            else if (_platform == PlatformModel.Instagram)
            {
                _jobService = JobServices.TypesInstagram[indexType];
                if (_settingScriptAction.GetBooleanValue("checkBox19"))
                {
                    await Globals.Semaphore.WaitAsync();
                    try
                    {
                        var list = _settingScriptAction.GetValuesList("textBox1");
                        if (!list.Any())
                        {
                            throw new Exception("Không có token job service.");
                        }
                        _account.TokenJob = list[0];
                        list.RemoveAt(0);
                        _settingScriptAction.AddValueList("textBox1", list);
                    }
                    finally
                    {
                        Globals.Semaphore.Release();
                    }
                }
                if (string.IsNullOrEmpty(_account.TokenJob))
                {
                    throw new Exception("Không có token job service.");
                }
                switch (_jobService)
                {
                    case JobServices.GoLike:
                        {
                            var client = new GoLikeClient();
                            if (string.IsNullOrEmpty(_account.FullName) || string.IsNullOrEmpty(_account.Bio))
                            {
                                var accountIg = await client.GetAccount(_account.TokenJob);
                                if (accountIg.ContainsKey("error"))
                                {
                                    throw new Exception($"Get info account golike lỗi: {accountIg["error"]}");
                                }
                                string fullname = string.Empty;
                                if (string.IsNullOrEmpty(_account.FullName))
                                {
                                    fullname = $"{SubdyHelper.GetStringRandom(SubdyHelper.FirstnameVN)} {SubdyHelper.GetStringRandom(SubdyHelper.LastnameVN)}";
                                }
                                string code = accountIg["code"];


                                var value = await _facebookService.UpateInfo(_client, fullname, code, "");
                                if (value.ContainsKey("fullname"))
                                {
                                    _account.FullName = value["fullname"];
                                }
                                if (value.ContainsKey("bio"))
                                {
                                    _account.Bio = value["bio"];
                                }
                                _accountContext.Update(_account);
                            }
                            bool isvery = false;
                            for (int i = 0; i < 2; i++)
                            {
                                var accountIg = await client.GetInstagramAccount(_account.TokenJob);
                                if (accountIg.ContainsKey("error"))
                                {
                                    throw new Exception($"Get list id account golike lỗi: {accountIg["error"]}");
                                }
                                if (!accountIg.ContainsKey(_account.Uid))
                                {
                                    accountIg = await client.VerifyAccountInstagram(_account.TokenJob, _account.UserName);
                                    if (accountIg.ContainsKey("error"))
                                    {
                                        throw new Exception($"Verify account instagram golike lỗi: {accountIg["error"]}");
                                    }
                                    SetStatus(accountIg["success"], 2);
                                    isvery = false;
                                    continue;
                                }
                                else
                                {
                                    isvery = true;
                                    break;
                                }

                            }
                            if (!isvery)
                            {
                                throw new Exception("Đã xảy ra khi thêm tài khoản golike...");
                            }
                            break;
                        }
                    case JobServices.VipIG:
                        {
                            VipIGClient vipigClient = new VipIGClient();
                            var accVipIG = await vipigClient.LoginByToken(_account.TokenJob);
                            if (string.IsNullOrEmpty(accVipIG))
                                throw new Exception("Đăng nhập vipig.net lỗi.");
                            _cookieService = accVipIG.Split('|')[2];
                            _account.Result = accVipIG.Split('|')[1];

                            _sate = "Cấu hình tài khoản";
                            SetStatus("Đang cấu hình tài khoản.", 2);
                            bool configured;
                            if (!_settingScriptAction.GetBooleanValue("check_AddAccount", false))
                            {
                                configured = await vipigClient.CauHinh(_account.Uid);
                                if (!configured) throw new Exception("Cấu hình không hợp lệ");

                                bool check = false;
                                string id = await vipigClient.GetIdByUsername(_account.Uid);
                                if (string.IsNullOrEmpty(id))
                                {
                                    check = await vipigClient.DatNick(_account.Uid) != 1;
                                }
                                if (!check)
                                {
                                    if (!await vipigClient.CauHinhNhanh(_account.Uid))
                                        throw new Exception($"Cần thêm nick: {_account.Uid} vào trước khi chạy");
                                }
                            }
                            else
                            {
                                _sate = "Cấu hình tài khoản nhanh";
                                SetStatus("Đang cấu hình tài khoản.", 2);
                                bool check = false;
                                string id = await vipigClient.GetIdByUsername(_account.Uid);
                                if (!string.IsNullOrEmpty(id))
                                {
                                    check = await vipigClient.DatNick(id) == 1;
                                }
                                if (!check)
                                {
                                    if (!await vipigClient.CauHinhNhanh(_account.Uid))
                                        throw new Exception($"Cần thêm nick: {_account.Uid} vào trước khi chạy");
                                }

                            }



                            break;
                        }
                }
            }
            if (_settingScriptAction.GetBooleanValue("checkBox1", true))
            {
                _job_types.Add(JobTypes.Like);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox8", true))
            {
                _job_types.Add(JobTypes.Love);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox9", true))
            {
                _job_types.Add(JobTypes.Care);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox11", true))
            {
                _job_types.Add(JobTypes.Haha);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox10", true))
            {
                _job_types.Add(JobTypes.Sad);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox12", true))
            {
                _job_types.Add(JobTypes.Wow);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox15", true))
            {
                _job_types.Add(JobTypes.Angry);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox18", true))
            {
                _job_types.Add(JobTypes.LikePage);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox17", true))
            {
                _job_types.Add(JobTypes.JoinGroup);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox14", true))
            {
                _job_types.Add(JobTypes.Share);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox13", true))
            {
                _job_types.Add(JobTypes.Follow);
            }
            if (_settingScriptAction.GetBooleanValue("checkBox16", true))
            {
                _job_types.Add(JobTypes.LikeComment);
            }
        }

        private async Task ScrollNewFeed()
        {
            while (!_isStop)
            {
                waitForDoJobStart.Wait();
                if (!_settingGeneral.GetBooleanValue("checkBox14", true)) continue;
                if (!_client.Package(FacebookHander.Package(_platform), 1))
                {
                    if (_platform == PlatformModel.Facebook)
                    {
                        _client.Shell($"am start -n com.facebook.katana/.IntentUriHandler \"fb://feed\"");
                    }
                    else if (_platform == PlatformModel.Instagram)
                    {
                        _client.AppStart(FacebookHander.Package(_platform));
                    }
                    continue;
                }
                _client.ElementWithAttributes("//*[@content-desc=\"Close\"]", timeoutInSeconds: 1);

                _client.SwipeByPercent(52, 92, 52, 45, 1000, SubdyHelper.RandomValue(1, 5));
                int second = SubdyHelper.RandomValue(1, 15);
                for (int i = 0; i < second; i++)
                {
                    waitForDoJobStart.Wait();
                    await Task.Delay(1000);
                }

            }
        }

        private async Task<SubdyExtension> DoJob(JobModel job)
        {
            SubdyExtension extension = new SubdyExtension(SubdyEnum.JobFail, $"Chưa hỗ trợ loại {job.Type} này.");
            switch (_typeJob)
            {
                case JobTypes.Like:
                case JobTypes.Love:
                case JobTypes.Care:
                case JobTypes.Haha:
                case JobTypes.Wow:
                case JobTypes.Sad:
                case JobTypes.Angry:
                case JobTypes.LikeComment:
                    {
                        return await JobReaction(job);
                        break;
                    }
                case JobTypes.Follow:
                    {
                        return await JobFollow(job);
                    }
            }



            return extension;
        }
        private async Task<SubdyExtension> HanderDoJob(JobModel job, SubdyExtension subdy)
        {
            if (subdy.SubdyEnum == SubdyEnum.Success && !string.IsNullOrEmpty(job.Link))
            {
                _client.ADB.Shell("input keyevent 4");
                if (await GotoUrl(job.Link))
                {
                    string type = job.Type.ToLower();
                    if (type == JobTypes.Like || type == JobTypes.Love ||
                       type == JobTypes.Sad || type == JobTypes.Haha ||
                       type == JobTypes.Wow || type == JobTypes.Angry ||
                       type == JobTypes.Care || type == JobTypes.LikeComment)
                    {
                        if (_platform == PlatformModel.Facebook)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                string dump = _client.GetXMLSource();
                                if (string.IsNullOrEmpty(dump))
                                {
                                    continue;
                                }
                                dump = dump.ToLower();
                                if (!_client.ElementWithAttributes("//*[@text=\"Share\"]", 1, dump, false) || !dump.Contains("share") && !dump.Contains("like"))
                                {
                                    _client.SwipeByPercent(56, 82, 56, 16, 1000);
                                    continue;
                                }
                                break;
                            }
                        }

                    }

                    string xml = _client.GetXMLSource().ToLower();
                    if (xml.Contains("log into another account"))
                    {
                        subdy.SubdyEnum = SubdyEnum.LogOut;
                        subdy.Message = $"Tài khoản logout";
                    }
                    else if (xml.Contains("you can't use this feature right now"))
                    {
                        subdy.SubdyEnum = SubdyEnum.Block;
                        subdy.Message = $"Tài khoản bị chặn chức năng [You Can't Use This Feature Right Now]";
                    }
                    else if (xml.Contains("sorry, something went wrong"))
                    {
                        subdy.SubdyEnum = SubdyEnum.Block;
                        subdy.Message = "Có thể tài khoản đã bị chặn tương tác [sorry, something went wrong]";
                    }
                    else if (xml.Contains("text=\"cancel\""))
                    {
                        subdy.SubdyEnum = SubdyEnum.LogOut;
                        subdy.Message = "không load được trang facebook";
                    }
                    else if (xml.Contains("use facebook without messaging"))
                    {
                        subdy.SubdyEnum = SubdyEnum.JobFail;
                        subdy.Message = $"Không tìm thấy nút : {job.Type.ToUpper()} [Use Facebook without messaging]";
                    }
                    else if (xml.Contains("go to news feed") || xml.Contains("see more on facebook"))
                    {
                        subdy.SubdyEnum = SubdyEnum.JobFail;
                        subdy.Message = $"Link lỗi hoặc không tồn tại! LINK: [{job.Link}]";
                    }
                    if (type == JobTypes.Like || type == JobTypes.Love ||
                      type == JobTypes.Sad || type == JobTypes.Haha ||
                      type == JobTypes.Wow || type == JobTypes.Angry ||
                      type == JobTypes.Care || type == JobTypes.LikeComment)
                    {
                        if (_platform == PlatformModel.Facebook && !xml.Contains(", pressed. double tap and hold"))
                        {
                            subdy.SubdyEnum = SubdyEnum.Block;
                            subdy.Message = $"Chặn tương tác {job.Type}";
                        }
                        else if (_platform == PlatformModel.Instagram && !xml.Contains("liked"))
                        {
                            subdy.SubdyEnum = SubdyEnum.Block;
                            subdy.Message = $"Chặn tương tác {job.Type}";
                        }
                    }
                    if (type == JobTypes.Follow)
                    {
                        if (_platform == PlatformModel.Instagram)
                        {
                            bool check = xml.Contains("requested");
                            if (!check)
                            {
                                check = (_client.FindElements(1, "", "//*[@resource-id=\"com.instagram.android:id/profile_header_user_action_follow_button\"]").Any() && _client.FindElements(1, "", "//*[@resource-id=\"com.instagram.android:id/profile_header_user_action_follow_button\"]")[0].OuterXml.ToLower().Contains("following"));
                            }
                            if (!check)
                            {
                                subdy.SubdyEnum = SubdyEnum.Block;
                                subdy.Message = $"Chặn tương tác {job.Type}";
                            }
                        }

                    }
                }
            }
            _client.ADB.Shell("input keyevent 4");
            return subdy;
        }
        private async Task<SubdyExtension> ReportJob(JobModel job, SubdyExtension subdy, List<string> idJobs = null)
        {
            if (subdy.SubdyEnum == SubdyEnum.Success)
            {
                try
                {
                    if (_platform == PlatformModel.Facebook)
                    {
                        subdy.Message = await JobClient.ReportFacebookJob(_jobService, _account.Uid, _account.FullName, _tokenJobService, job);
                    }
                    else if (_platform == PlatformModel.Instagram)
                    {
                        if (_jobService == JobServices.GoLike)
                        {
                            var message = await new GoLikeClient().ReportInstagramJob(job.JobId, _infoAccountService["id"], _account.TokenJob);
                            if (message.ContainsKey("error"))
                            {
                                throw new Exception(message["error"]);
                            }
                            subdy.Message = message["success"];
                        }
                        else if (_jobService == JobServices.VipIG && job.Type == JobTypes.Like)
                        {
                            var reward = await new VipIGClient(_cookieService).ClaimLikeReward(job.ObjectId);
                            string message = (reward["mess"] ?? reward["error"])?.ToString();
                            subdy.Message = message;
                        }
                        else if (_jobService == JobServices.VipIG && job.Type == JobTypes.Follow)
                        {
                            string idList = string.Join(",", idJobs);
                            idJobs.Clear();
                            var reward = await new VipIGClient(_cookieService).ClaimFollowReward(idList);
                            string message = (reward["mess"] ?? reward["error"])?.ToString();
                            subdy.Message = message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    subdy.SubdyEnum = SubdyEnum.JobFail;
                    subdy.Message = ex.Message;
                }
            }

            return subdy;
        }
        private void EnsureKey(string key)
        {
            if (!_doJobInfo.ContainsKey(key))
                _doJobInfo[key] = 0;
        }
        private void HanderJob(SubdyExtension subdy, JobModel job)
        {

            string type = job.Type;
            EnsureKey(type);
            EnsureKey($"{type}_fail");
            EnsureKey($"{type}_faillientiep");

            switch (subdy.SubdyEnum)
            {
                case SubdyEnum.LogOut:
                    throw subdy;

                case SubdyEnum.Block:
                    _blockJobTuongTac++;
                    _doJobInfo[$"{type}_faillientiep"]++;
                    _doJobInfo[$"{type}_fail"]++;
                    break;

                case SubdyEnum.JobFail:
                    _jobFail_LienTiep++;
                    _doJobInfo[$"{type}_faillientiep"]++;
                    _doJobInfo[$"{type}_fail"]++;
                    break;

                case SubdyEnum.Success:
                    _jobFail_LienTiep = 0;
                    _doJobInfo[type]++;
                    _doJobInfo[$"{type}_faillientiep"] = 0;
                    _account.JobTotal++;
                    break;
            }

            _countJob++;
        }
        private async Task<SubdyExtension> JobReaction(JobModel job)
        {
            if (_platform == PlatformModel.Facebook)
            {
                bool isLink = false;
                List<string> urls = new List<string>();
                string url = await FacebookHander.GetUrlByObjectId(job.ObjectId);
                if (string.IsNullOrEmpty(url))
                {
                    //string charString = SubdyHelper.RandomString("abcdefghijklmnopqrstuvwxyz", SubdyHelper.RandomValue(3, 10));
                    urls = new List<string>
                {
                    $"https://www.facebook.com/abc/posts/{job.ObjectId}",
                    $"https://www.facebook.com/photo/?fbid={job.ObjectId}",
                    $"https://www.facebook.com/permalink.php?story_fbid={job.ObjectId}"
                };

                }
                else
                {
                    urls.Add(url);
                }
                foreach (string link in urls)
                {
                    isLink = await GotoUrl(link);
                    if (isLink)
                    {
                        url = link;
                        break;
                    }
                    _client.Shell("input keyevent 4");
                }
                if (!isLink)
                {
                    return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Không tồn tại bài viết.");
                }
            ReFail:
                isLink = false;
                for (int i = 0; i < 10; i++)
                {
                    string dump = _client.GetXMLSource();
                    if (string.IsNullOrEmpty(dump))
                    {
                        continue;
                    }
                    dump = dump.ToLower();
                    if (!_client.ElementWithAttributes("//*[@text=\"Share\"]", 1, dump, false) || !dump.Contains("share") && !dump.Contains("like"))
                    {
                        _client.SwipeByPercent(56, 82, 56, 16, 1000);
                        continue;
                    }
                    if (dump.Contains(", pressed. double tap and hold"))
                    {
                        return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Đã làm job đó trước.");
                    }
                    isLink = true;
                    break;
                }
                if (!isLink)
                {
                    return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Không tìm được nút {job.Type}.");
                }
                job.Link = url;
                int second = SubdyHelper.RandomValue(_settingScriptAction.GetIntType("numericUpDown2", 5), _settingScriptAction.GetIntType("numericUpDown1", 10));
                await DelayMessageAsync(second, "Delay trước khi click tương tác", 2);
                var element = _client.FindElement("", new List<string> { "//*[@content-desc=\"Tap to open more options\"]", "//*[contains(@content-desc, 'Like button')]", "//*[contains(@content-desc, 'Like. Double')]" }, 5);
                if (element == "//*[@content-desc=\"Tap to open more options\"]")
                {
                    _client.ElementWithAttributes(element, 5);
                    _client.ElementWithAttributes("//*[@content-desc=\"Hide\"]", 5);
                    goto ReFail;
                }
                var elementLike = _client.FindPoint(element, 15);
                string type = job.Type.ToLower();
                if (elementLike != null && elementLike != System.Drawing.Point.Empty)
                {
                    string num = job.Type.ToLower();

                    _client.LongClick(elementLike.X, elementLike.Y, 1000);
                    if (!type.Contains("like"))
                    {
                        num = char.ToUpper(type[0]) + type.Substring(1);
                    }
                    if (_client.ElementWithAttributes($"//*[@content-desc='{num}']", 3))
                    {
                        return new SubdyExtension(SubdyEnum.Success, $"Job: {job.ObjectId?.Substring(0, 3)}... success.");
                    }
                }
            }
            else if (_platform == PlatformModel.Instagram)
            {
                string link = $"instagram://media?id={job.ObjectId}";
                if (!await GotoUrl(link))
                {
                    return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Không tồn tại bài viết.");
                }
                bool isLink = false;
                for (int i = 0; i < 10; i++)
                {
                    string dump = _client.GetXMLSource();
                    if (string.IsNullOrEmpty(dump))
                    {
                        continue;
                    }
                    dump = dump.ToLower();
                    if (_client.ElementWithAttributes("//*[@content-desc=\"Liked\"]", 1, dump, false))
                    {
                        return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Đã làm job đó trước.");
                    }
                    if (_client.ElementWithAttributes("//*[@content-desc=\"Like\"]", 1, dump, false))
                    {
                        isLink = true;
                        break;
                    }
                    _client.SwipeByPercent(56, 82, 56, 16, 1000);
                }
                if (!isLink)
                {
                    return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Không tìm được nút {job.Type}.");
                }
                job.Link = link;
                int second = SubdyHelper.RandomValue(_settingScriptAction.GetIntType("numericUpDown2", 5), _settingScriptAction.GetIntType("numericUpDown1", 10));
                await DelayMessageAsync(second, "Delay trước khi click tương tác", 2);
                if (_client.ElementWithAttributes("//*[@content-desc=\"Like\"]", 5, "", true))
                {
                    return new SubdyExtension(SubdyEnum.Success, $"Job: {job.ObjectId?.Substring(0, 3)}... success.");
                }
            }
            return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Không tìm được nút {job.Type}.");
        }
        private async Task<bool> GotoUrl(string url)
        {
            List<string> xpaths = new List<string>
            {
                "//*[contains(@text, \"go to news feed\")]",
                "//*[contains(@text, \"content not found\")]",
                "//*[@text=\"Page Not Found\"]",
                "//*[@text=\"Connection lost\"]",
                "//*[@text=\"The page you requested was not found.\"]",
                "//*[@content-desc=\"Go to profile\"]",
                "//*[@text=\"Sorry, this page isn't available.\"]",
                "//*[@text=\"The link you followed may be broken, or the page may have been removed. \"]",
            };
            if (url.Contains("posts"))
            {
                xpaths.AddRange(new string[]{
                "//*[@content-desc=\"Close\"]",
                "//*[@content-desc=\"Search\"]"});
            }
            else if (url.Contains("photo"))
            {
                xpaths.AddRange(new string[]{
                "//*[@content-desc=\"Back\"]",
                "//*[@content-desc=\"More\"]"});
            }
            else if (url.Contains("instagram://media") || url.Contains("instagram://user"))
            {
                xpaths.Add("//*[@resource-id=\"com.instagram.android:id/action_bar_button_back\"]");
            }

            if (_platform == PlatformModel.Facebook)
            {
                _client.ADB.Shell($"am start -n com.facebook.katana/com.facebook.katana.IntentUriHandler \"{url}\" -p com.facebook.katana");
            }
            else if (_platform == PlatformModel.Instagram)
            {
                _client.ADB.Shell($"am start -a android.intent.action.VIEW -d \"{url}\" -p com.instagram.android");
            }

            await Task.Delay(2000);
            var xpath = _client.FindElement("", xpaths, 5);
            switch (xpath)
            {
                case "//*[@text=\"Sorry, this page isn't available.\"]":
                case "//*[@text=\"The link you followed may be broken, or the page may have been removed. \"]":
                case "//*[contains(@text, \"go to news feed\")]":
                case "//*[contains(@text, \"content not found\")]":
                case "//*[@text=\"Page Not Found\"]":
                case "//*[@text=\"The page you requested was not found.\"]":
                case "//*[@content-desc=\"Go to profile\"]":
                    return false;
                case "//*[@content-desc=\"Close\"]":
                case "//*[@content-desc=\"Search\"]":
                    {
                        if (_client.ElementWithAttributes("//*[@content-desc=\"Close\"]", 1, click: false) && _client.ElementWithAttributes("//*[@content-desc=\"Search\"]", 1, click: false))
                        {
                            return true;
                        }
                        return false;
                    }
                case "//*[@content-desc=\"Back\"]":
                case "//*[@content-desc=\"More\"]":
                    {
                        if (_client.ElementWithAttributes("//*[@content-desc=\"Back\"]", 1, click: false) && _client.ElementWithAttributes("//*[@content-desc=\"More\"]", 1, click: false))
                        {
                            return true;
                        }
                        return false;
                    }
                case "//*[@resource-id=\"com.instagram.android:id/action_bar_button_back\"]":
                    {
                        return true;
                    }
                case "//*[@text=\"Connection lost\"]":
                    {
                        throw new SubdyExtension(SubdyEnum.Error, "Mất kết nối internet.");
                    }
            }
            return false;
        }
        private async Task<SubdyExtension> JobFollow(JobModel job)
        {
            if (_platform == PlatformModel.Facebook)
            {

            }
            else if (_platform == PlatformModel.Instagram)
            {
                string link = $"instagram://user?username={job.ObjectId}";
                if (!await GotoUrl(link))
                {
                    return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Không tồn tại bài viết.");
                }
                bool isLink = false;
                for (int i = 0; i < 10; i++)
                {
                    string dump = _client.GetXMLSource();
                    if (string.IsNullOrEmpty(dump))
                    {
                        continue;
                    }
                    dump = dump.ToLower();
                    if (_client.ElementWithAttributes("//*[@text=\"Requested\"]", 1, dump, false) || (_client.FindElements(1, "", "//*[@resource-id=\"com.instagram.android:id/profile_header_user_action_follow_button\"]").Any() && _client.FindElements(1, "", "//*[@resource-id=\"com.instagram.android:id/profile_header_user_action_follow_button\"]")[0].OuterXml.ToLower().Contains("following")))
                    {
                        return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Đã làm job đó trước.");
                    }
                    if (_client.ElementWithAttributes("//*[@text=\"Follow\"]", 1, dump, false))
                    {
                        isLink = true;
                        break;
                    }
                }
                if (!isLink)
                {
                    return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Không tìm được nút {job.Type}.");
                }
                job.Link = link;
                int second = SubdyHelper.RandomValue(_settingScriptAction.GetIntType("numericUpDown2", 5), _settingScriptAction.GetIntType("numericUpDown1", 10));
                await DelayMessageAsync(second, "Delay trước khi click tương tác", 2);
                if (_client.ElementWithAttributes("//*[@text=\"Follow\"]", 5, "", true))
                {
                    return new SubdyExtension(SubdyEnum.Success, $"Job: {job.ObjectId?.Substring(0, 3)}... success.");
                }
            }
            return new SubdyExtension(SubdyEnum.JobFail, $"Job: {job.ObjectId?.Substring(0, 3)}... fail. Không tìm được nút {job.Type}.");
        }
        private async Task<string> JobLikePage(JobModel job)
        {
            return "";
        }
        private async Task<string> JobGroup(JobModel job)
        {
            return "";
        }
        private async Task<string> JobShare(JobModel job)
        {
            return null;
        }
    }

}
