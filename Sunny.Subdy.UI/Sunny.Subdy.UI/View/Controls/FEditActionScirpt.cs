using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.View.Forms;
using Sunny.Subdy.UI.View.Forms.Actions;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class FEditActionScirpt : Form
    {
        private Script _script;
        private ScriptActionContext _scriptActionContext;
        public FEditActionScirpt(Script script)
        {
            InitializeComponent();
            _script = script;
            _scriptActionContext = new ScriptActionContext();
            LoadData();
        }
        private void LoadData()
        {
            uiDataGridView2.Rows.Clear();
            if (string.IsNullOrEmpty(_script.Config))
            {
                return;
            }
            var actionIds = _script.Config
     .Split('|', StringSplitOptions.RemoveEmptyEntries)
     .Select(id => Guid.TryParse(id, out var guid) ? guid : Guid.Empty)
     .Where(guid => guid != Guid.Empty)
     .ToList();
            var actions = _scriptActionContext.GetByIdsInOrder(actionIds);
            if (actions.Count == 0)
            {
                return;
            }
            int i = 0;
            foreach (var action in actions)
            {
                i++;
                uiDataGridView2.Rows.Add(i, action.Name, "Sửa", "Xóa", action.Id);
            }
        }


        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            uiTextBox1.Text = string.Empty; // Xóa nội dung ô tìm kiếm
            LoadData();
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

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            fListActionFacebook fListAction = new fListActionFacebook(_script);
            fListAction.ShowDialog();
            uiTextBox1.Text = string.Empty; // Xóa nội dung ô tìm kiếm
            LoadData();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (uiDataGridView2.Rows.Count <= 0)
            {
                CommonMethod.ShowMessageError("Vui lòng thêm tương tác", "Lỗi");
                return;
            }
            int selectedRowIndex = uiDataGridView2.CurrentRow.Index;
            SwapRows(uiDataGridView2, selectedRowIndex, selectedRowIndex - 1);
        }
        private void SwapRows(DataGridView dataGridView, int rowIndex1, int rowIndex2)
        {
            if (rowIndex1 < 0 || rowIndex1 >= dataGridView.Rows.Count || rowIndex2 < 0 || rowIndex2 >= dataGridView.Rows.Count)
            {
                return;
            }

            DataGridViewRow row1 = dataGridView.Rows[rowIndex1];
            DataGridViewRow row2 = dataGridView.Rows[rowIndex2];

            for (int i = 0; i < row1.Cells.Count; i++)
            {
                object temp = row1.Cells[i].Value;
                row1.Cells[i].Value = row2.Cells[i].Value;
                row2.Cells[i].Value = temp;
            }
            int indexrow1 = Convert.ToInt32(row1.Cells[0].Value);
            int indexrow2 = Convert.ToInt32(row2.Cells[0].Value);
            row2.Cells[0].Value = indexrow1;
            row1.Cells[0].Value = indexrow2;
            dataGridView.Rows[rowIndex2].Selected = true;
            dataGridView.CurrentCell = dataGridView.Rows[rowIndex2].Cells[0];
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (uiDataGridView2.Rows.Count <= 0)
            {
                CommonMethod.ShowMessageError("Vui lòng thêm tương tác", "Lỗi");
                return;
            }
            int selectedRowIndex = uiDataGridView2.CurrentRow.Index;
            SwapRows(uiDataGridView2, selectedRowIndex, selectedRowIndex + 1);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = uiDataGridView2.Columns[e.ColumnIndex];
                Guid id = Guid.Parse(uiDataGridView2.Rows[e.RowIndex].Cells["colId"].Value.ToString() ?? string.Empty);
                if (column is DataGridViewButtonColumn && column.Name == "btnEdit")
                {
                    var action = _scriptActionContext.GetById(id);
                    OpenForm(action);
                }
                else if (column is DataGridViewButtonColumn && column.Name == "btnDelete")
                {
                    _scriptActionContext.DeleteById(id);
                }
                LoadData();
            }
        }
        private void OpenForm(ScriptAction action)
        {
            string config = string.Empty;
            string name = string.Empty;
            switch (action.Type)
            {
                case TypeAction.FB_SpamXu:
                    {
                        fActioc_SpamXu form = new fActioc_SpamXu(TypeAction.GetNameAction(action.Name), action.Json);
                        form.ShowDialog();
                        config = form._Json;
                        name = form._Name.Trim();
                        break;
                    }
            }
            if (string.IsNullOrEmpty(config))
            {
                return;
            }
            action = new ScriptAction
            {
                Name = name,
                Json = config,
            };
            if (!_scriptActionContext.Update(action))
            {
                CommonMethod.ShowMessageError("Lỗi khi thêm hành động vào kịch bản", "Lỗi");
                return;
            }
        }
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            _script.Config = string.Empty;
            string config = string.Empty;
            foreach (DataGridViewRow row in uiDataGridView2.Rows)
            {
                Guid id = Guid.Parse(row.Cells["colId"].Value.ToString() ?? string.Empty);
                config += id.ToString() + "|";
            }
            _script.Config = config.TrimEnd('|');
            new ScriptContext().Update(_script);
            CommonMethod.ShowMessageSuccess("Cập nhật thành công", "Thành công");
            this.Close();
        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
