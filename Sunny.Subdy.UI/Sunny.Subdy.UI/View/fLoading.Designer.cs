﻿namespace Sunny.Subdy.UI.View
{
    partial class fLoading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fLoading));
            uiRoundProcess1 = new Sunny.UI.UIRoundProcess();
            SuspendLayout();
            // 
            // uiRoundProcess1
            // 
            uiRoundProcess1.Dock = DockStyle.Fill;
            uiRoundProcess1.Font = new Font("Microsoft Sans Serif", 12F);
            uiRoundProcess1.ForeColor2 = Color.Black;
            uiRoundProcess1.Location = new Point(0, 0);
            uiRoundProcess1.MinimumSize = new Size(1, 1);
            uiRoundProcess1.Name = "uiRoundProcess1";
            uiRoundProcess1.Size = new Size(136, 114);
            uiRoundProcess1.TabIndex = 1;
            uiRoundProcess1.Text = "0.0%";
            // 
            // fLoading
            // 
            AllowShowTitle = false;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(136, 114);
            Controls.Add(uiRoundProcess1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "fLoading";
            Padding = new Padding(0);
            RectColor = Color.Transparent;
            ShowTitle = false;
            Text = "fLoading";
            TextAlignment = StringAlignment.Center;
            TopMost = true;
            ZoomScaleRect = new Rectangle(15, 15, 490, 379);
            Load += fLoading_Load;
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIRoundProcess uiRoundProcess1;
    }
}