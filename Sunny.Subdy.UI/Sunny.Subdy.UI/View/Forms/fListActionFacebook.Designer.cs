namespace Sunny.Subdy.UI.View.Forms
{
    partial class fListActionFacebook
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
            groupBox1 = new GroupBox();
            btn_FB_SpamXu = new Sunny.UI.UISymbolButton();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btn_FB_SpamXu);
            groupBox1.Dock = DockStyle.Left;
            groupBox1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(15, 15);
            groupBox1.Margin = new Padding(5);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(5);
            groupBox1.Size = new Size(253, 450);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Spam";
            // 
            // btn_FB_SpamXu
            // 
            btn_FB_SpamXu.Dock = DockStyle.Top;
            btn_FB_SpamXu.FillColor = Color.White;
            btn_FB_SpamXu.FillColor2 = Color.White;
            btn_FB_SpamXu.FillHoverColor = Color.Cyan;
            btn_FB_SpamXu.FillPressColor = Color.Cyan;
            btn_FB_SpamXu.FillSelectedColor = Color.Cyan;
            btn_FB_SpamXu.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_FB_SpamXu.ForeColor = Color.Black;
            btn_FB_SpamXu.Location = new Point(5, 25);
            btn_FB_SpamXu.Margin = new Padding(3, 3, 10, 3);
            btn_FB_SpamXu.MinimumSize = new Size(1, 1);
            btn_FB_SpamXu.Name = "btn_FB_SpamXu";
            btn_FB_SpamXu.Radius = 15;
            btn_FB_SpamXu.RectColor = Color.FromArgb(4, 60, 44);
            btn_FB_SpamXu.RectHoverColor = Color.FromArgb(4, 60, 44);
            btn_FB_SpamXu.RectPressColor = Color.FromArgb(4, 60, 44);
            btn_FB_SpamXu.RectSelectedColor = Color.FromArgb(4, 60, 44);
            btn_FB_SpamXu.Size = new Size(243, 35);
            btn_FB_SpamXu.Symbol = 362750;
            btn_FB_SpamXu.SymbolColor = Color.Black;
            btn_FB_SpamXu.SymbolSize = 15;
            btn_FB_SpamXu.TabIndex = 1;
            btn_FB_SpamXu.Text = "Spam xu";
            btn_FB_SpamXu.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btn_FB_SpamXu.Click += uiSymbolButton1_Click;
            // 
            // fListActionFacebook
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1017, 480);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fListActionFacebook";
            Padding = new Padding(15);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Sunny.UI.UISymbolButton btn_FB_SpamXu;
    }
}