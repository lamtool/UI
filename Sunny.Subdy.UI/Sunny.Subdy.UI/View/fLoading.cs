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
            loadUIFinished = true;

            await loadingTask;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private int loadingProgress = 0;
        private bool loadUIFinished = false;

        private async Task RunLoadingAsync()
        {
            int totalDuration = 30000;
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
