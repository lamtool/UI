using Sunny.Subdy.UI.View.Pages;

namespace Sunny.Subdy.UI
{
    partial class fMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer updateStatsTimer; // Thêm Timer để cập nhật các chỉ số

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            uiPanel1 = new Sunny.UI.UIPanel();
            uiNavMenu1 = new Sunny.UI.UINavMenu();
            uiTabControl1 = new Sunny.UI.UITabControl();
            uiPanel2 = new Sunny.UI.UIPanel();
            uiImageButton1 = new Sunny.UI.UIImageButton();
            uiLabel1 = new Sunny.UI.UILabel();
            uiPanel3 = new Sunny.UI.UIPanel();
            notificationBell1 = new Sunny.Subdy.UI.ControlViews.NotificationBell();
            uiImageButton2 = new Sunny.UI.UIImageButton();
            uiPanel4 = new Sunny.UI.UIPanel();
            uiLabel6 = new Sunny.UI.UILabel();
            uiLabel5 = new Sunny.UI.UILabel();
            uiLabel3 = new Sunny.UI.UILabel();
            uiLabel2 = new Sunny.UI.UILabel();
            uiPanel5 = new Sunny.UI.UIPanel();
            popupMessageBox = new Sunny.UI.UIPanel();
            uiLabel7 = new Sunny.UI.UILabel();
            popupBoxControl1 = new Sunny.Subdy.UI.ControlViews.PopupBoxControl();
            timer1 = new System.Windows.Forms.Timer(components);
            uiLabel4 = new Sunny.UI.UILabel();
            uiPanel1.SuspendLayout();
            uiPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiImageButton1).BeginInit();
            uiPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiImageButton2).BeginInit();
            uiPanel4.SuspendLayout();
            uiPanel5.SuspendLayout();
            popupMessageBox.SuspendLayout();
            SuspendLayout();
            // 
            // uiPanel1
            // 
            uiPanel1.BackColor = Color.FromArgb(1, 68, 33);
            uiPanel1.Controls.Add(uiNavMenu1);
            uiPanel1.Controls.Add(uiPanel2);
            uiPanel1.Dock = DockStyle.Left;
            uiPanel1.FillColor = Color.FromArgb(1, 68, 33);
            uiPanel1.FillColor2 = Color.FromArgb(1, 68, 33);
            uiPanel1.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel1.ForeColor = Color.White;
            uiPanel1.Location = new Point(2, 0);
            uiPanel1.Margin = new Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.RectColor = Color.FromArgb(1, 68, 33);
            uiPanel1.Size = new Size(55, 574);
            uiPanel1.TabIndex = 0;
            uiPanel1.Text = null;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiNavMenu1
            // 
            uiNavMenu1.BackColor = Color.FromArgb(4, 60, 44);
            uiNavMenu1.BorderStyle = BorderStyle.None;
            uiNavMenu1.Dock = DockStyle.Fill;
            uiNavMenu1.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            uiNavMenu1.FillColor = Color.FromArgb(4, 60, 44);
            uiNavMenu1.Font = new Font("Microsoft Sans Serif", 12F);
            uiNavMenu1.ForeColor = Color.White;
            uiNavMenu1.FullRowSelect = true;
            uiNavMenu1.HotTracking = true;
            uiNavMenu1.HoverColor = Color.Gray;
            uiNavMenu1.ItemHeight = 50;
            uiNavMenu1.Location = new Point(0, 75);
            uiNavMenu1.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            uiNavMenu1.Name = "uiNavMenu1";
            uiNavMenu1.SelectedForeColor = Color.White;
            uiNavMenu1.ShowLines = false;
            uiNavMenu1.ShowPlusMinus = false;
            uiNavMenu1.ShowRootLines = false;
            uiNavMenu1.Size = new Size(55, 499);
            uiNavMenu1.TabControl = uiTabControl1;
            uiNavMenu1.TabIndex = 2;
            uiNavMenu1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // uiTabControl1
            // 
            uiTabControl1.Dock = DockStyle.Fill;
            uiTabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            uiTabControl1.FillColor = Color.White;
            uiTabControl1.Font = new Font("Microsoft Sans Serif", 12F);
            uiTabControl1.Frame = this;
            uiTabControl1.ItemSize = new Size(0, 1);
            uiTabControl1.Location = new Point(24, 24);
            uiTabControl1.MainPage = "";
            uiTabControl1.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            uiTabControl1.Name = "uiTabControl1";
            uiTabControl1.SelectedIndex = 0;
            uiTabControl1.ShowToolTips = true;
            uiTabControl1.Size = new Size(1018, 403);
            uiTabControl1.SizeMode = TabSizeMode.Fixed;
            uiTabControl1.TabBackColor = Color.White;
            uiTabControl1.TabIndex = 0;
            uiTabControl1.TabSelectedForeColor = Color.White;
            uiTabControl1.TabSelectedHighColor = Color.White;
            uiTabControl1.TabUnSelectedForeColor = Color.White;
            uiTabControl1.TabVisible = false;
            uiTabControl1.TipsFont = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiTabControl1.SelectedIndexChanged += uiTabControl1_SelectedIndexChanged;
            // 
            // uiPanel2
            // 
            uiPanel2.BackColor = Color.FromArgb(4, 60, 44);
            uiPanel2.Controls.Add(uiImageButton1);
            uiPanel2.Dock = DockStyle.Top;
            uiPanel2.FillColor = Color.FromArgb(4, 60, 44);
            uiPanel2.FillColor2 = Color.FromArgb(4, 60, 44);
            uiPanel2.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel2.ForeColor = Color.White;
            uiPanel2.Location = new Point(0, 0);
            uiPanel2.Margin = new Padding(4, 5, 4, 5);
            uiPanel2.MinimumSize = new Size(1, 1);
            uiPanel2.Name = "uiPanel2";
            uiPanel2.RectColor = Color.FromArgb(4, 60, 44);
            uiPanel2.Size = new Size(55, 75);
            uiPanel2.TabIndex = 1;
            uiPanel2.Text = null;
            uiPanel2.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiImageButton1
            // 
            uiImageButton1.Cursor = Cursors.Hand;
            uiImageButton1.Font = new Font("Microsoft Sans Serif", 12F);
            uiImageButton1.ForeColor = Color.White;
            uiImageButton1.Image = Properties.Resources.menu_80;
            uiImageButton1.Location = new Point(13, 16);
            uiImageButton1.Name = "uiImageButton1";
            uiImageButton1.Size = new Size(32, 34);
            uiImageButton1.SizeMode = PictureBoxSizeMode.StretchImage;
            uiImageButton1.TabIndex = 0;
            uiImageButton1.TabStop = false;
            uiImageButton1.Text = null;
            uiImageButton1.Click += uiImageButton1_Click;
            // 
            // uiLabel1
            // 
            uiLabel1.Dock = DockStyle.Left;
            uiLabel1.Font = new Font("Microsoft Sans Serif", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiLabel1.ForeColor = Color.FromArgb(4, 60, 44);
            uiLabel1.Location = new Point(12, 10);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(198, 30);
            uiLabel1.TabIndex = 1;
            uiLabel1.Text = "LamTool.net";
            // 
            // uiPanel3
            // 
            uiPanel3.BackColor = Color.White;
            uiPanel3.Controls.Add(uiLabel4);
            uiPanel3.Controls.Add(notificationBell1);
            uiPanel3.Controls.Add(uiImageButton2);
            uiPanel3.Dock = DockStyle.Top;
            uiPanel3.FillColor = Color.White;
            uiPanel3.FillColor2 = Color.White;
            uiPanel3.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel3.Location = new Point(57, 50);
            uiPanel3.Margin = new Padding(4, 5, 4, 5);
            uiPanel3.MinimumSize = new Size(1, 1);
            uiPanel3.Name = "uiPanel3";
            uiPanel3.Padding = new Padding(20, 0, 20, 0);
            uiPanel3.Radius = 12;
            uiPanel3.RectColor = Color.White;
            uiPanel3.Size = new Size(1066, 73);
            uiPanel3.Style = Sunny.UI.UIStyle.Custom;
            uiPanel3.TabIndex = 4;
            uiPanel3.Text = null;
            uiPanel3.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // notificationBell1
            // 
            notificationBell1.Anchor = AnchorStyles.Right;
            notificationBell1.BackColor = Color.Transparent;
            notificationBell1.Cursor = Cursors.Hand;
            notificationBell1.Location = new Point(940, 17);
            notificationBell1.Name = "notificationBell1";
            notificationBell1.Notifications = (List<string>)resources.GetObject("notificationBell1.Notifications");
            notificationBell1.Size = new Size(53, 40);
            notificationBell1.TabIndex = 3;
            // 
            // uiImageButton2
            // 
            uiImageButton2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            uiImageButton2.Cursor = Cursors.Hand;
            uiImageButton2.Font = new Font("Microsoft Sans Serif", 12F);
            uiImageButton2.Image = Properties.Resources.account;
            uiImageButton2.Location = new Point(999, 27);
            uiImageButton2.Name = "uiImageButton2";
            uiImageButton2.Size = new Size(30, 30);
            uiImageButton2.SizeMode = PictureBoxSizeMode.StretchImage;
            uiImageButton2.TabIndex = 0;
            uiImageButton2.TabStop = false;
            uiImageButton2.Text = null;
            // 
            // uiPanel4
            // 
            uiPanel4.Controls.Add(uiLabel6);
            uiPanel4.Controls.Add(uiLabel5);
            uiPanel4.Controls.Add(uiLabel3);
            uiPanel4.Controls.Add(uiLabel2);
            uiPanel4.Dock = DockStyle.Right;
            uiPanel4.FillColor = Color.White;
            uiPanel4.FillColor2 = Color.FromArgb(249, 250, 251);
            uiPanel4.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel4.ForeColor = Color.Black;
            uiPanel4.Location = new Point(452, 10);
            uiPanel4.Margin = new Padding(4, 5, 4, 5);
            uiPanel4.MinimumSize = new Size(1, 1);
            uiPanel4.Name = "uiPanel4";
            uiPanel4.Radius = 30;
            uiPanel4.RectColor = Color.White;
            uiPanel4.Size = new Size(402, 30);
            uiPanel4.TabIndex = 2;
            uiPanel4.Text = null;
            uiPanel4.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiLabel6
            // 
            uiLabel6.BackColor = Color.Transparent;
            uiLabel6.Font = new Font("Microsoft Sans Serif", 9.75F);
            uiLabel6.ForeColor = Color.DarkGreen;
            uiLabel6.Location = new Point(315, 8);
            uiLabel6.Name = "uiLabel6";
            uiLabel6.Size = new Size(66, 18);
            uiLabel6.TabIndex = 4;
            uiLabel6.Text = "68%";
            uiLabel6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel5
            // 
            uiLabel5.BackColor = Color.Transparent;
            uiLabel5.Font = new Font("Microsoft Sans Serif", 9.75F);
            uiLabel5.ForeColor = Color.FromArgb(0, 0, 192);
            uiLabel5.Location = new Point(202, 8);
            uiLabel5.Name = "uiLabel5";
            uiLabel5.Size = new Size(56, 18);
            uiLabel5.TabIndex = 3;
            uiLabel5.Text = "68%";
            uiLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            uiLabel3.BackColor = Color.Transparent;
            uiLabel3.Font = new Font("Microsoft Sans Serif", 9.75F);
            uiLabel3.ForeColor = Color.DimGray;
            uiLabel3.Location = new Point(264, 8);
            uiLabel3.Name = "uiLabel3";
            uiLabel3.Size = new Size(45, 18);
            uiLabel3.TabIndex = 1;
            uiLabel3.Text = "RAM:";
            uiLabel3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // uiLabel2
            // 
            uiLabel2.BackColor = Color.Transparent;
            uiLabel2.Font = new Font("Microsoft Sans Serif", 9.75F);
            uiLabel2.ForeColor = Color.DimGray;
            uiLabel2.Location = new Point(154, 8);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(42, 18);
            uiLabel2.TabIndex = 0;
            uiLabel2.Text = "CPU:";
            uiLabel2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // uiPanel5
            // 
            uiPanel5.BackColor = Color.FromArgb(245, 245, 245);
            uiPanel5.Controls.Add(uiTabControl1);
            uiPanel5.Dock = DockStyle.Fill;
            uiPanel5.FillColor = Color.FromArgb(245, 245, 245);
            uiPanel5.FillColor2 = Color.FromArgb(245, 245, 245);
            uiPanel5.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel5.Location = new Point(57, 123);
            uiPanel5.Margin = new Padding(4, 5, 4, 5);
            uiPanel5.MinimumSize = new Size(1, 1);
            uiPanel5.Name = "uiPanel5";
            uiPanel5.Padding = new Padding(24);
            uiPanel5.Radius = 25;
            uiPanel5.RectColor = Color.FromArgb(245, 245, 245);
            uiPanel5.Size = new Size(1066, 451);
            uiPanel5.TabIndex = 5;
            uiPanel5.Text = null;
            uiPanel5.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // popupMessageBox
            // 
            popupMessageBox.BackColor = Color.Transparent;
            popupMessageBox.Controls.Add(uiLabel7);
            popupMessageBox.Controls.Add(uiPanel4);
            popupMessageBox.Controls.Add(uiLabel1);
            popupMessageBox.Controls.Add(popupBoxControl1);
            popupMessageBox.Cursor = Cursors.SizeAll;
            popupMessageBox.Dock = DockStyle.Top;
            popupMessageBox.FillColor = Color.White;
            popupMessageBox.FillColor2 = Color.White;
            popupMessageBox.Font = new Font("Microsoft Sans Serif", 12F);
            popupMessageBox.ForeColor = Color.FromArgb(30, 58, 138);
            popupMessageBox.Location = new Point(57, 0);
            popupMessageBox.Margin = new Padding(8, 10, 8, 0);
            popupMessageBox.MinimumSize = new Size(1, 1);
            popupMessageBox.Name = "popupMessageBox";
            popupMessageBox.Padding = new Padding(12, 10, 12, 10);
            popupMessageBox.Radius = 8;
            popupMessageBox.RectColor = Color.Transparent;
            popupMessageBox.Size = new Size(1066, 50);
            popupMessageBox.TabIndex = 3;
            popupMessageBox.Text = null;
            popupMessageBox.TextAlignment = ContentAlignment.MiddleLeft;
            // 
            // uiLabel7
            // 
            uiLabel7.Dock = DockStyle.Left;
            uiLabel7.Font = new Font("Microsoft Sans Serif", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiLabel7.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel7.Location = new Point(210, 10);
            uiLabel7.Name = "uiLabel7";
            uiLabel7.Size = new Size(122, 30);
            uiLabel7.TabIndex = 2;
            uiLabel7.Text = "v17.09.06.2025";
            uiLabel7.TextAlign = ContentAlignment.BottomLeft;
            // 
            // popupBoxControl1
            // 
            popupBoxControl1.BackColor = Color.Transparent;
            popupBoxControl1.Dock = DockStyle.Right;
            popupBoxControl1.Location = new Point(854, 10);
            popupBoxControl1.Name = "popupBoxControl1";
            popupBoxControl1.Padding = new Padding(0, 10, 10, 0);
            popupBoxControl1.Size = new Size(200, 30);
            popupBoxControl1.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // uiLabel4
            // 
            uiLabel4.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiLabel4.ForeColor = Color.FromArgb(4, 60, 44);
            uiLabel4.Location = new Point(24, 25);
            uiLabel4.Name = "uiLabel4";
            uiLabel4.Size = new Size(243, 30);
            uiLabel4.TabIndex = 4;
            uiLabel4.Text = "Thống Kê";
            uiLabel4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // fMain
            // 
            AllowShowTitle = false;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(4, 60, 44);
            ClientSize = new Size(1125, 576);
            ControlBoxCloseFillHoverColor = Color.FromArgb(4, 60, 44);
            ControlBoxFillHoverColor = Color.FromArgb(4, 60, 44);
            ControlBoxForeColor = Color.FromArgb(4, 60, 44);
            Controls.Add(uiPanel5);
            Controls.Add(uiPanel3);
            Controls.Add(popupMessageBox);
            Controls.Add(uiPanel1);
            Font = new Font("Segoe UI", 10F);
            MainTabControl = uiTabControl1;
            Name = "fMain";
            Padding = new Padding(2, 0, 2, 2);
            RectColor = Color.FromArgb(4, 60, 44);
            Resizable = true;
            RightToLeft = RightToLeft.No;
            ShowDragStretch = true;
            ShowTitle = false;
            Style = Sunny.UI.UIStyle.Custom;
            Text = "fMain";
            TextAlignment = StringAlignment.Center;
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            uiPanel1.ResumeLayout(false);
            uiPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiImageButton1).EndInit();
            uiPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiImageButton2).EndInit();
            uiPanel4.ResumeLayout(false);
            uiPanel5.ResumeLayout(false);
            popupMessageBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIPanel uiPanel2;
        private Sunny.UI.UIImageButton uiImageButton1;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UINavMenu uiNavMenu1;
        private Sunny.UI.UIPanel uiPanel3;
        private Sunny.UI.UIImageButton uiImageButton2;
        private Sunny.UI.UIPanel uiPanel4;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UIPanel uiPanel5;
        private Sunny.UI.UIPanel popupMessageBox;
        private ControlViews.PopupBoxControl popupBoxControl1;
        private Sunny.UI.UITabControl uiTabControl1;
        private Sunny.UI.UILabel uiLabel6;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UILabel uiLabel7;
        private ControlViews.NotificationBell notificationBell1;
        private System.Windows.Forms.Timer timer1;
        private Sunny.UI.UILabel uiLabel4;
    }
}
