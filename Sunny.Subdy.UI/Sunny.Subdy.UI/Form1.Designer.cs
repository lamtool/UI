namespace Sunny.Subdy.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            TreeNode treeNode1 = new TreeNode("Node0");
            TreeNode treeNode2 = new TreeNode("Node1");
            TreeNode treeNode3 = new TreeNode("Node2");
            TreeNode treeNode4 = new TreeNode("Node3");
            TreeNode treeNode5 = new TreeNode("Node4");
            uiPanel1 = new Sunny.UI.UIPanel();
            uiPanel3 = new Sunny.UI.UIPanel();
            uiImageButton2 = new Sunny.UI.UIImageButton();
            uiPanel2 = new Sunny.UI.UIPanel();
            uiLabel1 = new Sunny.UI.UILabel();
            panel1 = new Panel();
            popupBoxControl1 = new Sunny.Subdy.UI.ControlViews.PopupBoxControl();
            uiImageButton1 = new Sunny.UI.UIImageButton();
            notificationBell1 = new Sunny.Subdy.UI.ControlViews.NotificationBell();
            uiNavMenu1 = new Sunny.UI.UINavMenu();
            panel2 = new Panel();
            uiPanel1.SuspendLayout();
            uiPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiImageButton2).BeginInit();
            uiPanel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiImageButton1).BeginInit();
            SuspendLayout();
            // 
            // uiPanel1
            // 
            uiPanel1.BackColor = Color.White;
            uiPanel1.Controls.Add(uiPanel3);
            uiPanel1.Controls.Add(uiPanel2);
            uiPanel1.Controls.Add(panel1);
            uiPanel1.Controls.Add(uiImageButton1);
            uiPanel1.Dock = DockStyle.Top;
            uiPanel1.FillColor = Color.White;
            uiPanel1.FillColor2 = Color.White;
            uiPanel1.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel1.Location = new Point(0, 0);
            uiPanel1.Margin = new Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.RectColor = Color.White;
            uiPanel1.RectDisableColor = Color.White;
            uiPanel1.Size = new Size(1081, 112);
            uiPanel1.TabIndex = 6;
            uiPanel1.Text = null;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiPanel3
            // 
            uiPanel3.Controls.Add(notificationBell1);
            uiPanel3.Controls.Add(uiImageButton2);
            uiPanel3.Dock = DockStyle.Right;
            uiPanel3.FillColor = Color.Transparent;
            uiPanel3.FillColor2 = Color.Transparent;
            uiPanel3.FillDisableColor = Color.Transparent;
            uiPanel3.Font = new Font("Franklin Gothic Medium", 14.25F, FontStyle.Bold);
            uiPanel3.Location = new Point(824, 35);
            uiPanel3.Margin = new Padding(4, 5, 4, 5);
            uiPanel3.MinimumSize = new Size(1, 1);
            uiPanel3.Name = "uiPanel3";
            uiPanel3.Padding = new Padding(20, 15, 20, 35);
            uiPanel3.RectColor = Color.Transparent;
            uiPanel3.RectDisableColor = Color.Transparent;
            uiPanel3.Size = new Size(257, 77);
            uiPanel3.TabIndex = 7;
            uiPanel3.Text = null;
            uiPanel3.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiImageButton2
            // 
            uiImageButton2.Dock = DockStyle.Right;
            uiImageButton2.Font = new Font("Microsoft Sans Serif", 12F);
            uiImageButton2.Image = Properties.Resources.account;
            uiImageButton2.Location = new Point(212, 15);
            uiImageButton2.Margin = new Padding(0, 0, 10, 0);
            uiImageButton2.Name = "uiImageButton2";
            uiImageButton2.Size = new Size(25, 27);
            uiImageButton2.SizeMode = PictureBoxSizeMode.StretchImage;
            uiImageButton2.TabIndex = 0;
            uiImageButton2.TabStop = false;
            uiImageButton2.Text = null;
            // 
            // uiPanel2
            // 
            uiPanel2.Controls.Add(uiLabel1);
            uiPanel2.Dock = DockStyle.Fill;
            uiPanel2.FillColor = Color.Transparent;
            uiPanel2.FillColor2 = Color.Transparent;
            uiPanel2.FillDisableColor = Color.Transparent;
            uiPanel2.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel2.Location = new Point(226, 35);
            uiPanel2.Margin = new Padding(4, 5, 4, 5);
            uiPanel2.MinimumSize = new Size(1, 1);
            uiPanel2.Name = "uiPanel2";
            uiPanel2.RectColor = Color.Transparent;
            uiPanel2.RectDisableColor = Color.Transparent;
            uiPanel2.Size = new Size(855, 77);
            uiPanel2.TabIndex = 4;
            uiPanel2.Text = null;
            uiPanel2.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiLabel1
            // 
            uiLabel1.Dock = DockStyle.Fill;
            uiLabel1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel1.Location = new Point(0, 0);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Padding = new Padding(20, 0, 0, 0);
            uiLabel1.Size = new Size(855, 77);
            uiLabel1.TabIndex = 0;
            uiLabel1.Text = "Quản lý tài khoản";
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Controls.Add(popupBoxControl1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(226, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(855, 35);
            panel1.TabIndex = 3;
            // 
            // popupBoxControl1
            // 
            popupBoxControl1.BackColor = Color.Transparent;
            popupBoxControl1.Dock = DockStyle.Right;
            popupBoxControl1.Location = new Point(657, 0);
            popupBoxControl1.Name = "popupBoxControl1";
            popupBoxControl1.Padding = new Padding(0, 10, 10, 0);
            popupBoxControl1.Size = new Size(198, 35);
            popupBoxControl1.TabIndex = 2;
            // 
            // uiImageButton1
            // 
            uiImageButton1.Dock = DockStyle.Left;
            uiImageButton1.Font = new Font("Microsoft Sans Serif", 12F);
            uiImageButton1.Image = Properties.Resources.Logo;
            uiImageButton1.ImageDisabled = Properties.Resources.Logo;
            uiImageButton1.ImageHover = Properties.Resources.Logo;
            uiImageButton1.ImagePress = Properties.Resources.Logo;
            uiImageButton1.ImageSelected = Properties.Resources.Logo;
            uiImageButton1.Location = new Point(0, 0);
            uiImageButton1.Name = "uiImageButton1";
            uiImageButton1.Size = new Size(226, 112);
            uiImageButton1.SizeMode = PictureBoxSizeMode.StretchImage;
            uiImageButton1.TabIndex = 1;
            uiImageButton1.TabStop = false;
            uiImageButton1.Text = null;
            // 
            // notificationBell1
            // 
            notificationBell1.Anchor = AnchorStyles.Right;
            notificationBell1.BackColor = Color.Transparent;
            notificationBell1.Font = new Font("Franklin Gothic Medium", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            notificationBell1.Location = new Point(177, 9);
            notificationBell1.Name = "notificationBell1";
            notificationBell1.Notifications = (List<string>)resources.GetObject("notificationBell1.Notifications");
            notificationBell1.Size = new Size(32, 36);
            notificationBell1.TabIndex = 1;
            // 
            // uiNavMenu1
            // 
            uiNavMenu1.BackColor = Color.White;
            uiNavMenu1.BorderStyle = BorderStyle.None;
            uiNavMenu1.Dock = DockStyle.Left;
            uiNavMenu1.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            uiNavMenu1.FillColor = Color.White;
            uiNavMenu1.Font = new Font("Microsoft Sans Serif", 12F);
            uiNavMenu1.ForeColor = Color.Gray;
            uiNavMenu1.FullRowSelect = true;
            uiNavMenu1.HotTracking = true;
            uiNavMenu1.HoverColor = Color.Cyan;
            uiNavMenu1.ItemHeight = 50;
            uiNavMenu1.LineColor = Color.White;
            uiNavMenu1.Location = new Point(0, 112);
            uiNavMenu1.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            uiNavMenu1.Name = "uiNavMenu1";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Node0";
            treeNode2.Name = "Node1";
            treeNode2.Text = "Node1";
            treeNode3.Name = "Node2";
            treeNode3.Text = "Node2";
            treeNode4.Name = "Node3";
            treeNode4.Text = "Node3";
            treeNode5.Name = "Node4";
            treeNode5.Text = "Node4";
            uiNavMenu1.Nodes.AddRange(new TreeNode[] { treeNode1, treeNode2, treeNode3, treeNode4, treeNode5 });
            uiNavMenu1.ShowLines = false;
            uiNavMenu1.ShowPlusMinus = false;
            uiNavMenu1.ShowRootLines = false;
            uiNavMenu1.Size = new Size(226, 445);
            uiNavMenu1.TabIndex = 7;
            uiNavMenu1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(226, 522);
            panel2.Name = "panel2";
            panel2.Size = new Size(855, 35);
            panel2.TabIndex = 8;
            // 
            // Form1
            // 
            AllowShowTitle = false;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1081, 557);
            Controls.Add(panel2);
            Controls.Add(uiNavMenu1);
            Controls.Add(uiPanel1);
            EscClose = true;
            Font = new Font("Franklin Gothic Medium", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaximumSize = new Size(1920, 1040);
            Name = "Form1";
            Padding = new Padding(0);
            ShowTitle = false;
            Text = "Form1";
            TitleHeight = 29;
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            Load += Form1_Load;
            uiPanel1.ResumeLayout(false);
            uiPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiImageButton2).EndInit();
            uiPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiImageButton1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIImageButton uiImageButton1;
        private ControlViews.PopupBoxControl popupBoxControl1;
        private Panel panel1;
        private Sunny.UI.UIPanel uiPanel3;
        private Sunny.UI.UIPanel uiPanel2;
        private Sunny.UI.UIImageButton uiImageButton2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UINavMenu uiNavMenu1;
        private ControlViews.NotificationBell notificationBell1;
        private Panel panel2;
    }
}
