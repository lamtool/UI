namespace Sunny.Subdy.UI.View.Controls
{
    partial class fEditScirpt
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
            uiLabel1 = new Sunny.UI.UILabel();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            txtDate = new Sunny.UI.UIDatePicker();
            txtType = new Sunny.UI.UIComboBox();
            txtName = new Sunny.UI.UITextBox();
            SuspendLayout();
            // 
            // uiLabel1
            // 
            uiLabel1.Dock = DockStyle.Top;
            uiLabel1.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiLabel1.ForeColor = Color.Black;
            uiLabel1.Location = new Point(0, 0);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(532, 40);
            uiLabel1.TabIndex = 1;
            uiLabel1.Text = "Tạo kịch bản";
            uiLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.FillColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.FillColor2 = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.FillHoverColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.FillPressColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.FillSelectedColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton1.Location = new Point(124, 260);
            uiSymbolButton1.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Radius = 15;
            uiSymbolButton1.RectColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.RectHoverColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.RectPressColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.RectSelectedColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.Size = new Size(128, 35);
            uiSymbolButton1.Symbol = 61543;
            uiSymbolButton1.SymbolSize = 15;
            uiSymbolButton1.TabIndex = 16;
            uiSymbolButton1.Text = "Lưu";
            uiSymbolButton1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.FillColor = Color.DarkRed;
            uiSymbolButton2.FillColor2 = Color.DarkRed;
            uiSymbolButton2.FillHoverColor = Color.DarkRed;
            uiSymbolButton2.FillPressColor = Color.DarkRed;
            uiSymbolButton2.FillSelectedColor = Color.DarkRed;
            uiSymbolButton2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton2.Location = new Point(281, 260);
            uiSymbolButton2.Margin = new Padding(10, 3, 3, 3);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Radius = 15;
            uiSymbolButton2.RectColor = Color.DarkRed;
            uiSymbolButton2.RectHoverColor = Color.DarkRed;
            uiSymbolButton2.RectPressColor = Color.DarkRed;
            uiSymbolButton2.RectSelectedColor = Color.DarkRed;
            uiSymbolButton2.Size = new Size(128, 35);
            uiSymbolButton2.Symbol = 61453;
            uiSymbolButton2.SymbolSize = 15;
            uiSymbolButton2.TabIndex = 15;
            uiSymbolButton2.Text = "Đóng";
            uiSymbolButton2.TipsColor = Color.DarkRed;
            uiSymbolButton2.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // txtDate
            // 
            txtDate.FillColor = Color.White;
            txtDate.Font = new Font("Microsoft Sans Serif", 12F);
            txtDate.Location = new Point(70, 192);
            txtDate.Margin = new Padding(4, 5, 4, 5);
            txtDate.MaxLength = 10;
            txtDate.MinimumSize = new Size(63, 0);
            txtDate.Name = "txtDate";
            txtDate.Padding = new Padding(0, 0, 30, 2);
            txtDate.ReadOnly = true;
            txtDate.Size = new Size(389, 29);
            txtDate.SymbolDropDown = 61555;
            txtDate.SymbolNormal = 61555;
            txtDate.SymbolSize = 24;
            txtDate.TabIndex = 14;
            txtDate.Text = "2025-06-13";
            txtDate.TextAlignment = ContentAlignment.MiddleLeft;
            txtDate.Value = new DateTime(2025, 6, 13, 14, 46, 43, 0);
            txtDate.Watermark = "";
            // 
            // txtType
            // 
            txtType.DataSource = null;
            txtType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            txtType.FillColor = Color.White;
            txtType.Font = new Font("Microsoft Sans Serif", 12F);
            txtType.ItemHoverColor = Color.FromArgb(155, 200, 255);
            txtType.Items.AddRange(new object[] { "Facebook", "Instagram", "TikTok", "Gmail" });
            txtType.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            txtType.Location = new Point(70, 82);
            txtType.Margin = new Padding(4, 5, 4, 5);
            txtType.MinimumSize = new Size(63, 0);
            txtType.Name = "txtType";
            txtType.Padding = new Padding(0, 0, 30, 2);
            txtType.Size = new Size(389, 29);
            txtType.SymbolSize = 24;
            txtType.TabIndex = 13;
            txtType.TextAlignment = ContentAlignment.MiddleLeft;
            txtType.Watermark = "";
            // 
            // txtName
            // 
            txtName.FillColor = Color.White;
            txtName.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtName.Location = new Point(70, 131);
            txtName.Margin = new Padding(4, 5, 4, 5);
            txtName.MinimumSize = new Size(1, 16);
            txtName.Name = "txtName";
            txtName.Padding = new Padding(5);
            txtName.ShowText = false;
            txtName.Size = new Size(389, 35);
            txtName.Symbol = 363070;
            txtName.TabIndex = 12;
            txtName.TextAlignment = ContentAlignment.MiddleLeft;
            txtName.Watermark = "Nhập tên kịch bản...";
            // 
            // fEditScirpt
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(532, 328);
            Controls.Add(uiSymbolButton1);
            Controls.Add(uiSymbolButton2);
            Controls.Add(txtDate);
            Controls.Add(txtType);
            Controls.Add(txtName);
            Controls.Add(uiLabel1);
            Name = "fEditScirpt";
            Text = "fEditScirpt";
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UISymbolButton uiSymbolButton2;
        private Sunny.UI.UIDatePicker txtDate;
        private Sunny.UI.UIComboBox txtType;
        private Sunny.UI.UITextBox txtName;
    }
}