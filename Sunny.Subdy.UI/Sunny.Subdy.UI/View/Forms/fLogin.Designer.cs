using Sunny.UI;

namespace Sunny.Subdy.UI.View.Forms
{
    partial class fLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fLogin));
            uiLine1 = new UILine();
            edtUser = new UITextBox();
            edtPassword = new UITextBox();
            btnLogin = new UISymbolButton();
            btnCancel = new UISymbolButton();
            uiPanel1 = new UIPanel();
            uiLinkLabel1 = new UILinkLabel();
            uiAvatar1 = new UIAvatar();
            panel1 = new System.Windows.Forms.Panel();
            uiSymbolButton5 = new UISymbolButton();
            uiSymbolButton4 = new UISymbolButton();
            uiSymbolButton3 = new UISymbolButton();
            uiSymbolButton2 = new UISymbolButton();
            uiSymbolButton1 = new UISymbolButton();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            uiPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiLine1
            // 
            uiLine1.BackColor = System.Drawing.Color.Transparent;
            uiLine1.Font = new System.Drawing.Font("Segoe UI", 9F);
            uiLine1.ForeColor = System.Drawing.Color.FromArgb(48, 48, 48);
            uiLine1.Location = new System.Drawing.Point(3, 88);
            uiLine1.MinimumSize = new System.Drawing.Size(2, 2);
            uiLine1.Name = "uiLine1";
            uiLine1.RadiusSides = UICornerRadiusSides.None;
            uiLine1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            uiLine1.Size = new System.Drawing.Size(287, 28);
            uiLine1.StyleCustomMode = true;
            uiLine1.TabIndex = 5;
            uiLine1.Text = "Đăng nhập";
            // 
            // edtUser
            // 
            edtUser.Cursor = System.Windows.Forms.Cursors.IBeam;
            edtUser.EnterAsTab = true;
            edtUser.FillColor = System.Drawing.Color.White;
            edtUser.Font = new System.Drawing.Font("Segoe UI", 9F);
            edtUser.Location = new System.Drawing.Point(4, 121);
            edtUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            edtUser.MinimumSize = new System.Drawing.Size(1, 1);
            edtUser.Name = "edtUser";
            edtUser.Padding = new System.Windows.Forms.Padding(5);
            edtUser.ShowText = false;
            edtUser.Size = new System.Drawing.Size(285, 29);
            edtUser.Symbol = 361447;
            edtUser.SymbolSize = 22;
            edtUser.TabIndex = 0;
            edtUser.Watermark = "Tài khoản";
            // 
            // edtPassword
            // 
            edtPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            edtPassword.FillColor = System.Drawing.Color.White;
            edtPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            edtPassword.Location = new System.Drawing.Point(4, 162);
            edtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            edtPassword.MinimumSize = new System.Drawing.Size(1, 1);
            edtPassword.Name = "edtPassword";
            edtPassword.Padding = new System.Windows.Forms.Padding(5);
            edtPassword.PasswordChar = '*';
            edtPassword.ShowText = false;
            edtPassword.Size = new System.Drawing.Size(285, 29);
            edtPassword.Symbol = 361475;
            edtPassword.SymbolSize = 22;
            edtPassword.TabIndex = 1;
            edtPassword.Watermark = "Mật khẩu";
            // 
            // btnLogin
            // 
            btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            btnLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnLogin.Location = new System.Drawing.Point(39, 218);
            btnLogin.MinimumSize = new System.Drawing.Size(1, 1);
            btnLogin.Name = "btnLogin";
            btnLogin.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            btnLogin.ShowFocusColor = true;
            btnLogin.Size = new System.Drawing.Size(102, 29);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Đăng nhập";
            btnLogin.TipsFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            btnLogin.Click += btnLogin_Click;
            // 
            // btnCancel
            // 
            btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCancel.FillColor = System.Drawing.Color.FromArgb(230, 80, 80);
            btnCancel.FillColor2 = System.Drawing.Color.FromArgb(230, 80, 80);
            btnCancel.FillHoverColor = System.Drawing.Color.FromArgb(235, 115, 115);
            btnCancel.FillPressColor = System.Drawing.Color.FromArgb(184, 64, 64);
            btnCancel.FillSelectedColor = System.Drawing.Color.FromArgb(184, 64, 64);
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnCancel.Location = new System.Drawing.Point(163, 218);
            btnCancel.MinimumSize = new System.Drawing.Size(1, 1);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            btnCancel.RectColor = System.Drawing.Color.FromArgb(230, 80, 80);
            btnCancel.RectHoverColor = System.Drawing.Color.FromArgb(235, 115, 115);
            btnCancel.RectPressColor = System.Drawing.Color.FromArgb(184, 64, 64);
            btnCancel.RectSelectedColor = System.Drawing.Color.FromArgb(184, 64, 64);
            btnCancel.ShowFocusColor = true;
            btnCancel.Size = new System.Drawing.Size(102, 29);
            btnCancel.Style = UIStyle.Custom;
            btnCancel.StyleCustomMode = true;
            btnCancel.Symbol = 361453;
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Thoát";
            btnCancel.TipsFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            btnCancel.Click += btnCancel_Click;
            // 
            // uiPanel1
            // 
            uiPanel1.Controls.Add(uiLinkLabel1);
            uiPanel1.Controls.Add(uiAvatar1);
            uiPanel1.Controls.Add(uiLine1);
            uiPanel1.Controls.Add(edtUser);
            uiPanel1.Controls.Add(edtPassword);
            uiPanel1.Controls.Add(btnCancel);
            uiPanel1.Controls.Add(btnLogin);
            uiPanel1.FillColor = System.Drawing.Color.White;
            uiPanel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            uiPanel1.Location = new System.Drawing.Point(350, 48);
            uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.RadiusSides = UICornerRadiusSides.None;
            uiPanel1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            uiPanel1.Size = new System.Drawing.Size(293, 345);
            uiPanel1.Style = UIStyle.Custom;
            uiPanel1.StyleCustomMode = true;
            uiPanel1.TabIndex = 9;
            uiPanel1.Text = null;
            // 
            // uiLinkLabel1
            // 
            uiLinkLabel1.ActiveLinkColor = System.Drawing.Color.FromArgb(80, 160, 255);
            uiLinkLabel1.BackColor = System.Drawing.Color.Transparent;
            uiLinkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            uiLinkLabel1.ForeColor = System.Drawing.Color.FromArgb(48, 48, 48);
            uiLinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            uiLinkLabel1.Location = new System.Drawing.Point(3, 259);
            uiLinkLabel1.Name = "uiLinkLabel1";
            uiLinkLabel1.Size = new System.Drawing.Size(287, 19);
            uiLinkLabel1.TabIndex = 7;
            uiLinkLabel1.TabStop = true;
            uiLinkLabel1.Text = "Bạn chưa có tài khoản? Đăng kí tài khoản.";
            uiLinkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            uiLinkLabel1.VisitedLinkColor = System.Drawing.Color.FromArgb(230, 80, 80);
            uiLinkLabel1.Click += uiLinkLabel1_Click;
            // 
            // uiAvatar1
            // 
            uiAvatar1.BackColor = System.Drawing.Color.Transparent;
            uiAvatar1.FillColor = System.Drawing.Color.Transparent;
            uiAvatar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            uiAvatar1.Location = new System.Drawing.Point(115, 22);
            uiAvatar1.MinimumSize = new System.Drawing.Size(1, 1);
            uiAvatar1.Name = "uiAvatar1";
            uiAvatar1.Size = new System.Drawing.Size(60, 60);
            uiAvatar1.TabIndex = 6;
            uiAvatar1.Text = "uiAvatar1";
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.FromArgb(4, 60, 44);
            panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            panel1.Controls.Add(uiSymbolButton5);
            panel1.Controls.Add(uiSymbolButton4);
            panel1.Controls.Add(uiSymbolButton3);
            panel1.Controls.Add(uiSymbolButton2);
            panel1.Controls.Add(uiSymbolButton1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Dock = System.Windows.Forms.DockStyle.Left;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(252, 450);
            panel1.TabIndex = 10;
            // 
            // uiSymbolButton5
            // 
            uiSymbolButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            uiSymbolButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            uiSymbolButton5.FillColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.FillColor2 = System.Drawing.Color.Transparent;
            uiSymbolButton5.FillDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.FillHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.FillPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.FillSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            uiSymbolButton5.ForeColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.ForeDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.ForeHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.ForePressColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.ForeSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.Image = Properties.Resources.icons8_zalo_40;
            uiSymbolButton5.LightColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.Location = new System.Drawing.Point(204, 363);
            uiSymbolButton5.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            uiSymbolButton5.MinimumSize = new System.Drawing.Size(1, 1);
            uiSymbolButton5.Name = "uiSymbolButton5";
            uiSymbolButton5.Radius = 15;
            uiSymbolButton5.RectColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.RectDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.RectHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.RectPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.RectSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton5.Size = new System.Drawing.Size(31, 30);
            uiSymbolButton5.Symbol = 57514;
            uiSymbolButton5.SymbolColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton5.SymbolHoverColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton5.SymbolPressColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton5.SymbolSelectedColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton5.SymbolSize = 35;
            uiSymbolButton5.TabIndex = 17;
            uiSymbolButton5.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            uiSymbolButton5.Click += uiSymbolButton5_Click;
            // 
            // uiSymbolButton4
            // 
            uiSymbolButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            uiSymbolButton4.FillColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.FillColor2 = System.Drawing.Color.Transparent;
            uiSymbolButton4.FillDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.FillHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.FillPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.FillSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            uiSymbolButton4.ForeColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.ForeDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.ForeHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.ForePressColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.ForeSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.LightColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.Location = new System.Drawing.Point(151, 363);
            uiSymbolButton4.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            uiSymbolButton4.MinimumSize = new System.Drawing.Size(1, 1);
            uiSymbolButton4.Name = "uiSymbolButton4";
            uiSymbolButton4.Radius = 10;
            uiSymbolButton4.RectColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.RectDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.RectHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.RectPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.RectSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton4.Size = new System.Drawing.Size(40, 30);
            uiSymbolButton4.Symbol = 61802;
            uiSymbolButton4.SymbolHoverColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton4.SymbolPressColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton4.SymbolSelectedColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton4.SymbolSize = 40;
            uiSymbolButton4.TabIndex = 16;
            uiSymbolButton4.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            uiSymbolButton4.Click += uiSymbolButton4_Click;
            // 
            // uiSymbolButton3
            // 
            uiSymbolButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            uiSymbolButton3.FillColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.FillColor2 = System.Drawing.Color.Transparent;
            uiSymbolButton3.FillDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.FillHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.FillPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.FillSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            uiSymbolButton3.ForeColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.ForeDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.ForeHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.ForePressColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.ForeSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.LightColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.Location = new System.Drawing.Point(107, 363);
            uiSymbolButton3.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            uiSymbolButton3.MinimumSize = new System.Drawing.Size(1, 1);
            uiSymbolButton3.Name = "uiSymbolButton3";
            uiSymbolButton3.Radius = 15;
            uiSymbolButton3.RectColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.RectDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.RectHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.RectPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.RectSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton3.Size = new System.Drawing.Size(31, 30);
            uiSymbolButton3.Symbol = 560030;
            uiSymbolButton3.SymbolHoverColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton3.SymbolPressColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton3.SymbolSelectedColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton3.SymbolSize = 40;
            uiSymbolButton3.TabIndex = 15;
            uiSymbolButton3.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            uiSymbolButton3.Click += uiSymbolButton3_Click;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            uiSymbolButton2.FillColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.FillColor2 = System.Drawing.Color.Transparent;
            uiSymbolButton2.FillDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.FillHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.FillPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.FillSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            uiSymbolButton2.ForeColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.ForeDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.ForeHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.ForePressColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.ForeSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.LightColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.Location = new System.Drawing.Point(63, 363);
            uiSymbolButton2.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            uiSymbolButton2.MinimumSize = new System.Drawing.Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Radius = 15;
            uiSymbolButton2.RectColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.RectDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.RectHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.RectPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.RectSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton2.Size = new System.Drawing.Size(31, 30);
            uiSymbolButton2.Symbol = 62150;
            uiSymbolButton2.SymbolHoverColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton2.SymbolPressColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton2.SymbolSelectedColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton2.SymbolSize = 37;
            uiSymbolButton2.TabIndex = 14;
            uiSymbolButton2.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            uiSymbolButton1.FillColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.FillColor2 = System.Drawing.Color.Transparent;
            uiSymbolButton1.FillDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.FillHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.FillPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.FillSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            uiSymbolButton1.ForeColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.ForeDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.ForeHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.ForePressColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.ForeSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.LightColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.Location = new System.Drawing.Point(21, 363);
            uiSymbolButton1.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            uiSymbolButton1.MinimumSize = new System.Drawing.Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Radius = 15;
            uiSymbolButton1.RectColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.RectDisableColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.RectHoverColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.RectPressColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.RectSelectedColor = System.Drawing.Color.Transparent;
            uiSymbolButton1.Size = new System.Drawing.Size(29, 30);
            uiSymbolButton1.Symbol = 57514;
            uiSymbolButton1.SymbolHoverColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton1.SymbolPressColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton1.SymbolSelectedColor = System.Drawing.Color.DodgerBlue;
            uiSymbolButton1.SymbolSize = 36;
            uiSymbolButton1.TabIndex = 13;
            uiSymbolButton1.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI Semibold", 26F);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(23, 110);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(213, 47);
            label1.TabIndex = 0;
            label1.Text = "LamTool.net";
            // 
            // label2
            // 
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(129, 157);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(107, 29);
            label2.TabIndex = 1;
            label2.Text = "Giải pháp MMO";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fLogin
            // 
            AllowShowTitle = false;
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(750, 450);
            Controls.Add(panel1);
            Controls.Add(uiPanel1);
            EscClose = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximumSize = new System.Drawing.Size(750, 450);
            MinimumSize = new System.Drawing.Size(750, 450);
            Name = "fLogin";
            Padding = new System.Windows.Forms.Padding(0);
            ShowTitle = false;
            Text = "LamTool.net";
            TopMost = true;
            ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 750, 450);
            uiPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private UILine uiLine1;
        private UITextBox edtUser;
        private UITextBox edtPassword;
        private UISymbolButton btnLogin;
        private UISymbolButton btnCancel;
        protected UIPanel uiPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private UISymbolButton uiSymbolButton5;
        private UISymbolButton uiSymbolButton4;
        private UISymbolButton uiSymbolButton3;
        private UISymbolButton uiSymbolButton2;
        private UISymbolButton uiSymbolButton1;
        private UILinkLabel uiLinkLabel1;
        private UIAvatar uiAvatar1;
    }
}