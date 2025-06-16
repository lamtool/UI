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
            uiSymbolButton3 = new Sunny.UI.UISymbolButton();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            View = new PictureBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            debugToolStripMenuItem = new ToolStripMenuItem();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
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
            panel2.Controls.Add(uiSymbolButton3);
            panel2.Controls.Add(uiSymbolButton2);
            panel2.Controls.Add(uiSymbolButton1);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new System.Drawing.Point(5, 353);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(5);
            panel2.Size = new Size(223, 31);
            panel2.TabIndex = 1;
            // 
            // uiSymbolButton3
            // 
            uiSymbolButton3.Cursor = Cursors.Hand;
            uiSymbolButton3.FillColor = Color.Transparent;
            uiSymbolButton3.FillColor2 = Color.Transparent;
            uiSymbolButton3.FillHoverColor = Color.Transparent;
            uiSymbolButton3.FillPressColor = Color.Transparent;
            uiSymbolButton3.FillSelectedColor = Color.Transparent;
            uiSymbolButton3.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton3.ForeColor = Color.Transparent;
            uiSymbolButton3.ForeHoverColor = Color.Transparent;
            uiSymbolButton3.ForePressColor = Color.Transparent;
            uiSymbolButton3.ForeSelectedColor = Color.Transparent;
            uiSymbolButton3.Location = new System.Drawing.Point(134, 4);
            uiSymbolButton3.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton3.MinimumSize = new Size(1, 1);
            uiSymbolButton3.Name = "uiSymbolButton3";
            uiSymbolButton3.Radius = 15;
            uiSymbolButton3.RectColor = Color.Transparent;
            uiSymbolButton3.RectHoverColor = Color.Transparent;
            uiSymbolButton3.RectPressColor = Color.Transparent;
            uiSymbolButton3.RectSelectedColor = Color.Transparent;
            uiSymbolButton3.Size = new Size(20, 22);
            uiSymbolButton3.Symbol = 361517;
            uiSymbolButton3.SymbolHoverColor = Color.DodgerBlue;
            uiSymbolButton3.SymbolPressColor = Color.DodgerBlue;
            uiSymbolButton3.SymbolSelectedColor = Color.DodgerBlue;
            uiSymbolButton3.SymbolSize = 25;
            uiSymbolButton3.TabIndex = 15;
            uiSymbolButton3.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton3.Click += uiSymbolButton3_Click;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.Cursor = Cursors.Hand;
            uiSymbolButton2.FillColor = Color.Transparent;
            uiSymbolButton2.FillColor2 = Color.Transparent;
            uiSymbolButton2.FillHoverColor = Color.Transparent;
            uiSymbolButton2.FillPressColor = Color.Transparent;
            uiSymbolButton2.FillSelectedColor = Color.Transparent;
            uiSymbolButton2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton2.ForeColor = Color.Transparent;
            uiSymbolButton2.ForeHoverColor = Color.Transparent;
            uiSymbolButton2.ForePressColor = Color.Transparent;
            uiSymbolButton2.ForeSelectedColor = Color.Transparent;
            uiSymbolButton2.Location = new System.Drawing.Point(66, 5);
            uiSymbolButton2.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Radius = 15;
            uiSymbolButton2.RectColor = Color.Transparent;
            uiSymbolButton2.RectHoverColor = Color.Transparent;
            uiSymbolButton2.RectPressColor = Color.Transparent;
            uiSymbolButton2.RectSelectedColor = Color.Transparent;
            uiSymbolButton2.Size = new Size(22, 22);
            uiSymbolButton2.Symbol = 361514;
            uiSymbolButton2.SymbolHoverColor = Color.DodgerBlue;
            uiSymbolButton2.SymbolPressColor = Color.DodgerBlue;
            uiSymbolButton2.SymbolSelectedColor = Color.DodgerBlue;
            uiSymbolButton2.SymbolSize = 25;
            uiSymbolButton2.TabIndex = 14;
            uiSymbolButton2.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Cursor = Cursors.Hand;
            uiSymbolButton1.FillColor = Color.Transparent;
            uiSymbolButton1.FillColor2 = Color.Transparent;
            uiSymbolButton1.FillHoverColor = Color.Transparent;
            uiSymbolButton1.FillPressColor = Color.Transparent;
            uiSymbolButton1.FillSelectedColor = Color.Transparent;
            uiSymbolButton1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton1.ForeColor = Color.Transparent;
            uiSymbolButton1.ForeHoverColor = Color.Transparent;
            uiSymbolButton1.ForePressColor = Color.Transparent;
            uiSymbolButton1.ForeSelectedColor = Color.Transparent;
            uiSymbolButton1.Location = new System.Drawing.Point(101, 5);
            uiSymbolButton1.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Radius = 15;
            uiSymbolButton1.RectColor = Color.Transparent;
            uiSymbolButton1.RectHoverColor = Color.Transparent;
            uiSymbolButton1.RectPressColor = Color.Transparent;
            uiSymbolButton1.RectSelectedColor = Color.Transparent;
            uiSymbolButton1.Size = new Size(20, 22);
            uiSymbolButton1.Symbol = 361713;
            uiSymbolButton1.SymbolHoverColor = Color.DodgerBlue;
            uiSymbolButton1.SymbolPressColor = Color.DodgerBlue;
            uiSymbolButton1.SymbolSelectedColor = Color.DodgerBlue;
            uiSymbolButton1.SymbolSize = 25;
            uiSymbolButton1.TabIndex = 13;
            uiSymbolButton1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
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
            panel2.ResumeLayout(false);
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
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UISymbolButton uiSymbolButton3;
        private Sunny.UI.UISymbolButton uiSymbolButton2;
    }
}
