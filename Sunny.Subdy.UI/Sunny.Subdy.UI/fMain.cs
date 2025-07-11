using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.UI.Commons;
using Sunny.Subdy.UI.Helper;
using Sunny.Subdy.UI.View.Forms;
using Sunny.Subdy.UI.View.Pages;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunny.Subdy.UI
{
    public partial class fMain : UIForm2
    {
        public static DateTime? StartTime = null;
        private pageFacebook _pageFacebook;
        private pageDevice _pagePhone;
        public fMain()
        {
            InitializeComponent();
            this.Text = "LamToolAutoPhone";
            xuToolStripMenuItem.Text = "Xu: " + Globals.User.Balance.ToMoneyString();
            uiHeaderButton1.TipsText = "0";
        }
        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        public async Task LoadUI()
        {
            new DragHandler(popupMessageBox, this);
            new DragHandler(uiLabel1, this);
            new DragHandler(uiPanel4, this);
            int pageIndex = 0;
            popupBoxControl1.RegisterButtonEvents(
          BtnMinimize_Click,
          BtnMaximize_Click,
          BtnClose_Click
      );
            this.MainTabControl = uiTabControl1;
            uiNavMenu1.TabControl = uiTabControl1;
            uiNavMenu1.CreateNode(AddPage(new fTest(), ++pageIndex));
            TreeNode parent = uiNavMenu1.CreateNode("Ứng dụng", 57512, 24, ++pageIndex);
            _pagePhone = new pageDevice();
            _pageFacebook = new pageFacebook(uiNavMenu1, _pagePhone);
            uiNavMenu1.CreateChildNode(parent, AddPage(_pageFacebook, ++pageIndex));
            uiNavMenu1.CreateChildNode(parent, AddPage(new pageTikTok(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(_pagePhone, ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageGroupAccount(_pageFacebook), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageScript(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageApi(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageSetting(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageSupport(), ++pageIndex));
            uiNavMenu1.MenuItemClick += UiNavMenu1_MenuItemClick;
            uiNavMenu1.SelectPage(5);
            timer1.Start();
            Globals.DataGridView = _pageFacebook._ucdgvAccount.uiDataGridView2;
        }
        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (CommonMethod.ShowConfirmWarning("Bạn có chắc muốn đóng phần mềm?"))
            {
                Environment.Exit(0);
            }
        }
        private void UiNavMenu1_MenuItemClick(TreeNode node, Sunny.UI.NavMenuItem item, int index)
        {
            uiLabel4.Text = item.Text;
        }

        private void uiImageButton1_Click(object sender, EventArgs e)
        {
            if (uiPanel1.Size.Width == 55)
            {
                uiPanel1.Size = new Size(270, this.Size.Height);
            }
            else
            {
                uiPanel1.Size = new Size(55, this.Size.Height);
            }

        }

        private void uiPanel6_Click(object sender, EventArgs e)
        {

        }
        private DateTime? _lastCheckUpdateTime = null;
        private void timer1_Tick(object sender, EventArgs e)
        {
            string cpu = $"{SystemUsageMonitor.GetCpuUsage():0.00}%";
            string ram = $"{SystemUsageMonitor.GetRamUsage():0.00}%";

            uiLabel6.Text = ram;

            uiLabel5.Text = cpu;
            if (StartTime.HasValue)
            {
                TimeSpan elapsedTime = DateTime.Now - StartTime.Value;
                toolStripLabel5.Text = elapsedTime.ToString(@"hh\:mm\:ss");
            }
            if (_lastCheckUpdateTime == null || (DateTime.Now - _lastCheckUpdateTime.Value).TotalMinutes >= 30)
            {
                CheckUpdateVersion();
                _lastCheckUpdateTime = DateTime.Now;
            }
        }
        private void CheckUpdateVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
            var (ok, vs, url) = LamToolClient.GetApiResponseAsync(Globals.DeviceId, Globals.NameApp, version);

            if (!ok)
            {
                MessageBox.Show("Đã xảy ra lỗi vui lòng liên hệ admin để được hỗ trợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TempLoginStorage.Clear();
                Application.Restart();
                Environment.Exit(0);
                return;
            }

            if (LamToolClient.IsNewerVersion(version, vs))
            {
                toolStripMenuItem2.Text = $"Đã có version [{vs}] mới nhất.";
                toolStripMenuItem2.ToolTipText = "Đã có version mới nhất bạn có muốn cập nhật không? ";
                uiHeaderButton1.TipsText = "1";
            }
        }
        private void uiTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uiPanel5_Click(object sender, EventArgs e)
        {

        }

        private void uiLedStopwatch1_TimerTick(object sender, EventArgs e)
        {

        }

        private void uiImageButton2_Click(object sender, EventArgs e)
        {
            uiImageButton2.ShowContextMenuStrip(uiContextMenuStrip1, 0, uiImageButton2.Height);
        }

        private void dDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TempLoginStorage.Clear();
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
