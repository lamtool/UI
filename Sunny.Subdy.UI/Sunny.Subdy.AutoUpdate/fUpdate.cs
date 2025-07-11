using Sunny.UI;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Sunny.Subdy.AutoUpdate
{
    public partial class fUpdate : UIForm
    {
        private string _link;
        private string _zipPath;
        private string _updateFolder;
        private string _version;

        public fUpdate(string link, string version)
        {
            InitializeComponent();
            _link = link;
            _version = version;
            _updateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdateTemp");
            _zipPath = Path.Combine(_updateFolder, "update.zip");
            this.Load += FUpdate_Load;
        }

        private void FUpdate_Load(object? sender, EventArgs e)
        {
            _ = StartUpdateProcessAsync();
        }

      

        private async Task StartUpdateProcessAsync()
        {
            try
            {
                uiLabel2.Text = "Đang tải bản cập nhật...";
                Directory.CreateDirectory(_updateFolder);
                await DownloadFileAsync(_link, _zipPath);

                uiLabel2.Text = "Chuẩn bị cập nhật...";
                await Task.Delay(500);

                CreateUpdateBatAndRestart();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }
        int lastPercent = 0;
        Stopwatch sw = Stopwatch.StartNew();
        private async Task DownloadFileAsync(string url, string destinationPath)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? -1L;
            using var stream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

            byte[] buffer = new byte[8192];
            long totalRead = 0;
            int read;
            int lastPercent = 0;
            var sw = Stopwatch.StartNew();

            while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, read));
                totalRead += read;

                if (totalBytes > 0)
                {
                    int percent = (int)(totalRead * 100 / totalBytes);
                    if (percent != lastPercent && (percent % 1 == 0 || sw.ElapsedMilliseconds > 500))
                    {
                        sw.Restart();
                        lastPercent = percent;
                        uiProcessBar1.Invoke(() => uiProcessBar1.Value = percent);
                        uiLabel2.Invoke(() => uiLabel2.Text = $"Đang tải... {percent}%");
                    }
                }
            }

            uiProcessBar1.Invoke(() => uiProcessBar1.Value = 100);
        }

        private void CreateUpdateBatAndRestart()
        {
            string exePath = Application.ExecutablePath;
            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string exeName = Path.GetFileName(exePath);
            string renamedExe = Path.GetFileNameWithoutExtension(exeName) + "-" + _version + ".exe";
            string newExeName = exeName; // giữ nguyên tên file mới
            string batPath = Path.Combine(Path.GetTempPath(), $"update_{Guid.NewGuid():N}.bat");

            string batContent = $@"
@echo off
cd /d ""{folderPath}""
timeout /t 2 >nul

rem Đổi tên exe cũ
rename ""{exeName}"" ""{renamedExe}""

rem Giải nén update.zip
powershell -Command ""Expand-Archive -LiteralPath '{_zipPath}' -DestinationPath '{folderPath}' -Force""

rem Ghi log trước khi chạy
echo Running new exe >> %TEMP%\update_log.txt

rem Chạy app mới với quyền admin
powershell -Command ""Start-Process -FilePath '{newExeName}' -Verb RunAs""

rem Dọn dẹp
rd /s /q ""{_updateFolder}""
del ""%~f0""
";

            File.WriteAllText(batPath, batContent, Encoding.UTF8);

            Process.Start(new ProcessStartInfo
            {
                FileName = batPath,
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }
    }

}
