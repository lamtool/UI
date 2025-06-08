using System;
using System.Windows.Forms;

namespace Sunny.UI
{
    partial class UIForm
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
            components = new System.ComponentModel.Container();
            imageList = new ImageList(components);
            SuspendLayout();
            // 
            // imageList
            // 
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new System.Drawing.Size(16, 16);
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // UIForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(243, 249, 255);
            ClientSize = new System.Drawing.Size(800, 480);
            DoubleBuffered = true;
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Name = "UIForm";
            Padding = new Padding(0, 35, 0, 0);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UIForm";
            Load += UIForm_Load;
            ResumeLayout(false);
        }

        #endregion
        private ImageList imageList;
    }
}