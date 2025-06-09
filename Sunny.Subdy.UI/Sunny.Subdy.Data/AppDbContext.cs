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

        public void EnsureTable<T>()
        {
            try
            {
                var type = typeof(T);
                var tableName = type.Name;
                var properties = type.GetProperties();

                using var conn = GetConnection();

                // 1. Lấy các cột hiện tại trong bảng
                var existingColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName})", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string colName = reader["name"].ToString();
                        existingColumns.Add(colName);
                    }
                }

                // 2. Nếu bảng chưa có (chưa có cột nào), thì tạo mới hoàn toàn
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

                    sb.Length -= 3; // remove last comma
                    sb.AppendLine(");");

                    using var createCmd = new SQLiteCommand(sb.ToString(), conn);
                    createCmd.ExecuteNonQuery();
                }
                else
                {
                    // 3. Nếu bảng đã có, thì kiểm tra từng thuộc tính xem có bị thiếu không
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

        private string? MapTypeToSqlite(Type type)
        {
            if (type == typeof(int) || type == typeof(long)) return "INTEGER";
            if (type == typeof(string)) return "TEXT";
            if (type == typeof(double) || type == typeof(float)) return "REAL";
            if (type == typeof(bool)) return "INTEGER";
            if (type == typeof(DateTime)) return "TEXT";
            return null;
        }

        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                using var conn = GetConnection();
                using var cmd = new SQLiteCommand(sql, conn);
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return false;
        }
        public bool ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
        {
            try
            {
                using var conn = GetConnection();
                using var cmd = new SQLiteCommand(sql, conn);

                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return false;

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
            }
            return null;
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
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    T entity = map(reader);
                    resultList.Add(entity);
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
                if (entity == null)
                    return false;

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
                        string columnName = prop.Name;
                        string paramName = $"@{columnName}";

                        columnNames.Add(columnName);
                        paramNames.Add(paramName);
                        parameters.Add(new SQLiteParameter(paramName, value));
                    }
                }

                string sql = $"INSERT INTO {tableName} ({string.Join(",", columnNames)}) VALUES ({string.Join(",", paramNames)});";

                using var conn = GetConnection();
                using var cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddRange(parameters.ToArray());
                int rows = cmd.ExecuteNonQuery();

                return rows > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return false;
        }
        public bool InsertEntities<T>(List<T> entities)
        {
            try
            {
                if (entities == null || entities.Count == 0)
                    return false;

                var type = typeof(T);
                var tableName = type.Name;
                var properties = type.GetProperties().Where(p => p.CanRead).ToList();

                using var conn = GetConnection();
                using var transaction = conn.BeginTransaction();

                var columnNames = properties.Select(p => p.Name).ToList();
                var paramNames = columnNames.Select(name => $"@{name}").ToList();

                string sql = $"INSERT INTO {tableName} ({string.Join(",", columnNames)}) VALUES ({string.Join(",", paramNames)});";
                using var cmd = new SQLiteCommand(sql, conn, transaction);

                foreach (var name in columnNames)
                    cmd.Parameters.Add(new SQLiteParameter($"@{name}"));

                int insertedCount = 0;
                foreach (var entity in entities)
                {
                    for (int i = 0; i < properties.Count; i++)
                    {
                        object? value = properties[i].GetValue(entity);
                        cmd.Parameters[i].Value = value ?? DBNull.Value;
                    }
                    insertedCount += cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return insertedCount == entities.Count;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return false;

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
                try
                {
                    foreach (var entity in entities)
                    {
                        var setClauses = new List<string>();
                        var cmd = new SQLiteCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;

                        foreach (var prop in properties)
                        {
                            if (prop == keyProp) continue;

                            var value = prop.GetValue(entity) ?? DBNull.Value;
                            string paramName = $"@{prop.Name}";
                            setClauses.Add($"{prop.Name} = {paramName}");
                            cmd.Parameters.AddWithValue(paramName, value);
                        }

                        // Khóa chính
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
                    Console.WriteLine($"[UpdateEntities] Error: {ex.Message}");
                    transaction.Rollback();
                    throw ex;
                }
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

                var setClauses = new List<string>();
                var cmd = new SQLiteCommand();

                foreach (var prop in properties)
                {
                    if (prop == keyProp) continue;

                    var value = prop.GetValue(entity) ?? DBNull.Value;
                    string paramName = $"@{prop.Name}";
                    setClauses.Add($"{prop.Name} = {paramName}");
                    cmd.Parameters.AddWithValue(paramName, value);
                }

                // Thêm điều kiện WHERE theo khóa chính
                var keyValue = keyProp.GetValue(entity);
                cmd.Parameters.AddWithValue("@Id", keyValue);

                string sql = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE {keyProp.Name} = @Id;";
                cmd.CommandText = sql;

                using var conn = GetConnection();
                cmd.Connection = conn;

                int affected = cmd.ExecuteNonQuery();
                return affected > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            return false;

        }
    }
}
