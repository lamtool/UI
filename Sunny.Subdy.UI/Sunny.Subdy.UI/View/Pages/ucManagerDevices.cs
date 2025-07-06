using AutoAndroid;
using AutoAndroid.Stream;
using Sunny.Subd.Core.Facebook;
using Sunny.Subd.Core.Gmail;
using Sunny.Subd.Core.Proxies;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.ControlViews.Convertes;
using Sunny.Subdy.UI.View.Forms;
using Sunny.Subdy.UI.View.Forms.Actions;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class ucManagerDevices : UserControl
    {
        private int batchSize = 200;
        private int startIndex = 0;
        public bool IsStart = false;
        private bool isLoading = false;
        private CancellationTokenSource cancellationTokenSource;
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DeviceModel))]
        public ucManagerDevices()
        {
            InitializeComponent();
            LoadDevices();
            uiDataGridView2.CellValueChanged += dgvDevices_CellValueChanged;
            uiDataGridView2.CurrentCellDirtyStateChanged += dgvDevices_CurrentCellDirtyStateChanged;
            uiDataGridView2.CellFormatting += uiDataGridView1_CellFormatting;
            flowLayoutPanel1.MouseWheel += (s, e) => CheckIfNeedMoreControls();
            flowLayoutPanel1.Scroll += (s, e) => CheckIfNeedMoreControls();
            flowLayoutPanel1.Resize += (s, e) => CheckIfNeedMoreControls();
        }
        private void LoadVirtualWindow(int start)
        {
            if (isLoading) return;
            isLoading = true;

            flowLayoutPanel1.SuspendLayout();

            // Xoá toàn bộ (có thể tối ưu về sau chỉ xóa/giữ cần thiết)
            flowLayoutPanel1.Controls.Clear();

            var controlsToShow = DeviceServices.DisplayList
                .Skip(start)
                .Take(batchSize)
                .ToArray();

            flowLayoutPanel1.Controls.AddRange(controlsToShow);
            flowLayoutPanel1.ResumeLayout();

            isLoading = false;
        }
        private void CheckIfNeedMoreControls()
        {
            int scrollValue = flowLayoutPanel1.VerticalScroll.Value;
            int maxScroll = flowLayoutPanel1.VerticalScroll.Maximum - flowLayoutPanel1.ClientSize.Height;

            // Tính phần trăm đã cuộn
            float scrollPercent = maxScroll > 0 ? (float)scrollValue / maxScroll : 0;
            int newStartIndex = (int)(scrollPercent * (DeviceServices.DisplayList.Count - batchSize));

            newStartIndex = Math.Max(0, Math.Min(newStartIndex, DeviceServices.DisplayList.Count - batchSize));

            if (Math.Abs(newStartIndex - startIndex) >= batchSize / 2) // chỉ update nếu khác biệt đáng kể
            {
                startIndex = newStartIndex;
                LoadVirtualWindow(startIndex);
            }
        }
        private void uiDataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var dgv = sender as Sunny.UI.UIDataGridView;
            var row = dgv.Rows[e.RowIndex];
            Color textColor = Color.Black;
            // Giả sử bạn có cột tên là "Status"
            if (row.Cells["dataGridViewTextBoxColumn7"].Value != null && !string.IsNullOrEmpty(row.Cells["dataGridViewTextBoxColumn7"].Value.ToString()) && int.TryParse(row.Cells["dataGridViewTextBoxColumn7"].Value.ToString(), out int type))
            {
                switch (type)
                {
                    case 1:
                        textColor = Color.Red;
                        break;
                    case 2:
                        textColor = Color.Green;
                        break;
                    case 0:
                    default:
                        // Giữ màu mặc định
                        break;
                }
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.ForeColor = textColor;
                    cell.Style.SelectionForeColor = textColor; // Đặt màu chữ khi chọn
                }
            }
            if (row.Cells["dataGridViewCheckBoxColumn1"].Value != null && !string.IsNullOrEmpty(row.Cells["dataGridViewCheckBoxColumn1"].Value.ToString()) && bool.TryParse(row.Cells["dataGridViewCheckBoxColumn1"].Value.ToString(), out bool check))
            {
                toolStripLabel4.Text = $"{DeviceServices.DeviceModels.Count(x => x.Check)}";
            }



        }
        PictureBox picture = new PictureBox
        {
            Image = Properties.Resources.LamTool_net,
            SizeMode = PictureBoxSizeMode.Zoom,
            Dock = DockStyle.Fill
        };
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DeviceModel))]
        private void LoadDevices()
        {
            try
            {
                BindingList<DeviceModel> bindingList = new BindingList<DeviceModel>(DeviceServices.DeviceModels);
                //SortableBindingList<DeviceModel> bindingList = new SortableBindingList<DeviceModel>(DeviceServices.DeviceModels);
                uiDataGridView2.DataSource = bindingList;
                toolStripLabel2.Text = $"{DeviceServices.DeviceModels.Count}";

                if (DeviceServices.DisplayList.Any())
                {
                    if (groupBox1.Controls.Contains(picture))
                    {
                        groupBox1.Controls.Remove(picture);
                        groupBox1.Controls.Add(flowLayoutPanel1);
                    }
                    LoadVirtualWindow(0);
                }
                else
                {
                    if (groupBox1.Controls.Contains(picture))
                    {
                        return;
                    }
                    groupBox1.Controls.Remove(flowLayoutPanel1);
                    groupBox1.Controls.Add(picture);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                throw ex;
            }

        }
        private void dgvDevices_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            uiDataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void dgvDevices_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (uiDataGridView2.IsCurrentCellDirty)
            {
                uiDataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }

        }
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            uiSymbolButton2.Enabled = false;
            uiSymbolButton1.Enabled = false;
            DeviceServices.GetDeviceModels();
            DeviceServices.GetScrcpyDisplays();
            LoadDevices();
            uiSymbolButton2.Enabled = true;
            uiSymbolButton1.Enabled = true;
        }
        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            uiSymbolButton2.Enabled = false;
            uiSymbolButton1.Enabled = false;
            DeviceServices.ADBKill();
            LoadDevices();
            uiSymbolButton2.Enabled = true;
            uiSymbolButton1.Enabled = true;
        }
        private void tấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceServices.SelectAll();
        }
        private void bỏChọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceServices.UnSelectAll();
        }
        private void bôiĐenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in uiDataGridView2.SelectedRows)
            {
                if (row.DataBoundItem is DeviceModel device)
                {
                    device.Check = true;
                }

            }
        }
        private async Task connectToolStripMenuItem_ClickSafe()
        {
            try
            {
                uiSymbolButton2.Enabled = false;
                uiSymbolButton1.Enabled = false;
                await DeviceServices.Connect();
                uiSymbolButton2.Enabled = true;
                uiSymbolButton1.Enabled = true;
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }

        }
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = connectToolStripMenuItem_ClickSafe();
        }
        private async Task mởToolStripMenuItem1_ClickSafe()
        {
            try
            {
                List<ScrcpyDisplay> displays = new List<ScrcpyDisplay>();
                foreach (DataGridViewRow row in uiDataGridView2.SelectedRows)
                {
                    if (row.DataBoundItem is DeviceModel device)
                    {
                        if (DeviceServices.DisplayList.FirstOrDefault(x => x.Device.Serial == device.Serial) is ScrcpyDisplay display)
                        {
                            displays.Add(display);
                        }
                    }

                }
                if (!displays.Any()) return;
                await DeviceServices.ConnectScrcpies(displays);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void mởToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _ = mởToolStripMenuItem1_ClickSafe();
        }
        private async Task tắtToolStripMenuItem_ClickSafe()
        {
            try
            {
                List<ScrcpyDisplay> displays = new List<ScrcpyDisplay>();
                foreach (DataGridViewRow row in uiDataGridView2.SelectedRows)
                {
                    if (row.DataBoundItem is DeviceModel device)
                    {
                        if (DeviceServices.DisplayList.FirstOrDefault(x => x.Device.Serial == device.Serial) is ScrcpyDisplay display)
                        {
                            displays.Add(display);
                        }
                    }

                }
                if (!displays.Any()) return;
                await DeviceServices.DisConnectScrcpies(displays);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }

        }
        private void tắtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = tắtToolStripMenuItem_ClickSafe();
        }
        private async Task càiĐặtApkToolStripMenuItem_ClickSafe()
        {
            try
            {
                string file = string.Empty;
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "APK files (*.apk)|*.apk";
                    openFileDialog.Title = "Select APK File";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        file = openFileDialog.FileName;
                    }
                }

                if (string.IsNullOrEmpty(file))
                {
                    return;
                }
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị để cài đặt APK.");
                    return;
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.InstallApk, file);
                CommonMethod.ShowMessageSuccess("Cài đặt APK thành công!");
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void càiĐặtApkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = càiĐặtApkToolStripMenuItem_ClickSafe();
        }
        private async Task bậtWifiToolStripMenuItem_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị để bật WiFi.");
                    return;
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.EnableWifi);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void bậtWifiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = bậtWifiToolStripMenuItem_ClickSafe();
        }
        private async Task tắtWifiToolStripMenuItem_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.DisableWifi);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void tắtWifiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = tắtWifiToolStripMenuItem_ClickSafe();
        }
        private void kếtNốiWifiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                string value = string.Empty;
                //if (this.ShowInputStringDialog(ref value, false, desc: "Nhập wifi: username|password", true))
                //{
                //    if (string.IsNullOrEmpty(value) || !value.Contains("|"))
                //    {
                //        this.ShowWarningTip("Vui lòng nhập đúng định dạng: username|password");
                //        return;
                //    }
                //    await DeviceServices.HandleEmulators(devices, EmuAction.ConnectWifi, value);
                //}
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }

        }
        private void gỡCàiĐặtPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                string value = string.Empty;
                //if (this.ShowInputStringDialog(ref value, false, desc: "Nhập package app:", true))
                //{
                //    if (string.IsNullOrEmpty(value))
                //    {
                //        return;
                //    }
                //    await DeviceServices.HandleEmulators(devices, EmuAction.UninstallApp, value);
                //}
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }

        }
        private async Task rebootToolStripMenuItem_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.Reboot);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void rebootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = rebootToolStripMenuItem_ClickSafe();
        }
        private async Task changeInfoToolStripMenuItem_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.ChangeInfo);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void changeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = changeInfoToolStripMenuItem_ClickSafe();
        }
        private async Task backupToolStripMenuItem_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                string value = string.Empty;
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Chọn thư mục cần lưu";
                    dialog.UseDescriptionForTitle = true; // Hiển thị mô tả làm tiêu đề (nếu .NET >= 6)
                    dialog.ShowNewFolderButton = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        value = dialog.SelectedPath;
                    }
                }
                if (string.IsNullOrEmpty(value))
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn thư mục lưu trữ!");
                    return;
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.BackupFB, value);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = backupToolStripMenuItem_ClickSafe();
        }
        private async Task backupToolStripMenuItem1_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                string value = string.Empty;
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Chọn thư mục cần lưu";
                    dialog.UseDescriptionForTitle = true; // Hiển thị mô tả làm tiêu đề (nếu .NET >= 6)
                    dialog.ShowNewFolderButton = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        value = dialog.SelectedPath;
                    }
                }
                if (string.IsNullOrEmpty(value))
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.BackupTikTok, value);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }

        }
        private void backupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _ = backupToolStripMenuItem1_ClickSafe();
        }
        private async Task backupToolStripMenuItem2_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                string value = string.Empty;
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Chọn thư mục cần lưu";
                    dialog.UseDescriptionForTitle = true; // Hiển thị mô tả làm tiêu đề (nếu .NET >= 6)
                    dialog.ShowNewFolderButton = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        value = dialog.SelectedPath;
                    }
                }
                if (string.IsNullOrEmpty(value))
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn thư mục lưu trữ!");
                    return;
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.BackupIG, value);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void backupToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _ = backupToolStripMenuItem2_ClickSafe();
        }
        private async Task restoreToolStripMenuItem_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                string value = string.Empty;
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Title = "Chọn file .tar.gz";
                    dialog.Filter = "Gzipped Tar Archive (*.tar.gz)|*.tar.gz";
                    dialog.RestoreDirectory = true;
                    dialog.Multiselect = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        value = dialog.FileName;
                    }
                }
                if (!File.Exists(value))
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn thư mục lưu trữ!");
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.RestoreFB, value);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = restoreToolStripMenuItem_ClickSafe();
        }
        private async Task restoreToolStripMenuItem1_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                string value = string.Empty;
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Title = "Chọn file .tar.gz";
                    dialog.Filter = "Gzipped Tar Archive (*.tar.gz)|*.tar.gz";
                    dialog.RestoreDirectory = true;
                    dialog.Multiselect = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        value = dialog.FileName;
                    }
                }
                if (!File.Exists(value))
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn thư mục lưu trữ!");
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.RestoreTikTok, value);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void restoreToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            _ = restoreToolStripMenuItem1_ClickSafe();
        }
        private async Task restoreToolStripMenuItem2_ClickSafe()
        {
            try
            {
                var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
                if (!devices.Any())
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }
                string value = string.Empty;
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Title = "Chọn file .tar.gz";
                    dialog.Filter = "Gzipped Tar Archive (*.tar.gz)|*.tar.gz";
                    dialog.RestoreDirectory = true;
                    dialog.Multiselect = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        value = dialog.FileName;
                    }
                }
                if (!File.Exists(value))
                {
                    CommonMethod.ShowMessageWarning("Chưa chọn file nào!");
                }
                await DeviceServices.HandleEmulators(devices, EmuAction.RestoreIG, value);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void restoreToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _ = restoreToolStripMenuItem2_ClickSafe();
        }
        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = uiTextBox1.Text.Trim().ToLower();

            uiDataGridView2.ClearSelection(); // Bỏ chọn tất cả trước

            if (string.IsNullOrEmpty(searchText))
                return;

            foreach (DataGridViewRow row in uiDataGridView2.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null &&
                        cell.Value.ToString()!.ToLower().Contains(searchText))
                    {
                        row.Selected = true; // chọn cả dòng nếu có ô khớp
                        break; // bỏ qua các ô còn lại trong dòng này
                    }
                }
            }
        }
        private void uiLinkLabel1_Click(object sender, EventArgs e)
        {
            fDocAPIPhone fDocAPI = new fDocAPIPhone();
            fDocAPI.ShowDialog();
        }
        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            if (uiSymbolButton4.Text == "Dừng" && cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                uiSymbolButton4.Enabled = false;
            }
            else
            {
                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.Cancel;
                    parentForm.Close();
                }
            }

        }
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            if (DeviceServices.DeviceModels.Where(x => x.Check).Any())
            {
                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close(); // đóng form
                }
            }
            else
            {
                CommonMethod.ShowMessageWarning("Vui lòng kết nối thiết bị trước khi bắt đầu!");
            }
        }
        private async Task facebookToolStripMenuItem1_ClickSafe(string platform)
        {
            try
            {
                if (!DeviceServices.DeviceModels.Any(x => x.Check))
                {
                    CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                    return;
                }

                fAction_RegFB fAction_RegFB = null;

                try
                {
                    fAction_RegFB = new fAction_RegFB(platform);
                    var value = fAction_RegFB.ShowDialog();
                    if (value == DialogResult.Cancel)
                        return;
                }
                catch (Exception ex)
                {
                    CommonMethod.ShowMessageError("Lỗi khi mở form cấu hình: " + ex.Message);
                    return;
                }

                groupBox2.Visible = true;
                uiSymbolButton4.Text = "Dừng";
                uiSymbolButton3.Enabled = false;

                try
                {
                    await RunAsync(fAction_RegFB.Folder);
                }
                catch (Exception ex)
                {
                    CommonMethod.ShowMessageError("Lỗi khi chạy tác vụ: " + ex.Message);
                }

                groupBox2.Visible = false;
                uiSymbolButton4.Text = "Đóng";
                uiSymbolButton3.Enabled = true;
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError("Lỗi không xác định: " + ex.Message);
            }
        }
        private void facebookToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _ = facebookToolStripMenuItem1_ClickSafe(RegistrationType.RegFacebook);
        }
        private async Task RunAsync(Folder folder)
        {
            LogManager.LogRegsiner.Clear();
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = cancellationTokenSource.Token;
            List<Task> tasks = new List<Task>();
            JsonHelper settingRegsiner = new JsonHelper(nameof(fAction_RegFB), false);
            JsonHelper settingGeneral = new JsonHelper(nameof(pageSetting), false);
            ProxyService.Proxies.Clear();
            ProxyService.Proxies.AddRange(settingGeneral.GetValuesList("txtLines"));
            GmailService.Gmails.Clear();
            string file = settingRegsiner.GetValuesFromInputString("txtGmail");
            if (File.Exists(file))
            {
                GmailService.Gmails.AddRange(File.ReadAllLines(file));
            }
            foreach (var device in DeviceServices.DeviceModels.Where(x => x.Check))
            {
                try
                {
                    if (folder.Type == "Facebook")
                    {
                        FacebookRegsiner facebook = new FacebookRegsiner(device, settingRegsiner, settingGeneral, ct, folder);
                        tasks.Add(Task.Run(async () =>
                        {
                            await facebook.RegisterAsync();
                        }));
                    }
                    else if (folder.Type == "Gmail")
                    {
                        GmailRegsiner gmail = new GmailRegsiner(device, settingRegsiner, settingGeneral, ct, folder);
                        tasks.Add(Task.Run(async () =>
                        {
                            await gmail.RegisterAsync();
                        }));
                    }

                }
                catch (Exception ex)
                {
                    CommonMethod.ShowMessageError(ex.Message);
                    break;
                }

            }
            await Task.WhenAll(tasks);
        }

        private void uiHeaderButton1_Click(object sender, EventArgs e)
        {
            fLogRegsiner logRegsiner = new fLogRegsiner();
            logRegsiner.Show();
        }

        private void uiSymbolButton5_Click(object sender, EventArgs e)
        {
            if (uiSymbolButton5.Symbol == 362498)
            {
                groupBox1.Visible = false;
                panel1.Dock = DockStyle.Fill;
                uiSymbolButton5.Symbol = 362500;
            }
            else
            {
                groupBox1.Visible = true;
                panel1.Dock = DockStyle.Right;
                uiSymbolButton5.Symbol = 362498;
            }
        }

        private void gmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = facebookToolStripMenuItem1_ClickSafe(RegistrationType.RegGmail);
        }
    }
}
