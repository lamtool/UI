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
            SuspendLayout();
            // 
            // fTest
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            BackgroundImage = Properties.Resources.PanelThanks;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(800, 450);
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
    }
}