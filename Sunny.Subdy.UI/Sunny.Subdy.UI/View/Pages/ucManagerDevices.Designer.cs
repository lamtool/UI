namespace Sunny.Subdy.UI.View.Pages
{
    partial class ucManagerDevices
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private bool isDraggingPanel = false;
        private int panel1OriginalWidth;
        private Point lastMousePosition;
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            panel1 = new Panel();
            panel2 = new Panel();
            uiDataGridView2 = new Sunny.UI.UIDataGridView();
            dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            uiContextMenuStrip1 = new Sunny.UI.UIContextMenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            tấtCảToolStripMenuItem = new ToolStripMenuItem();
            bôiĐenToolStripMenuItem = new ToolStripMenuItem();
            statusToolStripMenuItem = new ToolStripMenuItem();
            bỏChọnTấtCảToolStripMenuItem = new ToolStripMenuItem();
            tắtViewToolStripMenuItem = new ToolStripMenuItem();
            tắtToolStripMenuItem = new ToolStripMenuItem();
            mởToolStripMenuItem1 = new ToolStripMenuItem();
            mởToolStripMenuItem = new ToolStripMenuItem();
            càiĐặtApkToolStripMenuItem = new ToolStripMenuItem();
            wifiToolStripMenuItem = new ToolStripMenuItem();
            bậtWifiToolStripMenuItem = new ToolStripMenuItem();
            tắtWifiToolStripMenuItem = new ToolStripMenuItem();
            kếtNốiWifiToolStripMenuItem = new ToolStripMenuItem();
            gỡCàiĐặtPackageToolStripMenuItem = new ToolStripMenuItem();
            rebootToolStripMenuItem = new ToolStripMenuItem();
            changeInfoToolStripMenuItem = new ToolStripMenuItem();
            ứngDụngToolStripMenuItem = new ToolStripMenuItem();
            facebookToolStripMenuItem = new ToolStripMenuItem();
            backupToolStripMenuItem = new ToolStripMenuItem();
            restoreToolStripMenuItem = new ToolStripMenuItem();
            tikTokToolStripMenuItem = new ToolStripMenuItem();
            backupToolStripMenuItem1 = new ToolStripMenuItem();
            restoreToolStripMenuItem1 = new ToolStripMenuItem();
            instagramToolStripMenuItem = new ToolStripMenuItem();
            backupToolStripMenuItem2 = new ToolStripMenuItem();
            restoreToolStripMenuItem2 = new ToolStripMenuItem();
            connectToolStripMenuItem = new ToolStripMenuItem();
            disconnectToolStripMenuItem = new ToolStripMenuItem();
            dDToolStripMenuItem = new ToolStripMenuItem();
            facebookToolStripMenuItem1 = new ToolStripMenuItem();
            deviceModelBindingSource = new BindingSource(components);
            groupBox2 = new GroupBox();
            uiSymbolButton3 = new Sunny.UI.UISymbolButton();
            uiSymbolButton4 = new Sunny.UI.UISymbolButton();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            toolStripLabel2 = new ToolStripLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            toolStripLabel4 = new ToolStripLabel();
            uiPanel1 = new Sunny.UI.UIPanel();
            uiLinkLabel1 = new Sunny.UI.UILinkLabel();
            uiTextBox1 = new Sunny.UI.UITextBox();
            uiHeaderButton1 = new Sunny.UI.UIHeaderButton();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            groupBox1 = new GroupBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView2).BeginInit();
            uiContextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)deviceModelBindingSource).BeginInit();
            groupBox2.SuspendLayout();
            toolStrip1.SuspendLayout();
            uiPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(245, 245, 245);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(toolStrip1);
            panel1.Controls.Add(uiPanel1);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(630, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(500, 655);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(uiDataGridView2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 133);
            panel2.Name = "panel2";
            panel2.Size = new Size(500, 420);
            panel2.TabIndex = 10;
            // 
            // uiDataGridView2
            // 
            uiDataGridView2.AllowUserToAddRows = false;
            uiDataGridView2.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            uiDataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            uiDataGridView2.AutoGenerateColumns = false;
            uiDataGridView2.BackgroundColor = Color.White;
            uiDataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(4, 60, 44);
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(4, 60, 44);
            dataGridViewCellStyle2.SelectionForeColor = Color.DimGray;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            uiDataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            uiDataGridView2.ColumnHeadersHeight = 20;
            uiDataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            uiDataGridView2.Columns.AddRange(new DataGridViewColumn[] { dataGridViewCheckBoxColumn1, dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7 });
            uiDataGridView2.ContextMenuStrip = uiContextMenuStrip1;
            uiDataGridView2.DataSource = deviceModelBindingSource;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(220, 236, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            uiDataGridView2.DefaultCellStyle = dataGridViewCellStyle3;
            uiDataGridView2.Dock = DockStyle.Fill;
            uiDataGridView2.EnableHeadersVisualStyles = false;
            uiDataGridView2.Font = new Font("Microsoft Sans Serif", 12F);
            uiDataGridView2.GridColor = Color.DimGray;
            uiDataGridView2.Location = new Point(0, 0);
            uiDataGridView2.Name = "uiDataGridView2";
            uiDataGridView2.RectColor = Color.FromArgb(4, 60, 44);
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(243, 249, 255);
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            uiDataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            uiDataGridView2.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(220, 236, 255);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(48, 48, 48);
            uiDataGridView2.RowsDefaultCellStyle = dataGridViewCellStyle5;
            uiDataGridView2.ScrollBarBackColor = Color.FromArgb(4, 60, 44);
            uiDataGridView2.ScrollBarColor = Color.FromArgb(4, 60, 44);
            uiDataGridView2.ScrollBarRectColor = Color.FromArgb(4, 60, 44);
            uiDataGridView2.ScrollBarStyleInherited = false;
            uiDataGridView2.SelectedIndex = -1;
            uiDataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            uiDataGridView2.Size = new Size(500, 420);
            uiDataGridView2.StripeOddColor = Color.White;
            uiDataGridView2.TabIndex = 7;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            dataGridViewCheckBoxColumn1.DataPropertyName = "Check";
            dataGridViewCheckBoxColumn1.FillWeight = 50F;
            dataGridViewCheckBoxColumn1.HeaderText = "Chọn";
            dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.DataPropertyName = "Index";
            dataGridViewTextBoxColumn1.FillWeight = 40F;
            dataGridViewTextBoxColumn1.HeaderText = "#";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.DataPropertyName = "Serial";
            dataGridViewTextBoxColumn2.HeaderText = "Serial ";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.DataPropertyName = "NameDevice";
            dataGridViewTextBoxColumn3.HeaderText = "Name";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.DataPropertyName = "OS";
            dataGridViewTextBoxColumn4.FillWeight = 50F;
            dataGridViewTextBoxColumn4.HeaderText = "OS";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Width = 50;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn5.DataPropertyName = "Status";
            dataGridViewTextBoxColumn5.HeaderText = "Status";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.DataPropertyName = "Port";
            dataGridViewTextBoxColumn6.HeaderText = "Port";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.DataPropertyName = "TypeColor";
            dataGridViewTextBoxColumn7.HeaderText = "TypeColor";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.Visible = false;
            // 
            // uiContextMenuStrip1
            // 
            uiContextMenuStrip1.BackColor = Color.FromArgb(245, 245, 245);
            uiContextMenuStrip1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiContextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, bỏChọnTấtCảToolStripMenuItem, tắtViewToolStripMenuItem, mởToolStripMenuItem, ứngDụngToolStripMenuItem, connectToolStripMenuItem, disconnectToolStripMenuItem, dDToolStripMenuItem });
            uiContextMenuStrip1.Name = "uiContextMenuStrip1";
            uiContextMenuStrip1.Size = new Size(177, 180);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { tấtCảToolStripMenuItem, bôiĐenToolStripMenuItem, statusToolStripMenuItem });
            toolStripMenuItem1.ForeColor = SystemColors.ControlText;
            toolStripMenuItem1.Image = Properties.Resources.select_check_box_30dp;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(176, 22);
            toolStripMenuItem1.Text = "Chọn";
            // 
            // tấtCảToolStripMenuItem
            // 
            tấtCảToolStripMenuItem.Image = Properties.Resources.done_all_30;
            tấtCảToolStripMenuItem.Name = "tấtCảToolStripMenuItem";
            tấtCảToolStripMenuItem.Size = new Size(120, 22);
            tấtCảToolStripMenuItem.Text = "Tất cả";
            tấtCảToolStripMenuItem.Click += tấtCảToolStripMenuItem_Click;
            // 
            // bôiĐenToolStripMenuItem
            // 
            bôiĐenToolStripMenuItem.Image = Properties.Resources.checklist_30dp;
            bôiĐenToolStripMenuItem.Name = "bôiĐenToolStripMenuItem";
            bôiĐenToolStripMenuItem.Size = new Size(120, 22);
            bôiĐenToolStripMenuItem.Text = "Bôi đen";
            bôiĐenToolStripMenuItem.Click += bôiĐenToolStripMenuItem_Click;
            // 
            // statusToolStripMenuItem
            // 
            statusToolStripMenuItem.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusToolStripMenuItem.Image = Properties.Resources.playlist_add_check_30dp;
            statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            statusToolStripMenuItem.Size = new Size(120, 22);
            statusToolStripMenuItem.Text = "Status";
            // 
            // bỏChọnTấtCảToolStripMenuItem
            // 
            bỏChọnTấtCảToolStripMenuItem.Image = Properties.Resources.check_box_outline_blank_30dp;
            bỏChọnTấtCảToolStripMenuItem.Name = "bỏChọnTấtCảToolStripMenuItem";
            bỏChọnTấtCảToolStripMenuItem.Size = new Size(176, 22);
            bỏChọnTấtCảToolStripMenuItem.Text = "Bỏ chọn tất cả";
            bỏChọnTấtCảToolStripMenuItem.Click += bỏChọnTấtCảToolStripMenuItem_Click;
            // 
            // tắtViewToolStripMenuItem
            // 
            tắtViewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tắtToolStripMenuItem, mởToolStripMenuItem1 });
            tắtViewToolStripMenuItem.Image = Properties.Resources.mystery_30dp;
            tắtViewToolStripMenuItem.Name = "tắtViewToolStripMenuItem";
            tắtViewToolStripMenuItem.Size = new Size(176, 22);
            tắtViewToolStripMenuItem.Text = "View";
            // 
            // tắtToolStripMenuItem
            // 
            tắtToolStripMenuItem.Image = Properties.Resources.visibility_off_30dp;
            tắtToolStripMenuItem.Name = "tắtToolStripMenuItem";
            tắtToolStripMenuItem.Size = new Size(94, 22);
            tắtToolStripMenuItem.Text = "Tắt";
            tắtToolStripMenuItem.Click += tắtToolStripMenuItem_Click;
            // 
            // mởToolStripMenuItem1
            // 
            mởToolStripMenuItem1.Image = Properties.Resources.visibility_30dp;
            mởToolStripMenuItem1.Name = "mởToolStripMenuItem1";
            mởToolStripMenuItem1.Size = new Size(94, 22);
            mởToolStripMenuItem1.Text = "Mở";
            mởToolStripMenuItem1.Click += mởToolStripMenuItem1_Click;
            // 
            // mởToolStripMenuItem
            // 
            mởToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { càiĐặtApkToolStripMenuItem, wifiToolStripMenuItem, gỡCàiĐặtPackageToolStripMenuItem, rebootToolStripMenuItem, changeInfoToolStripMenuItem });
            mởToolStripMenuItem.Image = Properties.Resources.functions_30dp;
            mởToolStripMenuItem.Name = "mởToolStripMenuItem";
            mởToolStripMenuItem.Size = new Size(176, 22);
            mởToolStripMenuItem.Text = "Nâng cao";
            // 
            // càiĐặtApkToolStripMenuItem
            // 
            càiĐặtApkToolStripMenuItem.Image = Properties.Resources.apk_install_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            càiĐặtApkToolStripMenuItem.Name = "càiĐặtApkToolStripMenuItem";
            càiĐặtApkToolStripMenuItem.Size = new Size(192, 22);
            càiĐặtApkToolStripMenuItem.Text = "Cài đặt apk";
            càiĐặtApkToolStripMenuItem.Click += càiĐặtApkToolStripMenuItem_Click;
            // 
            // wifiToolStripMenuItem
            // 
            wifiToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { bậtWifiToolStripMenuItem, tắtWifiToolStripMenuItem, kếtNốiWifiToolStripMenuItem });
            wifiToolStripMenuItem.Image = Properties.Resources.wifi_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            wifiToolStripMenuItem.Name = "wifiToolStripMenuItem";
            wifiToolStripMenuItem.Size = new Size(192, 22);
            wifiToolStripMenuItem.Text = "Wifi";
            // 
            // bậtWifiToolStripMenuItem
            // 
            bậtWifiToolStripMenuItem.Image = Properties.Resources.signal_wifi_4_bar_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            bậtWifiToolStripMenuItem.Name = "bậtWifiToolStripMenuItem";
            bậtWifiToolStripMenuItem.Size = new Size(135, 22);
            bậtWifiToolStripMenuItem.Text = "Bật wifi";
            bậtWifiToolStripMenuItem.Click += bậtWifiToolStripMenuItem_Click;
            // 
            // tắtWifiToolStripMenuItem
            // 
            tắtWifiToolStripMenuItem.Image = Properties.Resources.wifi_off_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            tắtWifiToolStripMenuItem.Name = "tắtWifiToolStripMenuItem";
            tắtWifiToolStripMenuItem.Size = new Size(135, 22);
            tắtWifiToolStripMenuItem.Text = "Tắt wifi";
            tắtWifiToolStripMenuItem.Click += tắtWifiToolStripMenuItem_Click;
            // 
            // kếtNốiWifiToolStripMenuItem
            // 
            kếtNốiWifiToolStripMenuItem.Image = Properties.Resources.router_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            kếtNốiWifiToolStripMenuItem.Name = "kếtNốiWifiToolStripMenuItem";
            kếtNốiWifiToolStripMenuItem.Size = new Size(135, 22);
            kếtNốiWifiToolStripMenuItem.Text = "Kết nối wifi";
            kếtNốiWifiToolStripMenuItem.Click += kếtNốiWifiToolStripMenuItem_Click;
            // 
            // gỡCàiĐặtPackageToolStripMenuItem
            // 
            gỡCàiĐặtPackageToolStripMenuItem.Image = Properties.Resources.apk_document_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            gỡCàiĐặtPackageToolStripMenuItem.Name = "gỡCàiĐặtPackageToolStripMenuItem";
            gỡCàiĐặtPackageToolStripMenuItem.Size = new Size(192, 22);
            gỡCàiĐặtPackageToolStripMenuItem.Text = "Gỡ cài đặt package";
            gỡCàiĐặtPackageToolStripMenuItem.Click += gỡCàiĐặtPackageToolStripMenuItem_Click;
            // 
            // rebootToolStripMenuItem
            // 
            rebootToolStripMenuItem.Image = Properties.Resources.restart_alt_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            rebootToolStripMenuItem.Name = "rebootToolStripMenuItem";
            rebootToolStripMenuItem.Size = new Size(192, 22);
            rebootToolStripMenuItem.Text = "Reboot";
            rebootToolStripMenuItem.Click += rebootToolStripMenuItem_Click;
            // 
            // changeInfoToolStripMenuItem
            // 
            changeInfoToolStripMenuItem.Image = Properties.Resources.send_to_mobile_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            changeInfoToolStripMenuItem.Name = "changeInfoToolStripMenuItem";
            changeInfoToolStripMenuItem.Size = new Size(192, 22);
            changeInfoToolStripMenuItem.Text = "Change Info";
            changeInfoToolStripMenuItem.Click += changeInfoToolStripMenuItem_Click;
            // 
            // ứngDụngToolStripMenuItem
            // 
            ứngDụngToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { facebookToolStripMenuItem, tikTokToolStripMenuItem, instagramToolStripMenuItem });
            ứngDụngToolStripMenuItem.Image = Properties.Resources.apps_30dp_434343_FILL0_wght400_GRAD0_opsz24;
            ứngDụngToolStripMenuItem.Name = "ứngDụngToolStripMenuItem";
            ứngDụngToolStripMenuItem.Size = new Size(176, 22);
            ứngDụngToolStripMenuItem.Text = "Ứng dụng";
            // 
            // facebookToolStripMenuItem
            // 
            facebookToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backupToolStripMenuItem, restoreToolStripMenuItem });
            facebookToolStripMenuItem.Image = Properties.Resources.icons8_facebook_40;
            facebookToolStripMenuItem.Name = "facebookToolStripMenuItem";
            facebookToolStripMenuItem.Size = new Size(136, 22);
            facebookToolStripMenuItem.Text = "Facebook";
            // 
            // backupToolStripMenuItem
            // 
            backupToolStripMenuItem.Image = Properties.Resources.backup_40dp_1F1F1F_FILL0_wght400_GRAD0_opsz40;
            backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            backupToolStripMenuItem.Size = new Size(122, 22);
            backupToolStripMenuItem.Text = "Backup";
            backupToolStripMenuItem.Click += backupToolStripMenuItem_Click;
            // 
            // restoreToolStripMenuItem
            // 
            restoreToolStripMenuItem.Image = Properties.Resources.drive_folder_upload_40dp_1F1F1F_FILL0_wght400_GRAD0_opsz40;
            restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            restoreToolStripMenuItem.Size = new Size(122, 22);
            restoreToolStripMenuItem.Text = "Restore";
            restoreToolStripMenuItem.Click += restoreToolStripMenuItem_Click;
            // 
            // tikTokToolStripMenuItem
            // 
            tikTokToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backupToolStripMenuItem1, restoreToolStripMenuItem1 });
            tikTokToolStripMenuItem.Image = Properties.Resources.icons8_tiktok_40;
            tikTokToolStripMenuItem.Name = "tikTokToolStripMenuItem";
            tikTokToolStripMenuItem.Size = new Size(136, 22);
            tikTokToolStripMenuItem.Text = "TikTok";
            // 
            // backupToolStripMenuItem1
            // 
            backupToolStripMenuItem1.Image = Properties.Resources.backup_40dp_1F1F1F_FILL0_wght400_GRAD0_opsz40;
            backupToolStripMenuItem1.Name = "backupToolStripMenuItem1";
            backupToolStripMenuItem1.Size = new Size(122, 22);
            backupToolStripMenuItem1.Text = "Backup";
            backupToolStripMenuItem1.Click += backupToolStripMenuItem1_Click;
            // 
            // restoreToolStripMenuItem1
            // 
            restoreToolStripMenuItem1.Image = Properties.Resources.drive_folder_upload_40dp_1F1F1F_FILL0_wght400_GRAD0_opsz40;
            restoreToolStripMenuItem1.Name = "restoreToolStripMenuItem1";
            restoreToolStripMenuItem1.Size = new Size(122, 22);
            restoreToolStripMenuItem1.Text = "Restore";
            restoreToolStripMenuItem1.Click += restoreToolStripMenuItem1_Click;
            // 
            // instagramToolStripMenuItem
            // 
            instagramToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backupToolStripMenuItem2, restoreToolStripMenuItem2 });
            instagramToolStripMenuItem.Image = Properties.Resources.icons8_instagram_40;
            instagramToolStripMenuItem.Name = "instagramToolStripMenuItem";
            instagramToolStripMenuItem.Size = new Size(136, 22);
            instagramToolStripMenuItem.Text = "Instagram";
            // 
            // backupToolStripMenuItem2
            // 
            backupToolStripMenuItem2.Image = Properties.Resources.backup_40dp_1F1F1F_FILL0_wght400_GRAD0_opsz40;
            backupToolStripMenuItem2.Name = "backupToolStripMenuItem2";
            backupToolStripMenuItem2.Size = new Size(122, 22);
            backupToolStripMenuItem2.Text = "Backup";
            backupToolStripMenuItem2.Click += backupToolStripMenuItem2_Click;
            // 
            // restoreToolStripMenuItem2
            // 
            restoreToolStripMenuItem2.Image = Properties.Resources.drive_folder_upload_40dp_1F1F1F_FILL0_wght400_GRAD0_opsz40;
            restoreToolStripMenuItem2.Name = "restoreToolStripMenuItem2";
            restoreToolStripMenuItem2.Size = new Size(122, 22);
            restoreToolStripMenuItem2.Text = "Restore";
            restoreToolStripMenuItem2.Click += restoreToolStripMenuItem2_Click;
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Image = Properties.Resources.trip_origin_30dp_48752C_FILL0_wght400_GRAD0_opsz24;
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.Size = new Size(176, 22);
            connectToolStripMenuItem.Text = "Connect";
            connectToolStripMenuItem.Click += connectToolStripMenuItem_Click;
            // 
            // disconnectToolStripMenuItem
            // 
            disconnectToolStripMenuItem.Image = Properties.Resources.trip_origin_30dp_EA3323_FILL0_wght400_GRAD0_opsz24;
            disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            disconnectToolStripMenuItem.Size = new Size(176, 22);
            disconnectToolStripMenuItem.Text = "Disconnect";
            // 
            // dDToolStripMenuItem
            // 
            dDToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { facebookToolStripMenuItem1 });
            dDToolStripMenuItem.Image = Properties.Resources.app_registration_40dp_1F1F1F_FILL0_wght400_GRAD0_opsz40;
            dDToolStripMenuItem.Name = "dDToolStripMenuItem";
            dDToolStripMenuItem.Size = new Size(176, 22);
            dDToolStripMenuItem.Text = "Đăng kí tài khoản";
            // 
            // facebookToolStripMenuItem1
            // 
            facebookToolStripMenuItem1.Image = Properties.Resources.icons8_facebook_40;
            facebookToolStripMenuItem1.Name = "facebookToolStripMenuItem1";
            facebookToolStripMenuItem1.Size = new Size(136, 22);
            facebookToolStripMenuItem1.Text = "Facebook";
            facebookToolStripMenuItem1.Click += facebookToolStripMenuItem1_Click;
            // 
            // deviceModelBindingSource
            // 
            deviceModelBindingSource.DataSource = typeof(AutoAndroid.DeviceModel);
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.White;
            groupBox2.Controls.Add(uiSymbolButton3);
            groupBox2.Controls.Add(uiSymbolButton4);
            groupBox2.Dock = DockStyle.Bottom;
            groupBox2.Location = new Point(0, 553);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(500, 77);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Action";
            groupBox2.Visible = false;
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
            uiSymbolButton3.Location = new Point(112, 24);
            uiSymbolButton3.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton3.MinimumSize = new Size(1, 1);
            uiSymbolButton3.Name = "uiSymbolButton3";
            uiSymbolButton3.Radius = 15;
            uiSymbolButton3.RectColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.RectHoverColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.RectPressColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.RectSelectedColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton3.Size = new Size(130, 40);
            uiSymbolButton3.Symbol = 361515;
            uiSymbolButton3.SymbolSize = 18;
            uiSymbolButton3.TabIndex = 15;
            uiSymbolButton3.Text = "Bắt đầu";
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
            uiSymbolButton4.Location = new Point(260, 24);
            uiSymbolButton4.Margin = new Padding(10, 3, 3, 3);
            uiSymbolButton4.MinimumSize = new Size(1, 1);
            uiSymbolButton4.Name = "uiSymbolButton4";
            uiSymbolButton4.Radius = 15;
            uiSymbolButton4.RectColor = Color.DarkRed;
            uiSymbolButton4.RectHoverColor = Color.DarkRed;
            uiSymbolButton4.RectPressColor = Color.DarkRed;
            uiSymbolButton4.RectSelectedColor = Color.DarkRed;
            uiSymbolButton4.Size = new Size(130, 40);
            uiSymbolButton4.Symbol = 61517;
            uiSymbolButton4.SymbolSize = 17;
            uiSymbolButton4.TabIndex = 14;
            uiSymbolButton4.Text = "Đóng";
            uiSymbolButton4.TipsColor = Color.DarkRed;
            uiSymbolButton4.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton4.Click += uiSymbolButton4_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.White;
            toolStrip1.Dock = DockStyle.Bottom;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStripLabel2, toolStripSeparator1, toolStripLabel3, toolStripLabel4 });
            toolStrip1.Location = new Point(0, 630);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(500, 25);
            toolStrip1.TabIndex = 8;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(37, 22);
            toolStripLabel1.Text = "Tổng:";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.ForeColor = Color.Blue;
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(19, 22);
            toolStripLabel2.Text = "32";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(54, 22);
            toolStripLabel3.Text = "Đã chọn:";
            // 
            // toolStripLabel4
            // 
            toolStripLabel4.ForeColor = Color.Green;
            toolStripLabel4.Name = "toolStripLabel4";
            toolStripLabel4.Size = new Size(19, 22);
            toolStripLabel4.Text = "10";
            // 
            // uiPanel1
            // 
            uiPanel1.BackColor = Color.White;
            uiPanel1.Controls.Add(uiLinkLabel1);
            uiPanel1.Controls.Add(uiTextBox1);
            uiPanel1.Controls.Add(uiHeaderButton1);
            uiPanel1.Controls.Add(uiSymbolButton1);
            uiPanel1.Controls.Add(uiSymbolButton2);
            uiPanel1.Dock = DockStyle.Top;
            uiPanel1.FillColor = Color.White;
            uiPanel1.FillColor2 = Color.White;
            uiPanel1.FillDisableColor = Color.White;
            uiPanel1.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel1.Location = new Point(0, 0);
            uiPanel1.Margin = new Padding(10, 5, 4, 5);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.Padding = new Padding(0, 15, 20, 10);
            uiPanel1.RectColor = Color.White;
            uiPanel1.RectDisableColor = Color.White;
            uiPanel1.Size = new Size(500, 133);
            uiPanel1.TabIndex = 0;
            uiPanel1.Text = null;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiLinkLabel1
            // 
            uiLinkLabel1.ActiveLinkColor = Color.FromArgb(80, 160, 255);
            uiLinkLabel1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiLinkLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLinkLabel1.LinkBehavior = LinkBehavior.AlwaysUnderline;
            uiLinkLabel1.Location = new Point(401, 115);
            uiLinkLabel1.Name = "uiLinkLabel1";
            uiLinkLabel1.Size = new Size(56, 16);
            uiLinkLabel1.TabIndex = 6;
            uiLinkLabel1.TabStop = true;
            uiLinkLabel1.Text = "Doc Api";
            uiLinkLabel1.VisitedLinkColor = Color.FromArgb(230, 80, 80);
            uiLinkLabel1.Click += uiLinkLabel1_Click;
            // 
            // uiTextBox1
            // 
            uiTextBox1.FillColor = Color.White;
            uiTextBox1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiTextBox1.Location = new Point(19, 48);
            uiTextBox1.Margin = new Padding(4, 5, 4, 5);
            uiTextBox1.MinimumSize = new Size(1, 16);
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(5);
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(195, 35);
            uiTextBox1.Symbol = 61442;
            uiTextBox1.TabIndex = 5;
            uiTextBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBox1.Watermark = "Tìm kiếm...";
            uiTextBox1.TextChanged += uiTextBox1_TextChanged;
            // 
            // uiHeaderButton1
            // 
            uiHeaderButton1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            uiHeaderButton1.CircleColor = Color.White;
            uiHeaderButton1.CircleHoverColor = Color.White;
            uiHeaderButton1.CircleSize = 15;
            uiHeaderButton1.Cursor = Cursors.Hand;
            uiHeaderButton1.FillColor = Color.White;
            uiHeaderButton1.FillDisableColor = Color.White;
            uiHeaderButton1.FillHoverColor = Color.Blue;
            uiHeaderButton1.FillPressColor = Color.White;
            uiHeaderButton1.FillSelectedColor = Color.White;
            uiHeaderButton1.Font = new Font("Microsoft Sans Serif", 12F);
            uiHeaderButton1.ForeHoverColor = Color.Blue;
            uiHeaderButton1.ForeSelectedColor = Color.Blue;
            uiHeaderButton1.Location = new Point(463, 106);
            uiHeaderButton1.MinimumSize = new Size(1, 1);
            uiHeaderButton1.Name = "uiHeaderButton1";
            uiHeaderButton1.Padding = new Padding(0, 8, 0, 3);
            uiHeaderButton1.Radius = 0;
            uiHeaderButton1.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            uiHeaderButton1.RectSides = ToolStripStatusLabelBorderSides.None;
            uiHeaderButton1.Size = new Size(25, 25);
            uiHeaderButton1.Symbol = 61552;
            uiHeaderButton1.SymbolColor = Color.Gray;
            uiHeaderButton1.SymbolSize = 20;
            uiHeaderButton1.TabIndex = 3;
            uiHeaderButton1.TipsColor = Color.White;
            uiHeaderButton1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.FillColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.FillColor2 = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.FillHoverColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.FillPressColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.FillSelectedColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton1.Location = new Point(221, 48);
            uiSymbolButton1.Margin = new Padding(3, 3, 10, 3);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Radius = 15;
            uiSymbolButton1.RectColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.RectHoverColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.RectPressColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.RectSelectedColor = Color.FromArgb(4, 60, 44);
            uiSymbolButton1.Size = new Size(128, 35);
            uiSymbolButton1.Symbol = 61473;
            uiSymbolButton1.SymbolSize = 15;
            uiSymbolButton1.TabIndex = 2;
            uiSymbolButton1.Text = "Load devices";
            uiSymbolButton1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.FillColor = Color.DarkRed;
            uiSymbolButton2.FillColor2 = Color.DarkRed;
            uiSymbolButton2.FillHoverColor = Color.DarkRed;
            uiSymbolButton2.FillPressColor = Color.DarkRed;
            uiSymbolButton2.FillSelectedColor = Color.DarkRed;
            uiSymbolButton2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiSymbolButton2.Location = new Point(360, 48);
            uiSymbolButton2.Margin = new Padding(10, 3, 3, 3);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Radius = 15;
            uiSymbolButton2.RectColor = Color.DarkRed;
            uiSymbolButton2.RectHoverColor = Color.DarkRed;
            uiSymbolButton2.RectPressColor = Color.DarkRed;
            uiSymbolButton2.RectSelectedColor = Color.DarkRed;
            uiSymbolButton2.Size = new Size(128, 35);
            uiSymbolButton2.Symbol = 561503;
            uiSymbolButton2.SymbolSize = 15;
            uiSymbolButton2.TabIndex = 1;
            uiSymbolButton2.Text = "Kill ADB";
            uiSymbolButton2.TipsColor = Color.DarkRed;
            uiSymbolButton2.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(flowLayoutPanel1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(10);
            groupBox1.Size = new Size(630, 655);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Devices";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(10, 26);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(610, 619);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // ucManagerDevices
            // 
            BackColor = Color.White;
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "ucManagerDevices";
            Size = new Size(1130, 655);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiDataGridView2).EndInit();
            uiContextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)deviceModelBindingSource).EndInit();
            groupBox2.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            uiPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UISymbolButton uiSymbolButton2;
        private Sunny.UI.UIContextMenuStrip uiContextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem tấtCảToolStripMenuItem;
        private ToolStripMenuItem bôiĐenToolStripMenuItem;
        private ToolStripMenuItem statusToolStripMenuItem;
        private ToolStripMenuItem bỏChọnTấtCảToolStripMenuItem;
        private ToolStripMenuItem tắtViewToolStripMenuItem;
        private ToolStripMenuItem tắtToolStripMenuItem;
        private ToolStripMenuItem mởToolStripMenuItem1;
        private ToolStripMenuItem mởToolStripMenuItem;
        private ToolStripMenuItem càiĐặtApkToolStripMenuItem;
        private ToolStripMenuItem wifiToolStripMenuItem;
        private ToolStripMenuItem bậtWifiToolStripMenuItem;
        private ToolStripMenuItem tắtWifiToolStripMenuItem;
        private ToolStripMenuItem kếtNốiWifiToolStripMenuItem;
        private ToolStripMenuItem gỡCàiĐặtPackageToolStripMenuItem;
        private ToolStripMenuItem rebootToolStripMenuItem;
        private ToolStripMenuItem ứngDụngToolStripMenuItem;
        private ToolStripMenuItem facebookToolStripMenuItem;
        private ToolStripMenuItem backupToolStripMenuItem;
        private ToolStripMenuItem restoreToolStripMenuItem;
        private ToolStripMenuItem changeInfoToolStripMenuItem;
        private ToolStripMenuItem tikTokToolStripMenuItem;
        private ToolStripMenuItem backupToolStripMenuItem1;
        private ToolStripMenuItem restoreToolStripMenuItem1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private BindingSource deviceModelBindingSource;
        private Sunny.UI.UIDataGridView uiDataGridView2;
        private Sunny.UI.UIHeaderButton uiHeaderButton1;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem disconnectToolStripMenuItem;
        private Sunny.UI.UITextBox uiTextBox1;
        private Sunny.UI.UILinkLabel uiLinkLabel1;
        private GroupBox groupBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private ToolStripMenuItem instagramToolStripMenuItem;
        private ToolStripMenuItem backupToolStripMenuItem2;
        private ToolStripMenuItem restoreToolStripMenuItem2;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel3;
        private ToolStripLabel toolStripLabel4;
        private Panel panel2;
        private Sunny.UI.UISymbolButton uiSymbolButton3;
        private Sunny.UI.UISymbolButton uiSymbolButton4;
        public GroupBox groupBox2;
        private ToolStripMenuItem dDToolStripMenuItem;
        private ToolStripMenuItem facebookToolStripMenuItem1;
    }
}