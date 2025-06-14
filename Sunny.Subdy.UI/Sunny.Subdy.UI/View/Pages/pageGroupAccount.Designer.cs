namespace Sunny.Subdy.UI.View.Pages
{
    partial class pageGroupAccount
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
            uiPanel1 = new Sunny.UI.UIPanel();
            txtType = new Sunny.UI.UIComboBox();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            uiTextBox1 = new Sunny.UI.UITextBox();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            flowLayoutPanel1 = new FlowLayoutPanel();
            uiPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiPanel1
            // 
            uiPanel1.BackColor = Color.WhiteSmoke;
            uiPanel1.Controls.Add(txtType);
            uiPanel1.Controls.Add(uiSymbolButton2);
            uiPanel1.Controls.Add(uiTextBox1);
            uiPanel1.Controls.Add(uiSymbolButton1);
            uiPanel1.Dock = DockStyle.Top;
            uiPanel1.FillColor = Color.White;
            uiPanel1.FillColor2 = Color.WhiteSmoke;
            uiPanel1.FillDisableColor = SystemColors.Window;
            uiPanel1.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel1.Location = new Point(0, 0);
            uiPanel1.Margin = new Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.Radius = 25;
            uiPanel1.RectColor = Color.WhiteSmoke;
            uiPanel1.Size = new Size(1035, 96);
            uiPanel1.TabIndex = 0;
            uiPanel1.Text = null;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            uiPanel1.Click += uiPanel1_Click;
            // 
            // txtType
            // 
            txtType.DataSource = null;
            txtType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            txtType.FillColor = Color.White;
            txtType.Font = new Font("Microsoft Sans Serif", 12F);
            txtType.ItemHoverColor = Color.FromArgb(155, 200, 255);
            txtType.Items.AddRange(new object[] { "Tất cả", "Facebook", "Instagram", "TikTok", "Gmail" });
            txtType.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            txtType.Location = new Point(314, 26);
            txtType.Margin = new Padding(4, 5, 4, 5);
            txtType.MinimumSize = new Size(63, 0);
            txtType.Name = "txtType";
            txtType.Padding = new Padding(0, 0, 30, 2);
            txtType.Size = new Size(155, 22);
            txtType.SymbolSize = 24;
            txtType.TabIndex = 11;
            txtType.TextAlignment = ContentAlignment.MiddleLeft;
            txtType.Watermark = "";
            txtType.SelectedIndexChanged += txtType_SelectedIndexChanged;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.BackColor = Color.White;
            uiSymbolButton2.Cursor = Cursors.Hand;
            uiSymbolButton2.FillColor = Color.White;
            uiSymbolButton2.FillColor2 = Color.White;
            uiSymbolButton2.FillDisableColor = Color.White;
            uiSymbolButton2.FillHoverColor = Color.White;
            uiSymbolButton2.FillPressColor = Color.White;
            uiSymbolButton2.FillSelectedColor = Color.White;
            uiSymbolButton2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton2.Location = new Point(238, 26);
            uiSymbolButton2.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Radius = 15;
            uiSymbolButton2.RectColor = Color.White;
            uiSymbolButton2.RectDisableColor = Color.White;
            uiSymbolButton2.RectHoverColor = Color.White;
            uiSymbolButton2.RectPressColor = Color.White;
            uiSymbolButton2.RectSelectedColor = Color.White;
            uiSymbolButton2.Size = new Size(30, 22);
            uiSymbolButton2.Symbol = 61473;
            uiSymbolButton2.SymbolColor = Color.CornflowerBlue;
            uiSymbolButton2.SymbolDisableColor = Color.White;
            uiSymbolButton2.SymbolHoverColor = Color.CornflowerBlue;
            uiSymbolButton2.SymbolPressColor = Color.CornflowerBlue;
            uiSymbolButton2.SymbolSelectedColor = Color.CornflowerBlue;
            uiSymbolButton2.SymbolSize = 25;
            uiSymbolButton2.TabIndex = 10;
            uiSymbolButton2.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // uiTextBox1
            // 
            uiTextBox1.FillColor = Color.White;
            uiTextBox1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiTextBox1.Location = new Point(16, 26);
            uiTextBox1.Margin = new Padding(4, 5, 4, 5);
            uiTextBox1.MinimumSize = new Size(1, 16);
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(5);
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(215, 22);
            uiTextBox1.Symbol = 61442;
            uiTextBox1.TabIndex = 8;
            uiTextBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBox1.Watermark = "Tìm kiếm...";
            uiTextBox1.TextChanged += uiTextBox1_TextChanged;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.BackColor = Color.White;
            uiSymbolButton1.Cursor = Cursors.Hand;
            uiSymbolButton1.FillColor = Color.White;
            uiSymbolButton1.FillColor2 = Color.White;
            uiSymbolButton1.FillHoverColor = Color.White;
            uiSymbolButton1.FillPressColor = Color.White;
            uiSymbolButton1.FillSelectedColor = Color.White;
            uiSymbolButton1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton1.Location = new Point(270, 26);
            uiSymbolButton1.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Radius = 15;
            uiSymbolButton1.RectColor = Color.White;
            uiSymbolButton1.RectHoverColor = Color.White;
            uiSymbolButton1.RectPressColor = Color.White;
            uiSymbolButton1.RectSelectedColor = Color.White;
            uiSymbolButton1.Size = new Size(30, 22);
            uiSymbolButton1.Symbol = 61543;
            uiSymbolButton1.SymbolColor = Color.Green;
            uiSymbolButton1.SymbolHoverColor = Color.Green;
            uiSymbolButton1.SymbolPressColor = Color.Green;
            uiSymbolButton1.SymbolSelectedColor = Color.Green;
            uiSymbolButton1.SymbolSize = 25;
            uiSymbolButton1.TabIndex = 7;
            uiSymbolButton1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 96);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(0, 20, 0, 20);
            flowLayoutPanel1.Size = new Size(1035, 442);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // pageGroupAccount
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1035, 538);
            ControlBoxForeColor = Color.FromArgb(173, 178, 181);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(uiPanel1);
            Name = "pageGroupAccount";
            Text = "Nhóm TK";
            Load += pageGroupAccount_Load;
            uiPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UITextBox uiTextBox1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UISymbolButton uiSymbolButton2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Sunny.UI.UIComboBox txtType;
    }
}