using Sunny.Subdy.UI.Services;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View
{
    public partial class fLoading : UIForm2
    {
        public fLoading()
        {
            InitializeComponent();
        }

        private int loadingProgress = 0;
        private bool loadUIFinished = false;

        private async Task RunLoadingAsync()
        {
            int totalDuration = 30000; // 30 giây
            int maxProgress = 100;
            int updateInterval = 100; // ms
            int step = 1;

            int elapsed = 0;

            while (loadingProgress < maxProgress && elapsed < totalDuration)
            {
                if (loadUIFinished)
                {
                    // Khi LoadUI hoàn tất -> tăng nhanh trong 2s còn lại
                    int remaining = maxProgress - loadingProgress;
                    int fastDelay = 2000 / Math.Max(remaining, 1); // chia đều trong 2s
                    while (loadingProgress < maxProgress)
                    {
                        loadingProgress++;
                        uiRoundProcess1.Value = loadingProgress;
                        await Task.Delay(fastDelay);
                    }
                    return;
                }

                loadingProgress = Math.Min(loadingProgress + step, maxProgress);
                uiRoundProcess1.Value = loadingProgress;

                await Task.Delay(updateInterval);
                elapsed += updateInterval;
            }
        }

        private async void fLoading_Load(object sender, EventArgs e)
        {
            var loadingTask = RunLoadingAsync();

            var mainForm = new fMain();
            await new BuildConfig().Build(); // giả sử hàm này mất vài giây
            await mainForm.LoadUI(); // giả sử hàm này mất vài giây
            loadUIFinished = true;   // báo hiệu tiến trình load UI xong để tăng tốc

            await loadingTask;

            mainForm.Show();
            this.Hide();
        }
    }
}
