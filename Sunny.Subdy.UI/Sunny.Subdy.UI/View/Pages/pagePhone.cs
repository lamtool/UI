using AutoAndroid;
using Sunny.Subdy.UI.Services;
using Sunny.UI;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pagePhone : UIPage
    {
        public pagePhone()
        {
            InitializeComponent();
            this.Symbol = 558149;
            LoadDevices();
            uiDataGridView2.CellValueChanged += dgvDevices_CellValueChanged;
            uiDataGridView2.CurrentCellDirtyStateChanged += dgvDevices_CurrentCellDirtyStateChanged;
            uiDataGridView2.CellFormatting += uiDataGridView1_CellFormatting;
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
            }

            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Style.ForeColor = textColor;
                cell.Style.SelectionForeColor = textColor; // Đặt màu chữ khi chọn
            }
        }
        private void LoadDevices()
        {

            BindingList<DeviceModel> bindingList = new BindingList<DeviceModel>(DeviceServices.DeviceModels);
            uiDataGridView2.DataSource = bindingList;
            toolStripLabel2.Text = $"{DeviceServices.DeviceModels.Count}";
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
            foreach (DataGridViewRow row in uiDataGridView2.Rows)
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
    }
}
