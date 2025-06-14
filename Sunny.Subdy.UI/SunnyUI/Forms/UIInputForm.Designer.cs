namespace Sunny.UI
{
    partial class UIInputForm
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
            label = new UILabel();
            edit = new UITextBox();
            pnlBtm.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBtm
            // 
            pnlBtm.BackColor = System.Drawing.Color.White;
            pnlBtm.FillColor = System.Drawing.Color.White;
            pnlBtm.FillColor2 = System.Drawing.Color.White;
            pnlBtm.FillDisableColor = System.Drawing.Color.White;
            pnlBtm.Location = new System.Drawing.Point(0, 156);
            pnlBtm.RectColor = System.Drawing.Color.White;
            pnlBtm.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom;
            pnlBtm.Size = new System.Drawing.Size(474, 55);
            pnlBtm.Style = UIStyle.Custom;
            pnlBtm.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(247, 4);
            btnCancel.Style = UIStyle.Custom;
            // 
            // btnOK
            // 
            btnOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnOK.Location = new System.Drawing.Point(111, 4);
            btnOK.Style = UIStyle.Custom;
            btnOK.Text = "Thêm";
            // 
            // label
            // 
            label.AutoSize = true;
            label.BackColor = System.Drawing.Color.Transparent;
            label.Font = new System.Drawing.Font("Segoe UI", 9F);
            label.ForeColor = System.Drawing.Color.FromArgb(48, 48, 48);
            label.Location = new System.Drawing.Point(28, 57);
            label.Name = "label";
            label.Size = new System.Drawing.Size(84, 15);
            label.Style = UIStyle.Custom;
            label.TabIndex = 1;
            label.Text = "Vui lòng nhập:";
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // edit
            // 
            edit.Cursor = System.Windows.Forms.Cursors.IBeam;
            edit.FillColor = System.Drawing.Color.White;
            edit.Font = new System.Drawing.Font("Segoe UI", 9F);
            edit.Location = new System.Drawing.Point(29, 87);
            edit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            edit.MinimumSize = new System.Drawing.Size(1, 1);
            edit.Name = "edit";
            edit.Padding = new System.Windows.Forms.Padding(5);
            edit.RadiusSides = UICornerRadiusSides.None;
            edit.ShowText = false;
            edit.Size = new System.Drawing.Size(415, 34);
            edit.Style = UIStyle.Custom;
            edit.TabIndex = 0;
            edit.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            edit.Watermark = "";
            // 
            // UIInputForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(474, 211);
            Controls.Add(edit);
            Controls.Add(label);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            Name = "UIInputForm";
            Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            RectColor = System.Drawing.Color.FromArgb(4, 60, 44);
            Style = UIStyle.Custom;
            Text = "LamTool.net";
            TextAlignment = System.Drawing.StringAlignment.Center;
            TitleColor = System.Drawing.Color.FromArgb(4, 60, 44);
            TitleFont = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 473, 182);
            Shown += UIInputForm_Shown;
            Controls.SetChildIndex(pnlBtm, 0);
            Controls.SetChildIndex(label, 0);
            Controls.SetChildIndex(edit, 0);
            pnlBtm.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private UILabel label;
        private UITextBox edit;
    }
}