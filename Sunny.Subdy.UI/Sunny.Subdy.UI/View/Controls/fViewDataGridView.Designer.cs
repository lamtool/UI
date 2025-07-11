using System.Drawing;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Controls
{
    partial class fViewDataGridView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fViewDataGridView));
            label1 = new Label();
            btn_Save = new Button();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(693, 27);
            label1.TabIndex = 1;
            label1.Text = "Vui lòng chọn những cột cần hiển thị";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn_Save
            // 
            btn_Save.Location = new Point(266, 4);
            btn_Save.Name = "btn_Save";
            btn_Save.Size = new Size(95, 23);
            btn_Save.TabIndex = 16;
            btn_Save.Text = "Lưu";
            btn_Save.UseVisualStyleBackColor = true;
            btn_Save.Click += btn_Save_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(btn_Save);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 141);
            panel1.Name = "panel1";
            panel1.Size = new Size(693, 32);
            panel1.TabIndex = 17;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 27);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(693, 114);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // fViewDataGridView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(693, 173);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fViewDataGridView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cấu Hình Hiển Thị";
            Load += FormViewDataGridView_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private CheckBox cb_JobToday;
        private CheckBox cb_JobTotal;
        private CheckBox checkBox13;
        private CheckBox checkBox14;
        private Button btn_Save;
        private Panel panel1;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}