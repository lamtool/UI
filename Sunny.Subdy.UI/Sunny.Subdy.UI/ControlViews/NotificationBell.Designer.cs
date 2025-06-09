using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.ControlViews
{
    partial class NotificationBell
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
            bellButton = new UIHeaderButton();
            badgeLabel = new Label();
            SuspendLayout();
            // 
            // bellButton
            // 
            bellButton.CircleColor = Color.Transparent;
            bellButton.CircleDisabledColor = Color.Olive;
            bellButton.CircleHoverColor = Color.Transparent;
            bellButton.CircleSize = 30;
            bellButton.FillColor = Color.Transparent;
            bellButton.FillDisableColor = Color.Transparent;
            bellButton.FillHoverColor = Color.Olive;
            bellButton.FillPressColor = Color.Transparent;
            bellButton.FillSelectedColor = Color.Transparent;
            bellButton.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bellButton.Location = new Point(115, 0);
            bellButton.MinimumSize = new Size(1, 1);
            bellButton.Name = "bellButton";
            bellButton.Padding = new Padding(0, 8, 0, 3);
            bellButton.Radius = 0;
            bellButton.RadiusSides = UICornerRadiusSides.LeftTop;
            bellButton.RectSides = ToolStripStatusLabelBorderSides.None;
            bellButton.Size = new Size(35, 40);
            bellButton.Symbol = 361683;
            bellButton.SymbolColor = Color.Gold;
            bellButton.SymbolSize = 30;
            bellButton.TabIndex = 3;
            bellButton.TipsColor = Color.White;
            bellButton.TipsFont = new Font("Microsoft Sans Serif", 9F);
            bellButton.MouseEnter += BellButton_MouseEnter;
            bellButton.MouseLeave += BellButton_MouseLeave;
            // 
            // badgeLabel
            // 
            badgeLabel.AutoSize = true;
            badgeLabel.ForeColor = Color.Red;
            badgeLabel.Location = new Point(3, 0);
            badgeLabel.Name = "badgeLabel";
            badgeLabel.Size = new Size(0, 15);
            badgeLabel.TabIndex = 4;
            // 
            // NotificationBell
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(bellButton);
            Controls.Add(badgeLabel);
            Name = "NotificationBell";
            Size = new Size(150, 40);
            ResumeLayout(false);
            PerformLayout();
        }
        internal UIHeaderButton bellButton;
        private Label badgeLabel;
        #endregion
    }
}
