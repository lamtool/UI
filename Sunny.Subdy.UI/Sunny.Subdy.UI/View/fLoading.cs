using System.Reflection;
using DeviceId;
using Sunny.Subdy.AutoUpdate;
using Sunny.Subdy.AutoUpdate.Api;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.UI.Services;
using Sunny.UI;

namespace Sunny.Subdy.UI.View
{
    public partial class fLoading : UIForm2
    {
        public fMain MainForm { get; private set; }

        public fLoading()
        {
            InitializeComponent();

            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            uiRoundProcess1.BackColor = Color.Transparent;
        }

        private async void fLoading_Load(object sender, EventArgs e)
        {
            var loadingTask = RunLoadingAsync();

            MainForm = new fMain();
            await new BuildConfig().Build();
            await MainForm.LoadUI(); // thực hiện khởi tạo giao diện
            Globals.DeviceId = new DeviceIdBuilder().OnWindows(windows => windows.AddWindowsDeviceId()).ToString();
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
            MainForm.uiLabel7.Text = "v" + version;
            LamTool_API lamtool = new LamTool_API(Globals.DeviceId, Globals.NameApp, version);
            if (!await lamtool.GetApiResponseAsync())
            {
                MessageBox.Show("Đã xảy ra lỗi vui lòng liên hệ admin để được hỗ trợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            if (lamtool.IsNewerVersion())
            {
                if (MessageBox.Show($"Có phiên bản mới {lamtool._newVersion} bạn có muốn cập nhật không?", "Cập nhật", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    fUpdate updateForm = new fUpdate(lamtool._updateUrl);
                    updateForm.ShowDialog();
                    Application.Exit();
                    return;
                }
            }
            loadUIFinished = true;

            await loadingTask;
            this.DialogResult = DialogResult.OK;
            this.Close();
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
