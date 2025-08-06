using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sunny.Subdy.Data.Context
{
    public class JobHistoryContext
    {
        private readonly AppDbContext _db;
        private const string TableName = nameof(JobHistory);

        public JobHistoryContext()
        {
            _db = new AppDbContext("LT_JobHistory");
            _db.EnsureTable<JobHistory>();
            _db.ExecuteNonQuery($"CREATE INDEX IF NOT EXISTS idx_uid_date ON {TableName} (Uid, DateTime)");
        }

        public bool Add(JobHistory job)
        {
            job.DateTime = DateTime.Now.ToString("dd/MM/yyyy");
            return _db.InsertEntity(job);
        }

        public bool Update(JobHistory job)
        {
            return _db.UpdateEntity(job);
        }

        public bool Delete(string uid, string date)
        {
            string query = $"DELETE FROM {TableName} WHERE Uid = @uid AND DateTime = @date";
            var parameters = new Dictionary<string, object>
            {
                ["@uid"] = uid,
                ["@date"] = date
            };
            return _db.ExecuteNonQuery(query, parameters);
        }

        public List<JobHistory>? GetByUidAndDate(string uid, string date)
        {
            string query = $"SELECT * FROM {TableName} WHERE Uid = @uid AND DateTime = @date LIMIT 1";
            var parameters = new Dictionary<string, object>
            {
                ["@uid"] = uid,
                ["@date"] = date
            };
            return _db.GetAllEntities(query, MapToJob, parameters);
        }

        public JobHistory? GetByUidDateService(string uid, string date, string service)
        {
            string query = $@"
            SELECT * FROM {TableName}
            WHERE Uid = @uid AND DateTime = @date AND Service = @service
            LIMIT 1";
            var parameters = new Dictionary<string, object>
            {
                ["@uid"] = uid,
                ["@date"] = date,
                ["@service"] = service
            };
            return _db.GetAllEntities(query, MapToJob, parameters).FirstOrDefault();
        }

        public int CountByUidAndStatus(string uid, string status)
        {
            string query = $@"SELECT COUNT(*) FROM {TableName} WHERE Uid = @uid AND Status = @status";
            var parameters = new Dictionary<string, object>
            {
                ["@uid"] = uid,
                ["@status"] = status
            };

            object? result = _db.ExecuteScalar(query, parameters);
            return result != null && int.TryParse(result.ToString(), out int count) ? count : 0;
        }
        public int CountByUidAndStatusToday(string uid, string status)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            string query = $@"SELECT COUNT(*) FROM {TableName} WHERE Uid = @uid AND Status = @status AND DateTime = @date LIMIT 1";
            var parameters = new Dictionary<string, object>
            {
                ["@uid"] = uid,
                ["@status"] = status,
                ["@date"] = date
            };

            object? result = _db.ExecuteScalar(query, parameters);
            return result != null && int.TryParse(result.ToString(), out int count) ? count : 0;
        }
        private JobHistory MapToJob(SQLiteDataReader reader)
        {
            return new JobHistory
            {
                Id = Guid.TryParse(reader["Id"]?.ToString(), out var id) ? id : Guid.NewGuid(),
                Uid = reader["Uid"]?.ToString(),
                Platform = reader["Platform"]?.ToString(),
                Method = reader["Method"]?.ToString(),
                Description = reader["Description"]?.ToString(),
                Status = reader["Status"]?.ToString(),
                IdJob = reader["IdJob"]?.ToString(),
                IdObject = reader["IdObject"]?.ToString(),
                Coin = reader["Coin"]?.ToString(),
                Service = reader["Service"]?.ToString(),
                DateTime = reader["DateTime"]?.ToString()
            };
        }
        public Dictionary<string, string> GetHistorySummaryToDayByUid(string uid, string platform, string service)
        {
            var result = new Dictionary<string, string>
            {
                ["Total"] = "0",
                ["Success"] = "0",
                ["Fail"] = "0",
                ["XuSuccess"] = "0",
                ["XuFail"] = "0"
            };

            string dateStr = DateTime.Now.ToString("dd/MM/yyyy");

            string query = $@"
SELECT * FROM {TableName}
WHERE Uid = @uid AND Platform = @platform AND Service = @service AND DateTime = @date";

            var parameters = new Dictionary<string, object>
            {
                ["@uid"] = uid,
                ["@platform"] = platform,
                ["@service"] = service,
                ["@date"] = dateStr
            };

            var histories = _db.GetAllEntities(query, MapToJob, parameters);

            int successCount = 0, failCount = 0;
            int successCoin = 0, failCoin = 0;
            var methodCount_Success = new Dictionary<string, int>();
            var methodXu_Success = new Dictionary<string, int>();
            var methodCount_Fail = new Dictionary<string, int>();
            var methodXu_Fail = new Dictionary<string, int>();

            foreach (var history in histories)
            {
                bool isSuccess = history.Status == "Success";
                int coin = int.TryParse(history.Coin, out var c) ? c : 0;

                var method = history.Method?.Trim();
                if (string.IsNullOrEmpty(method)) continue;

                if (isSuccess)
                {
                    successCount++;
                    successCoin += coin;

                    methodCount_Success.TryAdd(method, 0);
                    methodXu_Success.TryAdd(method, 0);

                    methodCount_Success[method]++;
                    methodXu_Success[method] += coin;
                }
                else
                {
                    failCount++;
                    failCoin += coin;

                    methodCount_Fail.TryAdd(method, 0);
                    methodXu_Fail.TryAdd(method, 0);

                    methodCount_Fail[method]++;
                    methodXu_Fail[method] += coin;
                }
            }

            result["Total"] = histories.Count.ToMoneyString();
            result["Success"] = successCount.ToMoneyString();
            result["Fail"] = failCount.ToMoneyString();
            result["XuSuccess"] = successCoin.ToMoneyString();
            result["XuFail"] = failCoin.ToMoneyString();

            foreach (var method in methodCount_Success)
            {
                var key = method.Key;
                var count = method.Value;
                var xu = methodXu_Success[key];
                result[key] = $"{count.ToMoneyString()}/{xu.ToMoneyString()}";
            }

            foreach (var method in methodCount_Fail)
            {
                var key = method.Key;
                var count = method.Value;
                var xu = methodXu_Fail[key];
                result[$"{key}_Skip"] = $"{count.ToMoneyString()}/{xu.ToMoneyString()}";
            }

            return result;
        }
        public Dictionary<string, string> GetHistorySummaryToDay(string platform)
        {
            var result = new Dictionary<string, string>
            {
                ["Job Total"] = "0/0|0/0",
            };

            string dateStr = DateTime.Now.ToString("dd/MM/yyyy");

            string query = $@"
SELECT * FROM {TableName}
WHERE Platform = @platform AND DateTime = @date";

            var parameters = new Dictionary<string, object>
            {
                ["@platform"] = platform,
                ["@date"] = dateStr
            };

            var histories = _db.GetAllEntities(query, MapToJob, parameters);

            int successCount = 0, failCount = 0;
            int successCoin = 0, failCoin = 0;
            var methodCount_Success = new Dictionary<string, int>();
            var methodXu_Success = new Dictionary<string, int>();
            var methodCount_Fail = new Dictionary<string, int>();
            var methodXu_Fail = new Dictionary<string, int>();

            foreach (var history in histories)
            {
                bool isSuccess = history.Status == "Success";
                int coin = int.TryParse(history.Coin, out var c) ? c : 0;

                var method = history.Method?.Trim();
                if (string.IsNullOrEmpty(method)) continue;

                if (isSuccess)
                {
                    successCount++;
                    successCoin += coin;

                    methodCount_Success.TryAdd(method, 0);
                    methodXu_Success.TryAdd(method, 0);

                    methodCount_Success[method]++;
                    methodXu_Success[method] += coin;
                }
                else
                {
                    failCount++;
                    failCoin += coin;

                    methodCount_Fail.TryAdd(method, 0);
                    methodXu_Fail.TryAdd(method, 0);

                    methodCount_Fail[method]++;
                    methodXu_Fail[method] += coin;
                }
            }

            result["Job Total"] = $"{successCount.ToMoneyString()}/{failCount.ToMoneyString()}|{successCoin.ToMoneyString()}/{failCoin.ToMoneyString()}";

            foreach (var method in methodCount_Success)
            {
                try
                {
                    var key = method.Key;

                    var countSuccess = method.Value;
                    var xuSuccess = methodXu_Success.TryGetValue(key, out var sXu) ? sXu : 0;
                    var countFail = methodCount_Fail.TryGetValue(key, out var fCount) ? fCount : 0;
                    var xuFail = methodXu_Fail.TryGetValue(key, out var fXu) ? fXu : 0;

                    result[key] = $"{countSuccess.ToMoneyString()}/{countFail.ToMoneyString()}|{xuSuccess.ToMoneyString()}/{xuFail.ToMoneyString()}";
                }
                catch (Exception e)
                {

                }

              
            }

            return result;
        }
        public int GetCountSuccessByUid(string uid)
        {
            string status = "Success";
            string query = $@"
SELECT * FROM {TableName}
WHERE Uid = @uid AND Status = @status";

            var parameters = new Dictionary<string, object>
            {
                ["@uid"] = uid,
                ["@status"] = status,
            };
            int count = 0;
            try
            {
                count = _db.GetAllEntities(query, MapToJob, parameters).Count;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }

            return count;
        }
    }
}
