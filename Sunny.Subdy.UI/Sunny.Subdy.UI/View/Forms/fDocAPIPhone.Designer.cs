namespace Sunny.Subdy.UI.View.Forms
{
    partial class fDocAPIPhone
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
            label1 = new Label();
            textBox1 = new TextBox();
            uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            pictureBox1 = new PictureBox();
            textBox2 = new TextBox();
            uiTitlePanel2 = new Sunny.UI.UITitlePanel();
            pictureBox2 = new PictureBox();
            textBox3 = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            uiTitlePanel3 = new Sunny.UI.UITitlePanel();
            textBox5 = new TextBox();
            pictureBox3 = new PictureBox();
            textBox4 = new TextBox();
            uiTitlePanel4 = new Sunny.UI.UITitlePanel();
            textBox6 = new TextBox();
            pictureBox4 = new PictureBox();
            textBox7 = new TextBox();
            uiTitlePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            uiTitlePanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            uiTitlePanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            uiTitlePanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 23);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 0;
            label1.Text = "domain:";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.Blue;
            textBox1.Location = new Point(86, 10);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(247, 33);
            textBox1.TabIndex = 1;
            textBox1.Text = "http://localhost:8686";
            // 
            // uiTitlePanel1
            // 
            uiTitlePanel1.Collapsed = true;
            uiTitlePanel1.Controls.Add(pictureBox1);
            uiTitlePanel1.Controls.Add(textBox2);
            uiTitlePanel1.Font = new Font("Microsoft Sans Serif", 12F);
            uiTitlePanel1.Location = new Point(4, 5);
            uiTitlePanel1.Margin = new Padding(4, 5, 4, 5);
            uiTitlePanel1.MinimumSize = new Size(1, 1);
            uiTitlePanel1.Name = "uiTitlePanel1";
            uiTitlePanel1.ShowCollapse = true;
            uiTitlePanel1.ShowText = false;
            uiTitlePanel1.Size = new Size(424, 35);
            uiTitlePanel1.TabIndex = 3;
            uiTitlePanel1.Text = "Lấy danh sách devices";
            uiTitlePanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.docGetDevice;
            pictureBox1.Location = new Point(3, 76);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(418, 341);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // textBox2
            // 
            textBox2.ForeColor = Color.Blue;
            textBox2.Location = new Point(3, 44);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(418, 26);
            textBox2.TabIndex = 2;
            textBox2.Text = "GET: http://localhost:8686/devices";
            // 
            // uiTitlePanel2
            // 
            uiTitlePanel2.Collapsed = true;
            uiTitlePanel2.Controls.Add(pictureBox2);
            uiTitlePanel2.Controls.Add(textBox3);
            uiTitlePanel2.Font = new Font("Microsoft Sans Serif", 12F);
            uiTitlePanel2.Location = new Point(4, 50);
            uiTitlePanel2.Margin = new Padding(4, 5, 4, 5);
            uiTitlePanel2.MinimumSize = new Size(1, 1);
            uiTitlePanel2.Name = "uiTitlePanel2";
            uiTitlePanel2.ShowCollapse = true;
            uiTitlePanel2.ShowText = false;
            uiTitlePanel2.Size = new Size(424, 35);
            uiTitlePanel2.TabIndex = 4;
            uiTitlePanel2.Text = "Thay đổi thông tin thiết bị";
            uiTitlePanel2.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.docChangeDevicePhone;
            pictureBox2.Location = new Point(0, 73);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(424, 184);
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // textBox3
            // 
            textBox3.ForeColor = Color.Blue;
            textBox3.Location = new Point(3, 44);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(418, 26);
            textBox3.TabIndex = 2;
            textBox3.Text = "GET: http://localhost:8686/{deviceId}/Change";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(uiTitlePanel1);
            flowLayoutPanel1.Controls.Add(uiTitlePanel2);
            flowLayoutPanel1.Controls.Add(uiTitlePanel3);
            flowLayoutPanel1.Controls.Add(uiTitlePanel4);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(12, 49);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.RightToLeft = RightToLeft.No;
            flowLayoutPanel1.Size = new Size(983, 462);
            flowLayoutPanel1.TabIndex = 5;
            flowLayoutPanel1.WrapContents = false;
            // 
            // uiTitlePanel3
            // 
            uiTitlePanel3.Collapsed = true;
            uiTitlePanel3.Controls.Add(textBox5);
            uiTitlePanel3.Controls.Add(pictureBox3);
            uiTitlePanel3.Controls.Add(textBox4);
            uiTitlePanel3.Font = new Font("Microsoft Sans Serif", 12F);
            uiTitlePanel3.Location = new Point(4, 95);
            uiTitlePanel3.Margin = new Padding(4, 5, 4, 5);
            uiTitlePanel3.MinimumSize = new Size(1, 1);
            uiTitlePanel3.Name = "uiTitlePanel3";
            uiTitlePanel3.ShowCollapse = true;
            uiTitlePanel3.ShowText = false;
            uiTitlePanel3.Size = new Size(424, 35);
            uiTitlePanel3.TabIndex = 5;
            uiTitlePanel3.Text = "Backup facebook";
            uiTitlePanel3.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // textBox5
            // 
            textBox5.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox5.ForeColor = Color.Blue;
            textBox5.Location = new Point(3, 76);
            textBox5.Multiline = true;
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(418, 57);
            textBox5.TabIndex = 4;
            textBox5.Text = "--data '{\r\n    \"file\": \"C:\\\\Users\\\\LAMTOOL\\\\Documents\\\\facebook.tar.gz\"\r\n}'";
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.docBackDevice;
            pictureBox3.Location = new Point(0, 139);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(421, 118);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            // 
            // textBox4
            // 
            textBox4.ForeColor = Color.Blue;
            textBox4.Location = new Point(3, 44);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(418, 26);
            textBox4.TabIndex = 2;
            textBox4.Text = "POST: http://localhost:8686/{deviceId}/facebook/backup";
            // 
            // uiTitlePanel4
            // 
            uiTitlePanel4.Controls.Add(textBox6);
            uiTitlePanel4.Controls.Add(pictureBox4);
            uiTitlePanel4.Controls.Add(textBox7);
            uiTitlePanel4.Font = new Font("Microsoft Sans Serif", 12F);
            uiTitlePanel4.Location = new Point(4, 140);
            uiTitlePanel4.Margin = new Padding(4, 5, 4, 5);
            uiTitlePanel4.MinimumSize = new Size(1, 1);
            uiTitlePanel4.Name = "uiTitlePanel4";
            uiTitlePanel4.ShowCollapse = true;
            uiTitlePanel4.ShowText = false;
            uiTitlePanel4.Size = new Size(424, 260);
            uiTitlePanel4.TabIndex = 6;
            uiTitlePanel4.Text = "Restore facebook";
            uiTitlePanel4.TextAlignment = ContentAlignment.MiddleCenter;
            uiTitlePanel4.Click += uiTitlePanel4_Click;
            // 
            // textBox6
            // 
            textBox6.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox6.ForeColor = Color.Blue;
            textBox6.Location = new Point(3, 76);
            textBox6.Multiline = true;
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(418, 57);
            textBox6.TabIndex = 4;
            textBox6.Text = "--data '{\r\n    \"file\": \"C:\\\\Users\\\\LAMTOOL\\\\Documents\\\\facebook.tar.gz\"\r\n}'";
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.docRestoreDevice;
            pictureBox4.Location = new Point(0, 139);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(421, 118);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 3;
            pictureBox4.TabStop = false;
            // 
            // textBox7
            // 
            textBox7.ForeColor = Color.Blue;
            textBox7.Location = new Point(3, 44);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new Size(418, 26);
            textBox7.TabIndex = 2;
            textBox7.Text = "POST: http://localhost:8686/{deviceId}/facebook/backup";
            // 
            // fDocAPIPhone
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1007, 523);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fDocAPIPhone";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Load += fDocAPIPhone_Load;
            uiTitlePanel1.ResumeLayout(false);
            uiTitlePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            uiTitlePanel2.ResumeLayout(false);
            uiTitlePanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            uiTitlePanel3.ResumeLayout(false);
            uiTitlePanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            uiTitlePanel4.ResumeLayout(false);
            uiTitlePanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private PictureBox pictureBox1;
        private TextBox textBox2;
        private Sunny.UI.UITitlePanel uiTitlePanel2;
        private PictureBox pictureBox2;
        private TextBox textBox3;
        private FlowLayoutPanel flowLayoutPanel1;
        private Sunny.UI.UITitlePanel uiTitlePanel3;
        private PictureBox pictureBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private Sunny.UI.UITitlePanel uiTitlePanel4;
        private TextBox textBox6;
        private PictureBox pictureBox4;
        private TextBox textBox7;
    }
}