using Sunny.Subdy.UI.Commons;
using Sunny.Subdy.UI.Helper;
using Sunny.Subdy.UI.View.Pages;
using Sunny.UI;

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
            uiNavMenu1.CreateChildNode(parent, AddPage(new pageFacebook(), ++pageIndex));
            uiNavMenu1.CreateChildNode(parent, AddPage(new pageTikTok(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pagePhone(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageGroupAccount(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageScript(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageApi(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageSetting(), ++pageIndex));
            uiNavMenu1.CreateNode(AddPage(new pageSupport(), ++pageIndex));
            uiNavMenu1.MenuItemClick += UiNavMenu1_MenuItemClick;
            uiNavMenu1.SelectPage(5);
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

        private void uiTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
