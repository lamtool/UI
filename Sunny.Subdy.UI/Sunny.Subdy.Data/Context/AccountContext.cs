using System.Data.SQLite;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subdy.Data.Context
{
    public class AccountContext
    {
        private readonly AppDbContext _db;
        private const string TableName = nameof(Account);

        public AccountContext()
        {
            _db = new AppDbContext("LT_Account");
            _db.EnsureTable<Account>();
        }

        private Account MapToAccount(SQLiteDataReader reader)
        {
            Guid id;

            try
            {
                // Luôn ép về string, tránh lỗi nếu data là byte[] do lưu sai kiểu
                var idStr = reader["Id"]?.ToString();

                id = Guid.TryParse(idStr, out var parsedGuid)
                    ? parsedGuid
                    : Guid.Empty;
            }
            catch
            {
                // Nếu vẫn lỗi (do BLOB hay dữ liệu hỏng), trả về Guid.Empty để không crash
                id = Guid.Empty;
            }

            return new Account
            {
                Checked = false,
                Id = id,
                Uid = reader["Uid"]?.ToString() ?? string.Empty,
                Password = reader["Password"]?.ToString() ?? string.Empty,
                Phone = reader["Phone"]?.ToString() ?? string.Empty,
                TowFA = reader["TowFA"]?.ToString() ?? string.Empty,
                Cookie = reader["Cookie"]?.ToString() ?? string.Empty,
                Token = reader["Token"]?.ToString() ?? string.Empty,
                Proxy = reader["Proxy"]?.ToString() ?? string.Empty,
                Email = reader["Email"]?.ToString() ?? string.Empty,
                PassMail = reader["PassMail"]?.ToString() ?? string.Empty,
                UserAgent = reader["UserAgent"]?.ToString() ?? string.Empty,
                FullName = reader["FullName"]?.ToString() ?? string.Empty,
                RecentInteraction = reader["RecentInteraction"]?.ToString() ?? string.Empty,
                State = reader["State"]?.ToString() ?? string.Empty,
                Status = reader["Status"]?.ToString() ?? string.Empty,
                Result = reader["Result"]?.ToString() ?? string.Empty,
                EmailAdress = reader["EmailAdress"]?.ToString() ?? string.Empty,
                UserName = reader["UserName"]?.ToString() ?? string.Empty,
                NameFolder = reader["NameFolder"]?.ToString(),
                IsView = reader["IsView"] != DBNull.Value && Convert.ToBoolean(reader["IsView"])
            };
        }

        public List<Account> GetAll(string query, Dictionary<string, object>? parameters = null)
        {
            return _db.GetAllEntities(query, MapToAccount, parameters);
        }

        public List<Account> GetAll(List<string> nameFolders, bool? isView = true)
        {
            var parameters = new Dictionary<string, object>();
            var whereClauses = new List<string>();

            if (isView.HasValue)
            {
                whereClauses.Add("IsView = @IsView");
                parameters.Add("@IsView", isView.Value ? 1 : 0);
            }

            if (nameFolders != null && nameFolders.Count > 0)
            {
                var folderParams = new List<string>();
                for (int i = 0; i < nameFolders.Count; i++)
                {
                    string paramName = $"@folder{i}";
                    folderParams.Add(paramName);
                    parameters.Add(paramName, nameFolders[i]);
                }
                whereClauses.Add($"NameFolder IN ({string.Join(", ", folderParams)})");
            }

            string query = $"SELECT * FROM {TableName}";
            if (whereClauses.Any())
            {
                query += " WHERE " + string.Join(" AND ", whereClauses);
            }

            return GetAll(query, parameters);
        }

        public Account? Get(Guid id)
        {
            string query = $"SELECT * FROM {TableName} WHERE Id = @id";
            var parameters = new Dictionary<string, object> { { "@id", id.ToString() } };
            return GetAll(query, parameters).FirstOrDefault();
        }

        public Account? GetByName(string uid)
        {
            string query = $"SELECT * FROM {TableName} WHERE Uid = @uid";
            var parameters = new Dictionary<string, object> { { "@uid", uid } };
            return GetAll(query, parameters).FirstOrDefault();
        }

        public bool AddRange(List<Account> accounts)
        {
            return _db.InsertEntities(accounts);
        }

        public bool Add(Account account)
        {
            return _db.InsertEntity(account);
        }

        public bool UpdateAccountsFolderName(string oldFolderName, string newFolderName)
        {
            string query = $"UPDATE {TableName} SET NameFolder = @newFolderName WHERE NameFolder = @oldFolderName";
            var parameters = new Dictionary<string, object>
        {
            { "@newFolderName", newFolderName },
            { "@oldFolderName", oldFolderName }
        };
            return _db.ExecuteNonQuery(query, parameters);
        }

        public bool Update(List<Account> accounts)
        {
            return _db.UpdateEntities(accounts);
        }

        public bool Update(Account account)
        {
            return _db.UpdateEntity(account);
        }

        public bool DeleteById(Guid id)
        {
            string query = $"DELETE FROM {TableName} WHERE Id = @id";
            var parameters = new Dictionary<string, object> { { "@id", id.ToString() } };
            return _db.ExecuteNonQuery(query, parameters);
        }

        public bool DeleteByIds(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return false;

            var parameters = new Dictionary<string, object>();
            var paramNames = new List<string>();
            for (int i = 0; i < ids.Count; i++)
            {
                string paramName = $"@id{i}";
                paramNames.Add(paramName);
                parameters.Add(paramName, ids[i].ToString());
            }

            string query = $"DELETE FROM {TableName} WHERE Id IN ({string.Join(", ", paramNames)})";
            return _db.ExecuteNonQuery(query, parameters);
        }
    }
}
