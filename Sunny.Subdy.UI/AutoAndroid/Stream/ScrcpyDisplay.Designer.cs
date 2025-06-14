using System.Drawing;
using System.Windows.Forms;

namespace AutoAndroid.Stream
{
    partial class ScrcpyDisplay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            View = new PictureBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            debugToolStripMenuItem = new ToolStripMenuItem();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)View).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new System.Drawing.Point(5, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(223, 22);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(4, 60, 44);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(10, 0, 0, 0);
            label1.Size = new Size(223, 22);
            label1.TabIndex = 1;
            label1.Text = "[SM-G930F]-ce021602202e251802";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(4, 60, 44);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new System.Drawing.Point(5, 353);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(5);
            panel2.Size = new Size(223, 31);
            panel2.TabIndex = 1;
            // 
            // View
            // 
            View.ContextMenuStrip = contextMenuStrip1;
            View.Dock = DockStyle.Fill;
            View.ErrorImage = null;
            View.Image = Properties.Resources.LamTool;
            View.Location = new System.Drawing.Point(5, 27);
            View.Name = "View";
            View.Size = new Size(223, 326);
            View.SizeMode = PictureBoxSizeMode.StretchImage;
            View.TabIndex = 2;
            View.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { debugToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(110, 26);
            // 
            // debugToolStripMenuItem
            // 
            debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            debugToolStripMenuItem.Size = new Size(109, 22);
            debugToolStripMenuItem.Text = "Debug";
            debugToolStripMenuItem.Click += debugToolStripMenuItem_Click;
            // 
            // ScrcpyDisplay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(4, 60, 44);
            Controls.Add(View);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ScrcpyDisplay";
            Padding = new Padding(5);
            Size = new Size(233, 389);
            Load += ScrcpyDisplay_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)View).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public Panel panel1;
        private Panel panel2;
        public PictureBox View;
        public Label label1;
        internal Sunny.UI.UIHeaderButton uiHeaderButton2;
        internal Sunny.UI.UIHeaderButton uiHeaderButton1;
        internal Sunny.UI.UIHeaderButton bellButton;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem debugToolStripMenuItem;
    }
}
