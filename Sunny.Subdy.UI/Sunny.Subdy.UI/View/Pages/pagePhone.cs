using AutoAndroid;
using Sunny.Subdy.UI.Services;
using Sunny.Subdy.UI.View.DeviceControl;
using Sunny.UI;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pagePhone : UIPage
    {
        private int batchSize = 15;
        private int startIndex = 0;
        private int estimatedControlHeight = 80; // hoặc đo thật
        private bool isLoading = false;
        public pagePhone()
        {
            InitializeComponent();
            this.Symbol = 558149;
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

            BindingList<DeviceModel> bindingList = new BindingList<DeviceModel>(DeviceServices.DeviceModels);
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
    }
}
