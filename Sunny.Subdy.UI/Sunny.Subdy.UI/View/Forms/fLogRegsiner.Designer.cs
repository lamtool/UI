namespace Sunny.Subdy.UI.View.Forms
{
    partial class fLogRegsiner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fLogRegsiner));
            flowLayoutPanel1 = new FlowLayoutPanel();
            uiLabel2 = new Sunny.UI.UILabel();
            uiLabel1 = new Sunny.UI.UILabel();
            uiLabel3 = new Sunny.UI.UILabel();
            uiLabel4 = new Sunny.UI.UILabel();
            uiLabel5 = new Sunny.UI.UILabel();
            uiLabel6 = new Sunny.UI.UILabel();
            tableLayoutPanel1 = new TableLayoutPanel();
            listView_DIE = new ListView();
            listView_LIVE = new ListView();
            timer1 = new System.Windows.Forms.Timer(components);
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(uiLabel2);
            flowLayoutPanel1.Controls.Add(uiLabel1);
            flowLayoutPanel1.Controls.Add(uiLabel3);
            flowLayoutPanel1.Controls.Add(uiLabel4);
            flowLayoutPanel1.Controls.Add(uiLabel5);
            flowLayoutPanel1.Controls.Add(uiLabel6);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(10);
            flowLayoutPanel1.Size = new Size(890, 33);
            flowLayoutPanel1.TabIndex = 15;
            // 
            // uiLabel2
            // 
            uiLabel2.AutoSize = true;
            uiLabel2.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiLabel2.ForeColor = Color.DimGray;
            uiLabel2.Location = new Point(13, 10);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(37, 15);
            uiLabel2.TabIndex = 2;
            uiLabel2.Text = "Total:";
            uiLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel1
            // 
            uiLabel1.AutoSize = true;
            uiLabel1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiLabel1.ForeColor = Color.Blue;
            uiLabel1.Location = new Point(56, 10);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(14, 15);
            uiLabel1.TabIndex = 3;
            uiLabel1.Text = "0";
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            uiLabel3.AutoSize = true;
            uiLabel3.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiLabel3.ForeColor = Color.DimGray;
            uiLabel3.Location = new Point(76, 10);
            uiLabel3.Name = "uiLabel3";
            uiLabel3.Size = new Size(32, 15);
            uiLabel3.TabIndex = 4;
            uiLabel3.Text = "Live:";
            uiLabel3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel4
            // 
            uiLabel4.AutoSize = true;
            uiLabel4.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiLabel4.ForeColor = Color.Green;
            uiLabel4.Location = new Point(114, 10);
            uiLabel4.Name = "uiLabel4";
            uiLabel4.Size = new Size(14, 15);
            uiLabel4.TabIndex = 5;
            uiLabel4.Text = "0";
            uiLabel4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel5
            // 
            uiLabel5.AutoSize = true;
            uiLabel5.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiLabel5.ForeColor = Color.DimGray;
            uiLabel5.Location = new Point(134, 10);
            uiLabel5.Name = "uiLabel5";
            uiLabel5.Size = new Size(29, 15);
            uiLabel5.TabIndex = 6;
            uiLabel5.Text = "Die:";
            uiLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel6
            // 
            uiLabel6.AutoSize = true;
            uiLabel6.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiLabel6.ForeColor = Color.Red;
            uiLabel6.Location = new Point(169, 10);
            uiLabel6.Name = "uiLabel6";
            uiLabel6.Size = new Size(14, 15);
            uiLabel6.TabIndex = 7;
            uiLabel6.Text = "0";
            uiLabel6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(listView_DIE, 1, 0);
            tableLayoutPanel1.Controls.Add(listView_LIVE, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 33);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(890, 456);
            tableLayoutPanel1.TabIndex = 16;
            // 
            // listView_DIE
            // 
            listView_DIE.Dock = DockStyle.Fill;
            listView_DIE.ForeColor = Color.OrangeRed;
            listView_DIE.Location = new Point(448, 3);
            listView_DIE.Name = "listView_DIE";
            listView_DIE.Size = new Size(439, 450);
            listView_DIE.TabIndex = 1;
            listView_DIE.UseCompatibleStateImageBehavior = false;
            listView_DIE.View = System.Windows.Forms.View.List;
            // 
            // listView_LIVE
            // 
            listView_LIVE.Dock = DockStyle.Fill;
            listView_LIVE.ForeColor = Color.Green;
            listView_LIVE.Location = new Point(3, 3);
            listView_LIVE.Name = "listView_LIVE";
            listView_LIVE.Size = new Size(439, 450);
            listView_LIVE.TabIndex = 0;
            listView_LIVE.UseCompatibleStateImageBehavior = false;
            listView_LIVE.View = System.Windows.Forms.View.List;
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // fLogRegsiner
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(890, 489);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(flowLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "fLogRegsiner";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Log đăng kí tài khoản facebook";
            Load += fLogRegsiner_Load;
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UILabel uiLabel6;
        private TableLayoutPanel tableLayoutPanel1;
        private ListView listView_DIE;
        private ListView listView_LIVE;
        private System.Windows.Forms.Timer timer1;
    }
}