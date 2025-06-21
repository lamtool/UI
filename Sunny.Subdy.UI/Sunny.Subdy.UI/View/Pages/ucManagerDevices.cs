using System.ComponentModel;
using System.Threading;
using AutoAndroid;
using AutoAndroid.Stream;
using Sunny.Subd.Core.Facebook;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.ControlViews.Convertes;
using Sunny.Subdy.UI.View.Forms;
using Sunny.Subdy.UI.View.Forms.Actions;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class ucManagerDevices : UserControl
    {
        private int batchSize = 15;
        private int startIndex = 0;
        public bool IsStart = false;
        private bool isLoading = false;
        private CancellationTokenSource cancellationTokenSource;
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
        private void LoadDevices()
        {
            SortableBindingList<DeviceModel> bindingList = new SortableBindingList<DeviceModel>(DeviceServices.DeviceModels);
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
        private async void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiSymbolButton2.Enabled = false;
            uiSymbolButton1.Enabled = false;
            await DeviceServices.Connect();
            uiSymbolButton2.Enabled = true;
            uiSymbolButton1.Enabled = true;
        }
        private async void mởToolStripMenuItem1_Click(object sender, EventArgs e)
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
        private async void tắtToolStripMenuItem_Click(object sender, EventArgs e)
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
        private async void càiĐặtApkToolStripMenuItem_Click(object sender, EventArgs e)
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
        private async void bậtWifiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
            if (!devices.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị để bật WiFi.");
                return;
            }
            await DeviceServices.HandleEmulators(devices, EmuAction.EnableWifi);
        }
        private async void tắtWifiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
            if (!devices.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                return;
            }
            await DeviceServices.HandleEmulators(devices, EmuAction.DisableWifi);
        }
        private async void kếtNốiWifiToolStripMenuItem_Click(object sender, EventArgs e)
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
        private async void gỡCàiĐặtPackageToolStripMenuItem_Click(object sender, EventArgs e)
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
        private async void rebootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
            if (!devices.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                return;
            }
            await DeviceServices.HandleEmulators(devices, EmuAction.Reboot);
        }
        private async void changeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var devices = DeviceServices.DeviceModels.Where(x => x.Check).ToList();
            if (!devices.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                return;
            }
            await DeviceServices.HandleEmulators(devices, EmuAction.ChangeInfo);
        }
        private async void backupToolStripMenuItem_Click(object sender, EventArgs e)
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
        private async void backupToolStripMenuItem1_Click(object sender, EventArgs e)
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
        private async void backupToolStripMenuItem2_Click(object sender, EventArgs e)
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
        private async void restoreToolStripMenuItem_Click(object sender, EventArgs e)
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
        private async void restoreToolStripMenuItem1_Click(object sender, EventArgs e)
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
        private async void restoreToolStripMenuItem2_Click(object sender, EventArgs e)
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
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.Cancel;
                parentForm.Close();
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
        private void facebookToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!DeviceServices.DeviceModels.Where(x => x.Check).Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng chọn ít nhất một thiết bị.");
                return;
            }
            fAction_RegFB fAction_RegFB = new fAction_RegFB();
            var value = fAction_RegFB.ShowDialog();
            if (value == DialogResult.Cancel)
            {
                return;
            }
            groupBox2.Visible = true;
            uiSymbolButton4.Text = "Dừng";
            uiSymbolButton3.Enabled = false;
        }
        private async Task RunAsync()
        {
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = cancellationTokenSource.Token;
            List<Task> tasks = new List<Task>();
            JsonHelper settingRegsiner = SettingsTool.GetSettings(nameof(fAction_RegFB), true);
            JsonHelper settingGeneral = SettingsTool.GetSettings(nameof(pageSetting), true);
            foreach (var device in DeviceServices.DeviceModels.Where(x => x.Check))
            {
                FacebookRegsiner facebook = new FacebookRegsiner(device, settingRegsiner, settingGeneral, ct);
                tasks.Add(Task.Run(async () =>
                {
                    await facebook.RegisterAsync();
                }));
            }
            await Task.WhenAll(tasks);
        }
    }
}
