using Sunny.Subdy.UI.Commons;
using Sunny.Subdy.UI.Helper;
using Sunny.Subdy.UI.View.Pages;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Sunny.Subdy.UI
{
    public partial class fMain : UIForm2
    {
        public fMain()
        {
            InitializeComponent();
           
        }
        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        public async Task LoadUI()
        {
            notificationBell1.Notifications = new List<string> { "Thông báo 1", "Thông báo 2", "Thông báo 3" };
            new DragHandler(popupMessageBox, this);
            new DragHandler(uiLabel1, this);
            int pageIndex = 1000;
            popupBoxControl1.RegisterButtonEvents(
          BtnMinimize_Click,
          BtnMaximize_Click,
          BtnClose_Click
      );
            this.MainTabControl = uiTabControl1;
            uiNavMenu1.TabControl = uiTabControl1;
            uiNavMenu1.CreateNode(AddPage(new fTest()));
            TreeNode parent = uiNavMenu1.CreateNode("Ứng dụng", 57512, 24, pageIndex);
            uiNavMenu1.CreateChildNode(parent, AddPage(new pageFacebook(), ++pageIndex));
            uiNavMenu1.CreateChildNode(parent, AddPage(new pageTikTok(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pagePhone()));
            uiNavMenu1.CreateNode(AddPage(new pageGroupAccount()));
            uiNavMenu1.CreateNode(AddPage(new pageScript()));
            uiNavMenu1.CreateNode(AddPage(new pageApi()));
            uiNavMenu1.CreateNode(AddPage(new pageSetting()));
            uiNavMenu1.CreateNode(AddPage(new pageSupport()));
            timer1.Start();
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
            System.Windows.Forms.Application.Exit();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            string cpu = $"{SystemUsageMonitor.GetCpuUsage():0.00}%";
            string ram = $"{SystemUsageMonitor.GetRamUsage():0.00}%";

            if (uiLabel6.InvokeRequired)
            {
                uiLabel6.Invoke(new Action(() => uiLabel6.Text = ram));
            }
            else
            {
                uiLabel6.Text = ram;
            }

            if (uiLabel5.InvokeRequired)
            {
                uiLabel5.Invoke(new Action(() => uiLabel5.Text = cpu));
            }
            else
            {
                uiLabel5.Text = cpu;
            }
        }
    }
}
