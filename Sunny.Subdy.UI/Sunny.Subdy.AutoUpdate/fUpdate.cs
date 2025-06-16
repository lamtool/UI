using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Sunny.UI;

namespace Sunny.Subdy.AutoUpdate
{
    public partial class fUpdate : UIForm
    {
        private string _link;
        private string _tempZipPath;
        private string _extractPath;

        public fUpdate(string link)
        {
            InitializeComponent();
            _link = link;
            _tempZipPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"update_{Guid.NewGuid()}.zip");
            _extractPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdateTemp");
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            uiLabel2.Text = "Đang tải bản cập nhật...";
            try
            {
                await DownloadFileAsync(_link, _tempZipPath);
                uiLabel2.Text = "Đang giải nén...";
                await ExtractZipAsync(_tempZipPath, _extractPath);
                uiLabel2.Text = "Hoàn tất cập nhật.";
                await Task.Delay(1000);
                CreateUpdateBatAndRestart();
                Application.Exit();
            }
            catch (Exception ex)
            {
                uiLabel2.Text = "Lỗi: " + ex.Message;
            }
            finally
            {
                if (File.Exists(_tempZipPath))
                    File.Delete(_tempZipPath);
            }
        }

        private async Task DownloadFileAsync(string url, string destinationPath)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var contentLength = response.Content.Headers.ContentLength ?? -1L;
            using var stream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);

            var buffer = new byte[8192];
            long totalRead = 0;
            int read;

            while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, read));
                totalRead += read;

                if (contentLength > 0)
                {
                    int percent = (int)(totalRead * 100 / contentLength);
                    uiProcessBar1.Value = percent;
                    uiLabel2.Text = $"Đang cập nhật phiên bản mới nhất. Vui lòng chờ... \n Đang tải... {percent}%";
                    await Task.Delay(10);
                }
            }

            uiProcessBar1.Value = 100;
        }

        private async Task ExtractZipAsync(string zipPath, string destinationPath)
        {
            if (Directory.Exists(destinationPath))
                Directory.Delete(destinationPath, true);

            Directory.CreateDirectory(destinationPath);

            using var archive = ZipFile.OpenRead(zipPath);
            int total = archive.Entries.Count;
            int current = 0;

            foreach (var entry in archive.Entries)
            {
                string fullPath = Path.Combine(destinationPath, entry.FullName);

                if (entry.FullName.EndsWith("/")) // thư mục
                {
                    Directory.CreateDirectory(fullPath);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
                    entry.ExtractToFile(fullPath, overwrite: true);
                }

                current++;
                int percent = (int)(current * 100.0 / total);
                uiProcessBar1.Value = percent;
                uiLabel2.Text = $"Đang giải nén... {percent}%";
                await Task.Delay(10);
            }

            uiProcessBar1.Value = 100;
        }

        private void CreateUpdateBatAndRestart()
        {
            string exePath = Application.ExecutablePath;
            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string updateFolder = Path.Combine(folderPath, "UpdateTemp");
            string batPath = Path.Combine(Path.GetTempPath(), $"update_{Guid.NewGuid():N}.bat");

            string batContent = $@"
@echo off
timeout /t 1 >nul
xcopy /y /e /q ""{updateFolder}\*"" ""{folderPath}""
start """" ""{exePath}""
rd /s /q ""{updateFolder}""
del ""%~f0""
";

            File.WriteAllText(batPath, batContent);

            Process.Start(new ProcessStartInfo
            {
                FileName = batPath,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            });
        }
    }
}
