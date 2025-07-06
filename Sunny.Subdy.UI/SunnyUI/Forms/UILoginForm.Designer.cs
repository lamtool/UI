namespace Sunny.UI
{
    partial class UILoginForm
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
            uiAvatar1 = new UIAvatar();
            uiLine1 = new UILine();
            edtUser = new UITextBox();
            edtPassword = new UITextBox();
            btnLogin = new UISymbolButton();
            btnCancel = new UISymbolButton();
            uiPanel1 = new UIPanel();
            uiHeaderButton1 = new UIHeaderButton();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            uiHeaderButton2 = new UIHeaderButton();
            edtPassword.SuspendLayout();
            uiPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiAvatar1
            // 
            uiAvatar1.BackColor = System.Drawing.Color.Transparent;
            uiAvatar1.Font = new System.Drawing.Font("Segoe UI", 9F);
            uiAvatar1.Location = new System.Drawing.Point(65, 16);
            uiAvatar1.MinimumSize = new System.Drawing.Size(1, 1);
            uiAvatar1.Name = "uiAvatar1";
            uiAvatar1.Size = new System.Drawing.Size(60, 60);
            uiAvatar1.TabIndex = 4;
            uiAvatar1.Text = "uiAvatar1";
            // 
            // uiLine1
            // 
            uiLine1.BackColor = System.Drawing.Color.Transparent;
            uiLine1.Font = new System.Drawing.Font("Segoe UI", 9F);
            uiLine1.ForeColor = System.Drawing.Color.FromArgb(48, 48, 48);
            uiLine1.Location = new System.Drawing.Point(4, 85);
            uiLine1.MinimumSize = new System.Drawing.Size(2, 2);
            uiLine1.Name = "uiLine1";
            uiLine1.RadiusSides = UICornerRadiusSides.None;
            uiLine1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            uiLine1.Size = new System.Drawing.Size(182, 28);
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
            edtUser.Size = new System.Drawing.Size(182, 29);
            edtUser.Symbol = 361447;
            edtUser.SymbolSize = 22;
            edtUser.TabIndex = 0;
            edtUser.Watermark = "Tài khoản";
            // 
            // edtPassword
            // 
            edtPassword.Controls.Add(uiHeaderButton1);
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
            edtPassword.Size = new System.Drawing.Size(182, 29);
            edtPassword.Symbol = 361475;
            edtPassword.SymbolSize = 22;
            edtPassword.TabIndex = 1;
            edtPassword.Watermark = "Mật khẩu";
            edtPassword.DoEnter += btnLogin_Click;
            // 
            // btnLogin
            // 
            btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            btnLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnLogin.Location = new System.Drawing.Point(4, 206);
            btnLogin.MinimumSize = new System.Drawing.Size(1, 1);
            btnLogin.Name = "btnLogin";
            btnLogin.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            btnLogin.ShowFocusColor = true;
            btnLogin.Size = new System.Drawing.Size(86, 29);
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
            btnCancel.Location = new System.Drawing.Point(100, 206);
            btnCancel.MinimumSize = new System.Drawing.Size(1, 1);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new System.Windows.Forms.Padding(28, 0, 0, 0);
            btnCancel.RectColor = System.Drawing.Color.FromArgb(230, 80, 80);
            btnCancel.RectHoverColor = System.Drawing.Color.FromArgb(235, 115, 115);
            btnCancel.RectPressColor = System.Drawing.Color.FromArgb(184, 64, 64);
            btnCancel.RectSelectedColor = System.Drawing.Color.FromArgb(184, 64, 64);
            btnCancel.ShowFocusColor = true;
            btnCancel.Size = new System.Drawing.Size(86, 29);
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
            uiPanel1.Controls.Add(uiAvatar1);
            uiPanel1.Controls.Add(uiLine1);
            uiPanel1.Controls.Add(edtUser);
            uiPanel1.Controls.Add(edtPassword);
            uiPanel1.Controls.Add(btnCancel);
            uiPanel1.Controls.Add(btnLogin);
            uiPanel1.FillColor = System.Drawing.Color.White;
            uiPanel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            uiPanel1.Location = new System.Drawing.Point(514, 81);
            uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.RadiusSides = UICornerRadiusSides.None;
            uiPanel1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            uiPanel1.Size = new System.Drawing.Size(190, 245);
            uiPanel1.Style = UIStyle.Custom;
            uiPanel1.StyleCustomMode = true;
            uiPanel1.TabIndex = 9;
            uiPanel1.Text = null;
            // 
            // uiHeaderButton1
            // 
            uiHeaderButton1.BackColor = System.Drawing.Color.Transparent;
            uiHeaderButton1.CircleColor = System.Drawing.Color.Transparent;
            uiHeaderButton1.CircleHoverColor = System.Drawing.Color.Transparent;
            uiHeaderButton1.CircleSize = 15;
            uiHeaderButton1.FillColor = System.Drawing.Color.Transparent;
            uiHeaderButton1.FillDisableColor = System.Drawing.Color.Transparent;
            uiHeaderButton1.FillHoverColor = System.Drawing.Color.Transparent;
            uiHeaderButton1.FillPressColor = System.Drawing.Color.Transparent;
            uiHeaderButton1.FillSelectedColor = System.Drawing.Color.Transparent;
            uiHeaderButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            uiHeaderButton1.Location = new System.Drawing.Point(162, 0);
            uiHeaderButton1.MinimumSize = new System.Drawing.Size(1, 1);
            uiHeaderButton1.Name = "uiHeaderButton1";
            uiHeaderButton1.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
            uiHeaderButton1.Radius = 0;
            uiHeaderButton1.RadiusSides = UICornerRadiusSides.None;
            uiHeaderButton1.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            uiHeaderButton1.Size = new System.Drawing.Size(20, 29);
            uiHeaderButton1.Symbol = 61552;
            uiHeaderButton1.SymbolColor = System.Drawing.Color.Gray;
            uiHeaderButton1.SymbolSize = 15;
            uiHeaderButton1.TabIndex = 3;
            uiHeaderButton1.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.White;
            panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Dock = System.Windows.Forms.DockStyle.Left;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(252, 450);
            panel1.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI Semibold", 24F);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(29, 112);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(198, 45);
            label1.TabIndex = 0;
            label1.Text = "LamTool.net";
            // 
            // label2
            // 
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(120, 157);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(107, 29);
            label2.TabIndex = 1;
            label2.Text = "Giải pháp MMO";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiHeaderButton2
            // 
            uiHeaderButton2.BackColor = System.Drawing.Color.Transparent;
            uiHeaderButton2.CircleColor = System.Drawing.Color.Transparent;
            uiHeaderButton2.CircleHoverColor = System.Drawing.Color.Transparent;
            uiHeaderButton2.FillColor = System.Drawing.Color.Transparent;
            uiHeaderButton2.FillDisableColor = System.Drawing.Color.Transparent;
            uiHeaderButton2.FillHoverColor = System.Drawing.Color.Transparent;
            uiHeaderButton2.FillPressColor = System.Drawing.Color.Transparent;
            uiHeaderButton2.FillSelectedColor = System.Drawing.Color.Transparent;
            uiHeaderButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            uiHeaderButton2.Location = new System.Drawing.Point(321, 234);
            uiHeaderButton2.MinimumSize = new System.Drawing.Size(1, 1);
            uiHeaderButton2.Name = "uiHeaderButton2";
            uiHeaderButton2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
            uiHeaderButton2.Radius = 0;
            uiHeaderButton2.RadiusSides = UICornerRadiusSides.None;
            uiHeaderButton2.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            uiHeaderButton2.Size = new System.Drawing.Size(54, 55);
            uiHeaderButton2.Symbol = 161570;
            uiHeaderButton2.SymbolSize = 35;
            uiHeaderButton2.TabIndex = 4;
            uiHeaderButton2.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            // 
            // UILoginForm
            // 
            AllowShowTitle = false;
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(750, 450);
            Controls.Add(uiHeaderButton2);
            Controls.Add(panel1);
            Controls.Add(uiPanel1);
            EscClose = true;
            MaximumSize = new System.Drawing.Size(750, 450);
            MinimumSize = new System.Drawing.Size(750, 450);
            Name = "UILoginForm";
            Padding = new System.Windows.Forms.Padding(0);
            ShowIcon = false;
            ShowInTaskbar = false;
            ShowTitle = false;
            Text = "UILogin";
            TopMost = true;
            ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 750, 450);
            Shown += UILoginForm_Shown;
            edtPassword.ResumeLayout(false);
            uiPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private UIAvatar uiAvatar1;
        private UILine uiLine1;
        private UITextBox edtUser;
        private UITextBox edtPassword;
        private UISymbolButton btnLogin;
        private UISymbolButton btnCancel;
        protected UIPanel uiPanel1;
        private UIHeaderButton uiHeaderButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private UIHeaderButton uiHeaderButton2;
    }
}