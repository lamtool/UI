namespace Sunny.Subdy.AutoUpdate
{
    partial class fUpdate
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
            uiLabel1 = new Sunny.UI.UILabel();
            panel1 = new Panel();
            uiProcessBar1 = new Sunny.UI.UIProcessBar();
            uiLabel2 = new Sunny.UI.UILabel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiLabel1
            // 
            uiLabel1.BackColor = Color.White;
            uiLabel1.Dock = DockStyle.Top;
            uiLabel1.Font = new Font("Microsoft Sans Serif", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiLabel1.ForeColor = Color.FromArgb(4, 60, 44);
            uiLabel1.Location = new Point(0, 0);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(501, 47);
            uiLabel1.TabIndex = 2;
            uiLabel1.Text = "LamTool.net AutoUpdate";
            uiLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(uiProcessBar1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 177);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(20);
            panel1.Size = new Size(501, 69);
            panel1.TabIndex = 3;
            // 
            // uiProcessBar1
            // 
            uiProcessBar1.Dock = DockStyle.Fill;
            uiProcessBar1.FillColor = Color.FromArgb(235, 243, 255);
            uiProcessBar1.Font = new Font("Microsoft Sans Serif", 12F);
            uiProcessBar1.Location = new Point(20, 20);
            uiProcessBar1.MinimumSize = new Size(3, 3);
            uiProcessBar1.Name = "uiProcessBar1";
            uiProcessBar1.Size = new Size(461, 29);
            uiProcessBar1.TabIndex = 4;
            uiProcessBar1.Text = "uiProcessBar1";
           // uiProcessBar1.ValueChanged += this.uiProcessBar1_ValueChanged;
            // 
            // uiLabel2
            // 
            uiLabel2.Dock = DockStyle.Fill;
            uiLabel2.Font = new Font("Microsoft Sans Serif", 12F);
            uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel2.Location = new Point(0, 47);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Padding = new Padding(20, 5, 10, 10);
            uiLabel2.Size = new Size(501, 130);
            uiLabel2.TabIndex = 4;
            uiLabel2.Text = "uiLabel2";
            // 
            // fUpdate
            // 
            AllowShowTitle = false;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(501, 246);
            Controls.Add(uiLabel2);
            Controls.Add(panel1);
            Controls.Add(uiLabel1);
            Name = "fUpdate";
            Padding = new Padding(0);
            ShowIcon = false;
            ShowShadow = false;
            ShowTitle = false;
            Text = "Form1";
            ZoomScaleRect = new Rectangle(15, 15, 667, 386);
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private UI.UILabel uiLabel1;
        private Panel panel1;
        private UI.UIProcessBar uiProcessBar1;
        private UI.UILabel uiLabel2;
    }
}
