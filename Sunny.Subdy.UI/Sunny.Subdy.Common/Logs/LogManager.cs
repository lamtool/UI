using System.Text;

namespace Sunny.Subdy.Common.Logs
{
    public static class LogManager
    {
        private static readonly string BaseLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        private static void WriteLog(string nameLog, string message)
        {
            try
            {
                string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
                string logFolder = Path.Combine(BaseLogPath, dateFolder);
                Directory.CreateDirectory(logFolder);

                string logPath = Path.Combine(logFolder, nameLog + ".txt");

                using (var writer = new StreamWriter(logPath, true, Encoding.UTF8))
                {
                    writer.WriteLine($"{message}");
                }

                CleanupOldestFolderIfExceedsLimit();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Logging failed: " + ex.Message);
            }
        }

        private static void CleanupOldestFolderIfExceedsLimit()
        {
            try
            {
                var directories = new DirectoryInfo(BaseLogPath)
                    .GetDirectories()
                    .OrderBy(d => d.CreationTimeUtc)
                    .ToList();

                while (directories.Count > 8)
                {
                    var oldest = directories.First();
                    try
                    {
                        oldest.Delete(true);
                        directories.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete folder '{oldest.Name}': {ex.Message}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Cleanup failed: " + ex.Message);
            }
        }

        public static void Error(Exception exception)
        {
            if (exception == null) return;

            var sb = new StringBuilder();
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sb.AppendLine($"------------------ {timestamp} ----------------------------");

            int level = 0;
            while (exception != null)
            {
                sb.AppendLine($"Level {level}: {exception.GetType().FullName}");
                sb.AppendLine($"Message   : {exception.Message}");
                sb.AppendLine($"StackTrace: {exception.StackTrace}");
                sb.AppendLine(); // dòng trắng giữa các cấp
                exception = exception.InnerException;
                level++;
            }

            WriteLog("error", sb.ToString());
        }

        public static void Info(string message)
        {
            WriteLog("info", message);
        }

        public static void Debug(string message)
        {
            WriteLog("debug", message);
        }
    }
}
