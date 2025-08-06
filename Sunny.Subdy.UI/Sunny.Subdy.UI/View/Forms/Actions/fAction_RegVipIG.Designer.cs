namespace Sunny.Subdy.UI.View.Forms.Actions
{
    partial class fAction_RegVipIG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAction_RegVipIG));
            label1 = new System.Windows.Forms.Label();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            btnStart = new System.Windows.Forms.Button();
            btnStop = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(44, 30);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(109, 15);
            label1.TabIndex = 0;
            label1.Text = "Số lượng tài khoản:";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new System.Drawing.Point(159, 28);
            numericUpDown1.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(74, 23);
            numericUpDown1.TabIndex = 1;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnStart
            // 
            btnStart.BackColor = System.Drawing.Color.Green;
            btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnStart.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Bold);
            btnStart.ForeColor = System.Drawing.Color.White;
            btnStart.Location = new System.Drawing.Point(163, 191);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(106, 28);
            btnStart.TabIndex = 194;
            btnStart.Text = "Bắt đầu";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = System.Drawing.Color.Crimson;
            btnStop.Enabled = false;
            btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnStop.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Bold);
            btnStop.ForeColor = System.Drawing.Color.White;
            btnStop.Location = new System.Drawing.Point(300, 191);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(106, 28);
            btnStop.TabIndex = 195;
            btnStop.Text = "Đóng";
            btnStop.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(44, 67);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(99, 15);
            label2.TabIndex = 196;
            label2.Text = "Key gurucaptcha:";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(159, 64);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(337, 23);
            textBox1.TabIndex = 197;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(44, 103);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(60, 15);
            label3.TabIndex = 198;
            label3.Text = "Mật khẩu:";
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(159, 103);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(337, 23);
            textBox2.TabIndex = 199;
            textBox2.Text = "LamTool.net";
            // 
            // fAction_RegVipIG
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(534, 238);
            Controls.Add(textBox2);
            Controls.Add(label3);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(numericUpDown1);
            Controls.Add(label1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "fAction_RegVipIG";
            Text = "Cấu hình đăng kí tài khoản vipig.net";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
    }
}