using DeviceId;
using Sunny.Subdy.AutoUpdate;
using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.UI.ControlViews.Convertes;
using Sunny.Subdy.UI.Services;
using Sunny.UI;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;

namespace Sunny.Subdy.UI.View
{
    public partial class fLoading : UIForm2
    {
        public fMain MainForm { get; private set; }

        public fLoading()
        {
            InitializeComponent();
            this.Text = "LamToolAutoPhone";
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            uiRoundProcess1.BackColor = Color.Transparent;
        }
        private async Task fLoading_LoadSafe()
        {
            try
            {
                var loadingTask = RunLoadingAsync();

                MainForm = new fMain();
                await new BuildConfig().Build();
                await MainForm.LoadUI(); // thực hiện khởi tạo giao diện
                Globals.DeviceId = new DeviceIdBuilder().OnWindows(windows => windows.AddWindowsDeviceId())
                    .ToString();
                string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
                MainForm.uiLabel7.Text = "v" + version;
                var (ok, vs, url) = LamToolClient.GetApiResponseAsync(Globals.DeviceId, Globals.NameApp, version);
                if (!ok)
                {
                    MessageBox.Show("Đã xảy ra lỗi vui lòng liên hệ admin để được hỗ trợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                    return;
                }
                if (LamToolClient.IsNewerVersion(version, vs))
                {
                    if (MessageBox.Show($"Có phiên bản mới {vs} bạn có muốn cập nhật không?", "Cập nhật", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Hide();
                        fUpdate updateForm = new fUpdate(url, version);
                        updateForm.ShowDialog();
                        Environment.Exit(0);
                        return;
                    }
                }
                loadUIFinished = true;

                await loadingTask;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
                Environment.Exit(0);
            }
        }
        private void fLoading_Load(object sender, EventArgs e)
        {
            _ = fLoading_LoadSafe();
        }

        private int loadingProgress = 0;
        private bool loadUIFinished = false;

        private async Task RunLoadingAsync()
        {
            int totalDuration = 10000;
            int updateInterval = 100;

            int elapsed = 0;

            while (loadingProgress < 100 && elapsed < totalDuration)
            {
                if (loadUIFinished)
                {
                    while (loadingProgress < 100)
                    {
                        loadingProgress++;
                        uiRoundProcess1.Value = loadingProgress;
                        await Task.Delay(2000 / Math.Max(1, 100 - loadingProgress));
                    }
                    return;
                }

                loadingProgress = Math.Min(loadingProgress + 1, 100);
                uiRoundProcess1.Value = loadingProgress;

                await Task.Delay(updateInterval);
                elapsed += updateInterval;
            }
        }
    }

}
