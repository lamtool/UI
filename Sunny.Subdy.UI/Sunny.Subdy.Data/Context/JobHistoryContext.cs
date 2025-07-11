using Sunny.Subdy.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            EnsureIndex();
        }

        private void EnsureIndex()
        {
            string query = $"CREATE INDEX IF NOT EXISTS idx_uid_date ON {TableName} (Uid, DateTime)";
            _db.ExecuteNonQuery(query);
        }

        private JobHistory MapToJob(SQLiteDataReader reader)
        {
            return new JobHistory
            {
                Id = Guid.TryParse(reader["Id"]?.ToString(), out var id) ? id : Guid.NewGuid(),
                Uid = reader["Uid"]?.ToString(),

                Like = reader["Like"] as int? ?? 0,
                Love = reader["Love"] as int? ?? 0,
                Care = reader["Care"] as int? ?? 0,
                Wow = reader["Wow"] as int? ?? 0,
                Haha = reader["Haha"] as int? ?? 0,
                Angry = reader["Angry"] as int? ?? 0,
                Sad = reader["Sad"] as int? ?? 0,
                Share = reader["Share"] as int? ?? 0,
                Follow = reader["Follow"] as int? ?? 0,
                LikePage = reader["LikePage"] as int? ?? 0,
                JoinGroup = reader["JoinGroup"] as int? ?? 0,
                LikeComment = reader["LikeComment"] as int? ?? 0,
                Total = reader["Total"] as int? ?? 0,

                Like_Xu = reader["Like_Xu"] as int? ?? 0,
                Love_Xu = reader["Love_Xu"] as int? ?? 0,
                Care_Xu = reader["Care_Xu"] as int? ?? 0,
                Wow_Xu = reader["Wow_Xu"] as int? ?? 0,
                Haha_Xu = reader["Haha_Xu"] as int? ?? 0,
                Angry_Xu = reader["Angry_Xu"] as int? ?? 0,
                Sad_Xu = reader["Sad_Xu"] as int? ?? 0,
                Share_Xu = reader["Share_Xu"] as int? ?? 0,
                Follow_Xu = reader["Follow_Xu"] as int? ?? 0,
                LikePage_Xu = reader["LikePage_Xu"] as int? ?? 0,
                JoinGroup_Xu = reader["JoinGroup_Xu"] as int? ?? 0,
                LikeComment_Xu = reader["LikeComment_Xu"] as int? ?? 0,
                Total_Xu = reader["Total_Xu"] as int? ?? 0,

                Like_Skip = reader["Like_Skip"] as int? ?? 0,
                Love_Skip = reader["Love_Skip"] as int? ?? 0,
                Care_Skip = reader["Care_Skip"] as int? ?? 0,
                Wow_Skip = reader["Wow_Skip"] as int? ?? 0,
                Haha_Skip = reader["Haha_Skip"] as int? ?? 0,
                Angry_Skip = reader["Angry_Skip"] as int? ?? 0,
                Sad_Skip = reader["Sad_Skip"] as int? ?? 0,
                Share_Skip = reader["Share_Skip"] as int? ?? 0,
                Follow_Skip = reader["Follow_Skip"] as int? ?? 0,
                LikePage_Skip = reader["LikePage_Skip"] as int? ?? 0,
                JoinGroup_Skip = reader["JoinGroup_Skip"] as int? ?? 0,
                LikeComment_Skip = reader["LikeComment_Skip"] as int? ?? 0,
                Total_Skip = reader["Total_Skip"] as int? ?? 0,

                DateTime = reader["DateTime"]?.ToString()
            };
        }

        public List<JobHistory> GetAll(string query, Dictionary<string, object>? parameters = null)
        {
            return _db.GetAllEntities(query, MapToJob, parameters);
        }

        public JobHistory? GetByUid(string uid, string date)
        {
            string query = $"SELECT * FROM {TableName} WHERE Uid = @uid AND DateTime = @date LIMIT 1";
            var parameters = new Dictionary<string, object>
        {
            { "@uid", uid },
            { "@date", date }
        };
            return GetAll(query, parameters).FirstOrDefault();
        }

        public Dictionary<string, JobHistory> GetByUidsAndDate(List<string> uids, string date)
        {
            if (uids == null || uids.Count == 0 || string.IsNullOrWhiteSpace(date))
                return new();

            var parameters = new Dictionary<string, object> { { "@date", date } };
            var placeholders = new List<string>();

            for (int i = 0; i < uids.Count; i++)
            {
                var key = $"@uid{i}";
                placeholders.Add(key);
                parameters[key] = uids[i];
            }

            string query = $@"
            SELECT * FROM {TableName}
            WHERE Uid IN ({string.Join(", ", placeholders)})
            AND DateTime = @date
        ";

            return GetAll(query, parameters).ToDictionary(j => j.Uid ?? "", StringComparer.OrdinalIgnoreCase);
        }

        public bool Add(JobHistory job)
        {
            return _db.InsertEntity(job);
        }

        public bool AddRange(List<JobHistory> jobs)
        {
            return _db.InsertEntities(jobs);
        }

        public bool Update(JobHistory job)
        {
            return _db.UpdateEntity(job);
        }

        public bool UpdateRange(List<JobHistory> jobs)
        {
            return _db.UpdateEntities(jobs);
        }

        public bool DeleteByUidAndDate(string uid, string date)
        {
            string query = $"DELETE FROM {TableName} WHERE Uid = @uid AND DateTime = @date";
            var parameters = new Dictionary<string, object>
        {
            { "@uid", uid },
            { "@date", date }
        };
            return _db.ExecuteNonQuery(query, parameters);
        }

        public bool DeleteByUidsAndDate(List<string> uids, string date)
        {
            if (uids == null || uids.Count == 0) return false;

            var parameters = new Dictionary<string, object> { { "@date", date } };
            var placeholders = new List<string>();

            for (int i = 0; i < uids.Count; i++)
            {
                string param = $"@uid{i}";
                placeholders.Add(param);
                parameters[param] = uids[i];
            }

            string query = $"DELETE FROM {TableName} WHERE Uid IN ({string.Join(", ", placeholders)}) AND DateTime = @date";
            return _db.ExecuteNonQuery(query, parameters);
        }

        public JobHistory? GetTodayByUid(string uid)
        {
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            return GetByUid(uid, today);
        }

        public bool AddOrUpdateToday(JobHistory job)
        {
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            job.DateTime = today;

            var existing = GetByUid(job.Uid!, today);
            if (existing != null)
            {
                job.Id = existing.Id;
                return Update(job);
            }
            else
            {
                return Add(job);
            }
        }
        public Dictionary<string, JobStatistic> GetJobStatisticsSummary(string today)
        {
            string queryBase = $@"
SELECT
    SUM(Like) AS LikeTotal, SUM(Love) AS LoveTotal, SUM(Care) AS CareTotal,
    SUM(Haha) AS HahaTotal, SUM(Wow) AS WowTotal, SUM(Sad) AS SadTotal,
    SUM(Angry) AS AngryTotal, SUM(Share) AS ShareTotal, SUM(Follow) AS FollowTotal,
    SUM(LikePage) AS LikePageTotal, SUM(JoinGroup) AS JoinGroupTotal, SUM(LikeComment) AS LikeCommentTotal,
    SUM(Total) AS JobTotal,

    SUM(Like_Xu) AS LikeXuTotal, SUM(Love_Xu) AS LoveXuTotal, SUM(Care_Xu) AS CareXuTotal,
    SUM(Haha_Xu) AS HahaXuTotal, SUM(Wow_Xu) AS WowXuTotal, SUM(Sad_Xu) AS SadXuTotal,
    SUM(Angry_Xu) AS AngryXuTotal, SUM(Share_Xu) AS ShareXuTotal, SUM(Follow_Xu) AS FollowXuTotal,
    SUM(LikePage_Xu) AS LikePageXuTotal, SUM(JoinGroup_Xu) AS JoinGroupXuTotal, SUM(LikeComment_Xu) AS LikeCommentXuTotal,
    SUM(Total_Xu) AS XuTotal
FROM {TableName}";

            string queryToday = queryBase + " WHERE DateTime = @today";
            var allParams = new Dictionary<string, object> { { "@today", today } };

            Dictionary<string, object?> total = ReadFirstRow(queryBase);
            Dictionary<string, object?> todayStats = ReadFirstRow(queryToday, allParams);

            var result = new Dictionary<string, JobStatistic>();

            double GetValue(Dictionary<string, object?> dict, string column)
            {
                if (dict.TryGetValue(column, out var value) && value != null && value != DBNull.Value)
                {
                    if (double.TryParse(value.ToString(), out double result))
                        return result;
                }
                return 0;
            }

            void Add(string name)
            {
                double totalJob = GetValue(total, $"{name}Total");
                double totalXu = GetValue(total, $"{name}XuTotal");
                double todayJob = GetValue(todayStats, $"{name}Total");
                double todayXu = GetValue(todayStats, $"{name}XuTotal");

                result[name] = new JobStatistic
                {
                    JobTotal = totalJob,
                    XuTotal = totalXu,
                    JobToday = todayJob,
                    XuToday = todayXu
                };
            }

            string[] names = {
        "Like", "Love", "Care", "Haha", "Wow", "Sad", "Angry",
        "Share", "Follow", "LikePage", "JoinGroup", "LikeComment", "Job"
    };

            foreach (var name in names)
                Add(name);

            return result;
        }
        private Dictionary<string, object?> ReadFirstRow(string sql, Dictionary<string, object>? parameters = null)
        {
            return _db.ExecuteReader(sql, reader =>
            {
                var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    object? value = reader.GetValue(i);
                    row[name] = value;
                }
                return row;
            }, parameters ?? new()).FirstOrDefault() ?? new();
        }
    }
    public class JobStatistic
    {
        public double JobToday { get; set; }     // Số job hôm nay
        public double JobTotal { get; set; }     // Tổng số job
        public double XuToday { get; set; }      // Số xu hôm nay
        public double XuTotal { get; set; }      // Tổng số xu
    }
}
