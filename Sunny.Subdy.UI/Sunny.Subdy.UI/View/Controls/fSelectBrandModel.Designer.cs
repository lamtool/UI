namespace Sunny.Subdy.UI.View.Controls
{
    partial class fSelectBrandModel
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fSelectBrandModel));
            uiLabel1 = new Sunny.UI.UILabel();
            dgvDevices = new DataGridView();
            colCheckbox_Device = new DataGridViewCheckBoxColumn();
            tIndex = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            checkBox1 = new CheckBox();
            uiSymbolButton3 = new Sunny.UI.UISymbolButton();
            uiSymbolButton4 = new Sunny.UI.UISymbolButton();
            ((System.ComponentModel.ISupportInitialize)dgvDevices).BeginInit();
            SuspendLayout();
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("Microsoft Sans Serif", 12F);
            uiLabel1.ForeColor = Color.Blue;
            uiLabel1.Location = new Point(34, 52);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(288, 24);
            uiLabel1.TabIndex = 0;
            uiLabel1.Text = "Đã chọn 38/38";
            // 
            // dgvDevices
            // 
            dgvDevices.AllowUserToAddRows = false;
            dgvDevices.AllowUserToDeleteRows = false;
            dgvDevices.AllowUserToResizeRows = false;
            dgvDevices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvDevices.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 9.75F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.Teal;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDevices.Columns.AddRange(new DataGridViewColumn[] { colCheckbox_Device, tIndex, colStatus });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDevices.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDevices.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvDevices.Location = new Point(18, 79);
            dgvDevices.Name = "dgvDevices";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvDevices.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvDevices.RowHeadersVisible = false;
            dgvDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDevices.Size = new Size(316, 328);
            dgvDevices.TabIndex = 7;
            // 
            // colCheckbox_Device
            // 
            colCheckbox_Device.FillWeight = 50F;
            colCheckbox_Device.HeaderText = "";
            colCheckbox_Device.MinimumWidth = 50;
            colCheckbox_Device.Name = "colCheckbox_Device";
            colCheckbox_Device.Width = 50;
            // 
            // tIndex
            // 
            tIndex.HeaderText = "#";
            tIndex.Name = "tIndex";
            tIndex.ReadOnly = true;
            tIndex.Width = 41;
            // 
            // colStatus
            // 
            colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colStatus.HeaderText = "Brand";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(37, 85);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(15, 14);
            checkBox1.TabIndex = 8;
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // uiSymbolButton3
            // 
            uiSymbolButton3.Anchor = AnchorStyles.Right;
            uiSymbolButton3.FillColor = Color.Green;
            uiSymbolButton3.FillColor2 = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.FillHoverColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.FillPressColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.FillSelectedColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiSymbolButton3.Location = new Point(28, 442);
            uiSymbolButton3.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton3.MinimumSize = new Size(1, 1);
            uiSymbolButton3.Name = "uiSymbolButton3";
            uiSymbolButton3.Radius = 15;
            uiSymbolButton3.RectColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.RectHoverColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.RectPressColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.RectSelectedColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.Size = new Size(128, 29);
            uiSymbolButton3.Symbol = 557697;
            uiSymbolButton3.SymbolSize = 18;
            uiSymbolButton3.TabIndex = 15;
            uiSymbolButton3.Text = "Lưu";
            uiSymbolButton3.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton3.Click += uiSymbolButton3_Click;
            // 
            // uiSymbolButton4
            // 
            uiSymbolButton4.Anchor = AnchorStyles.Right;
            uiSymbolButton4.FillColor = Color.Red;
            uiSymbolButton4.FillColor2 = Color.DarkRed;
            uiSymbolButton4.FillHoverColor = Color.DarkRed;
            uiSymbolButton4.FillPressColor = Color.DarkRed;
            uiSymbolButton4.FillSelectedColor = Color.DarkRed;
            uiSymbolButton4.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiSymbolButton4.Location = new Point(176, 442);
            uiSymbolButton4.Margin = new Padding(10, 3, 3, 3);
            uiSymbolButton4.MinimumSize = new Size(1, 1);
            uiSymbolButton4.Name = "uiSymbolButton4";
            uiSymbolButton4.Radius = 15;
            uiSymbolButton4.RectColor = Color.DarkRed;
            uiSymbolButton4.RectHoverColor = Color.DarkRed;
            uiSymbolButton4.RectPressColor = Color.DarkRed;
            uiSymbolButton4.RectSelectedColor = Color.DarkRed;
            uiSymbolButton4.Size = new Size(128, 29);
            uiSymbolButton4.Symbol = 61453;
            uiSymbolButton4.SymbolSize = 17;
            uiSymbolButton4.TabIndex = 14;
            uiSymbolButton4.Text = "Dừng";
            uiSymbolButton4.TipsColor = Color.DarkRed;
            uiSymbolButton4.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton4.Click += uiSymbolButton4_Click;
            // 
            // fSelectBrandModel
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            ClientSize = new Size(352, 489);
            Controls.Add(uiSymbolButton3);
            Controls.Add(uiSymbolButton4);
            Controls.Add(checkBox1);
            Controls.Add(dgvDevices);
            Controls.Add(uiLabel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fSelectBrandModel";
            Padding = new Padding(15);
            Text = "Chọn danh sách brand device";
            TitleFont = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ZoomScaleRect = new Rectangle(15, 15, 320, 511);
            ((System.ComponentModel.ISupportInitialize)dgvDevices).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
        public DataGridView dgvDevices;
        private DataGridViewCheckBoxColumn colCheckbox_Device;
        private DataGridViewTextBoxColumn tIndex;
        private DataGridViewTextBoxColumn colStatus;
        private CheckBox checkBox1;
        private Sunny.UI.UISymbolButton uiSymbolButton3;
        private Sunny.UI.UISymbolButton uiSymbolButton4;
    }
}