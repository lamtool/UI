namespace Sunny.Subdy.UI.ControlViews
{
    partial class PopupBoxControl
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
            panelButtons = new Panel();
            btnClose = new Sunny.UI.UIButton();
            btnMaximize = new Sunny.UI.UIButton();
            btnMinimize = new Sunny.UI.UIButton();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.Transparent;
            panelButtons.Controls.Add(btnClose);
            panelButtons.Controls.Add(btnMaximize);
            panelButtons.Controls.Add(btnMinimize);
            panelButtons.Dock = DockStyle.Right;
            panelButtons.Location = new Point(130, 10);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(60, 21);
            panelButtons.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.FillColor = Color.Red;
            btnClose.FillColor2 = Color.Red;
            btnClose.FillHoverColor = Color.Red;
            btnClose.FillPressColor = Color.Red;
            btnClose.FillSelectedColor = Color.Red;
            btnClose.Font = new Font("Microsoft Sans Serif", 12F);
            btnClose.Location = new Point(45, 0);
            btnClose.MinimumSize = new Size(1, 1);
            btnClose.Name = "btnClose";
            btnClose.Radius = 15;
            btnClose.RectColor = Color.Red;
            btnClose.RectHoverColor = Color.Red;
            btnClose.RectPressColor = Color.Red;
            btnClose.RectSelectedColor = Color.Red;
            btnClose.Size = new Size(15, 15);
            btnClose.TabIndex = 0;
            btnClose.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // btnMaximize
            // 
            btnMaximize.FillColor = Color.Orange;
            btnMaximize.FillColor2 = Color.Orange;
            btnMaximize.FillHoverColor = Color.Orange;
            btnMaximize.FillPressColor = Color.Orange;
            btnMaximize.FillSelectedColor = Color.Orange;
            btnMaximize.Font = new Font("Microsoft Sans Serif", 12F);
            btnMaximize.Location = new Point(25, 0);
            btnMaximize.MinimumSize = new Size(1, 1);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Radius = 15;
            btnMaximize.RectColor = Color.Orange;
            btnMaximize.RectHoverColor = Color.Orange;
            btnMaximize.RectPressColor = Color.Orange;
            btnMaximize.RectSelectedColor = Color.Orange;
            btnMaximize.Size = new Size(15, 15);
            btnMaximize.TabIndex = 1;
            btnMaximize.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // btnMinimize
            // 
            btnMinimize.FillColor = Color.Green;
            btnMinimize.FillColor2 = Color.Green;
            btnMinimize.FillHoverColor = Color.Green;
            btnMinimize.FillPressColor = Color.Green;
            btnMinimize.FillSelectedColor = Color.Green;
            btnMinimize.Font = new Font("Microsoft Sans Serif", 12F);
            btnMinimize.Location = new Point(5, 0);
            btnMinimize.MinimumSize = new Size(1, 1);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Radius = 15;
            btnMinimize.RectColor = Color.Green;
            btnMinimize.RectHoverColor = Color.Green;
            btnMinimize.RectPressColor = Color.Green;
            btnMinimize.RectSelectedColor = Color.Green;
            btnMinimize.Size = new Size(15, 15);
            btnMinimize.TabIndex = 2;
            btnMinimize.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // PopupBoxControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(panelButtons);
            Name = "PopupBoxControl";
            Padding = new Padding(0, 10, 10, 0);
            Size = new Size(200, 31);
            Load += PopupBoxControl_Load;
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIButton btnMinimize;
        private Sunny.UI.UIButton btnMaximize;
        private Sunny.UI.UIButton btnClose;
        private Panel panelButtons;
    }
}
