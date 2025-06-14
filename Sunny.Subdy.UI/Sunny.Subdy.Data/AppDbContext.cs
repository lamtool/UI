using Sunny.Subdy.Common.Logs;
using System.Data.SQLite;
using System.Reflection;
using System.Text;

namespace Sunny.Subdy.Data
{
    public class AppDbContext
    {
        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class SqlKeyAttribute : Attribute { }

        private readonly string _connectionString;
        private readonly string _dbPath;

        public AppDbContext(string databaseName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dataDir = Path.Combine(baseDir, "data");
            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            _dbPath = Path.Combine(dataDir, $"{databaseName}.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";
            EnsureDatabaseFile();
        }

        private void EnsureDatabaseFile()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }
        }

        private string? MapTypeToSqlite(Type type)
        {
            if (type == typeof(int) || type == typeof(long)) return "INTEGER";
            if (type == typeof(string)) return "TEXT";
            if (type == typeof(double) || type == typeof(float)) return "REAL";
            if (type == typeof(bool)) return "INTEGER";
            if (type == typeof(DateTime)) return "TEXT";
            if (type == typeof(Guid)) return "TEXT"; // Thêm dòng này
            return null;
        }

        public SQLiteConnection GetConnection()
        {
            try
            {
                var conn = new SQLiteConnection(_connectionString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                throw;
            }
        }

        public void EnsureTable<T>()
        {
            try
            {
                var type = typeof(T);
                var tableName = type.Name;
                var properties = type.GetProperties();

                using var conn = GetConnection();

                var existingColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName})", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingColumns.Add(reader["name"].ToString());
                    }
                }

                if (existingColumns.Count == 0)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"CREATE TABLE IF NOT EXISTS {tableName} (");

                    foreach (var prop in properties)
                    {
                        string name = prop.Name;
                        string typeStr = MapTypeToSqlite(prop.PropertyType);
                        if (typeStr == null) continue;

                        bool isKey = prop.GetCustomAttribute<SqlKeyAttribute>() != null;
                        sb.AppendLine($"  {name} {typeStr}{(isKey ? " PRIMARY KEY" : "")},");
                    }

                    sb.Length -= 3;
                    sb.AppendLine(");");

                    using var createCmd = new SQLiteCommand(sb.ToString(), conn);
                    createCmd.ExecuteNonQuery();
                }
                else
                {
                    foreach (var prop in properties)
                    {
                        string name = prop.Name;
                        string typeStr = MapTypeToSqlite(prop.PropertyType);
                        if (typeStr == null) continue;

                        if (!existingColumns.Contains(name))
                        {
                            string alterSql = $"ALTER TABLE {tableName} ADD COLUMN {name} {typeStr};";
                            using var alterCmd = new SQLiteCommand(alterSql, conn);
                            alterCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                using var conn = GetConnection();
                using var cmd = new SQLiteCommand(sql, conn);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        public bool ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
        {
            try
            {
                using var conn = GetConnection();
                using var cmd = new SQLiteCommand(sql, conn);
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        public List<T> GetAllEntities<T>(string query, Func<SQLiteDataReader, T> map, Dictionary<string, object>? parameters = null)
        {
            var resultList = new List<T>();
            try
            {
                using var connection = GetConnection();
                using var command = new SQLiteCommand(query, connection);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    resultList.Add(map(reader));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return resultList;
        }

        public bool InsertEntity<T>(T entity)
        {
            try
            {
                if (entity == null) return false;

                var type = typeof(T);
                var tableName = type.Name;
                var properties = type.GetProperties()
                    .Where(p => p.GetValue(entity) != null)
                    .ToList();

                var columnNames = new List<string>();
                var paramNames = new List<string>();
                var parameters = new List<SQLiteParameter>();

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(entity);
                    if (value != null)
                    {
                        string name = prop.Name;
                        string param = $"@{name}";
                        columnNames.Add(name);
                        paramNames.Add(param);

                        // Ép kiểu nếu là Guid
                        object dbValue = value is Guid guid ? guid.ToString() : value;

                        parameters.Add(new SQLiteParameter(param, dbValue));
                    }
                }

                string sql = $"INSERT INTO {tableName} ({string.Join(",", columnNames)}) VALUES ({string.Join(",", paramNames)});";

                using var conn = GetConnection();
                using var cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddRange(parameters.ToArray());
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        public bool InsertEntities<T>(List<T> entities)
        {
            try
            {
                if (entities == null || entities.Count == 0) return false;

                var type = typeof(T);
                var tableName = type.Name;
                var props = type.GetProperties();

                using var conn = GetConnection();
                using var transaction = conn.BeginTransaction();

                foreach (var entity in entities)
                {
                    if (entity == null) continue;

                    var columnNames = new List<string>();
                    var paramNames = new List<string>();
                    var parameters = new List<SQLiteParameter>();

                    foreach (var prop in props)
                    {
                        var value = prop.GetValue(entity);

                        // Nếu là Guid và là Id thì tự tạo mới nếu rỗng
                        if (prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) && prop.PropertyType == typeof(Guid))
                        {
                            var guid = (Guid)value;
                            if (guid == Guid.Empty)
                            {
                                guid = Guid.NewGuid();
                                prop.SetValue(entity, guid);
                            }
                            value = guid.ToString();
                        }
                        else if (value is Guid g)
                        {
                            value = g.ToString();
                        }

                        // Bỏ qua nếu null
                        if (value == null) continue;

                        string name = prop.Name;
                        string param = $"@{name}";
                        columnNames.Add(name);
                        paramNames.Add(param);
                        parameters.Add(new SQLiteParameter(param, value));
                    }

                    if (columnNames.Count == 0) continue;

                    string sql = $"INSERT INTO {tableName} ({string.Join(",", columnNames)}) VALUES ({string.Join(",", paramNames)});";

                    using var cmd = new SQLiteCommand(sql, conn, transaction);
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }


        public bool UpdateEntity<T>(T entity)
        {
            try
            {
                if (entity == null) return false;

                var type = typeof(T);
                var tableName = type.Name;
                var properties = type.GetProperties().Where(p => p.CanRead).ToList();
                var keyProp = properties.FirstOrDefault(p => p.GetCustomAttribute<SqlKeyAttribute>() != null);

                if (keyProp == null)
                    throw new InvalidOperationException("Không tìm thấy thuộc tính khóa chính (SqlKeyAttribute).");

                var cmd = new SQLiteCommand();
                var setClauses = new List<string>();

                foreach (var prop in properties)
                {
                    if (prop == keyProp) continue;

                    var value = prop.GetValue(entity);
                    if (value is Guid guidVal) value = guidVal.ToString(); // Ép Guid về string

                    var paramName = $"@{prop.Name}";
                    setClauses.Add($"{prop.Name} = {paramName}");
                    cmd.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
                }

                var keyValue = keyProp.GetValue(entity);
                if (keyValue is Guid keyGuid) keyValue = keyGuid.ToString(); // Ép khóa chính Guid về string

                cmd.Parameters.AddWithValue("@Id", keyValue);

                string sql = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE {keyProp.Name} = @Id;";
                cmd.CommandText = sql;

                using var conn = GetConnection();
                cmd.Connection = conn;

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }

        public bool UpdateEntities<T>(List<T> entities)
        {
            try
            {
                if (entities == null || entities.Count == 0)
                    return false;

                var type = typeof(T);
                var tableName = type.Name;
                var properties = type.GetProperties().Where(p => p.CanRead).ToList();
                var keyProp = properties.FirstOrDefault(p => p.GetCustomAttribute<SqlKeyAttribute>() != null);
                if (keyProp == null)
                    throw new InvalidOperationException("Không tìm thấy khóa chính (SqlKeyAttribute).");

                using var conn = GetConnection();
                using var transaction = conn.BeginTransaction();

                foreach (var entity in entities)
                {
                    var setClauses = new List<string>();
                    var cmd = new SQLiteCommand { Connection = conn, Transaction = transaction };

                    foreach (var prop in properties)
                    {
                        if (prop == keyProp) continue;

                        var value = prop.GetValue(entity) ?? DBNull.Value;
                        string paramName = $"@{prop.Name}";
                        setClauses.Add($"{prop.Name} = {paramName}");
                        cmd.Parameters.AddWithValue(paramName, value);
                    }

                    var keyValue = keyProp.GetValue(entity);
                    cmd.Parameters.AddWithValue("@Id", keyValue);

                    string sql = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE {keyProp.Name} = @Id;";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return false;
            }
        }
    }
}
