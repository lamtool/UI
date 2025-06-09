namespace Sunny.Subdy.UI.View.Pages
{
    partial class fTest
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
            uiAnalogMeter1 = new Sunny.UI.UIAnalogMeter();
            uiButton1 = new Sunny.UI.UIButton();
            uiComboTreeView1 = new Sunny.UI.UIComboTreeView();
            SuspendLayout();
            // 
            // uiAnalogMeter1
            // 
            uiAnalogMeter1.Font = new Font("Microsoft Sans Serif", 12F);
            uiAnalogMeter1.Location = new Point(208, 106);
            uiAnalogMeter1.MaxValue = 100D;
            uiAnalogMeter1.MinimumSize = new Size(1, 1);
            uiAnalogMeter1.MinValue = 0D;
            uiAnalogMeter1.Name = "uiAnalogMeter1";
            uiAnalogMeter1.Renderer = null;
            uiAnalogMeter1.Size = new Size(180, 180);
            uiAnalogMeter1.TabIndex = 0;
            uiAnalogMeter1.Text = "uiAnalogMeter1";
            uiAnalogMeter1.Value = 0D;
            // 
            // uiButton1
            // 
            uiButton1.Font = new Font("Microsoft Sans Serif", 12F);
            uiButton1.Location = new Point(672, 127);
            uiButton1.MinimumSize = new Size(1, 1);
            uiButton1.Name = "uiButton1";
            uiButton1.Size = new Size(100, 35);
            uiButton1.TabIndex = 1;
            uiButton1.Text = "uiButton1";
            uiButton1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // uiComboTreeView1
            // 
            uiComboTreeView1.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            uiComboTreeView1.FillColor = Color.White;
            uiComboTreeView1.Font = new Font("Microsoft Sans Serif", 12F);
            uiComboTreeView1.Location = new Point(440, 274);
            uiComboTreeView1.Margin = new Padding(4, 5, 4, 5);
            uiComboTreeView1.MinimumSize = new Size(63, 0);
            uiComboTreeView1.Name = "uiComboTreeView1";
            uiComboTreeView1.Padding = new Padding(0, 0, 30, 2);
            uiComboTreeView1.Size = new Size(150, 29);
            uiComboTreeView1.SymbolSize = 24;
            uiComboTreeView1.TabIndex = 2;
            uiComboTreeView1.Text = "uiComboTreeView1";
            uiComboTreeView1.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboTreeView1.Watermark = "";
            // 
            // fTest
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(uiComboTreeView1);
            Controls.Add(uiButton1);
            Controls.Add(uiAnalogMeter1);
            ForeColor = Color.Black;
            Name = "fTest";
            Text = "Thống Kê";
            TitleFillColor = Color.White;
            TitleForeColor = Color.Black;
            Initialize += fTest_Initialize;
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIAnalogMeter uiAnalogMeter1;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIComboTreeView uiComboTreeView1;
    }
}