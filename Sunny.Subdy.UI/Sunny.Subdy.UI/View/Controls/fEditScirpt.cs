using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class fEditScirpt : Form
    {
        private Script _script;
        public fEditScirpt(Script script)
        {
            InitializeComponent();
            _script = script;
            if (_script != null)
            {
                txtName.Text = _script.Name;
                txtType.SelectedItem = _script.Type;
                uiLabel1.Text = "Cập nhật kịch bản: " + _script.Name;
                txtDate.Text = _script.DateCreate;
            }
            else
            {
                txtName.Text = string.Empty;
                txtType.SelectedIndex = 0;
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            ScriptContext _folderContext = new ScriptContext();
            if (string.IsNullOrEmpty(txtName.Text))
            {
                CommonMethod.ShowMessageWarning("Tên kịch bản không được bỏ trống!", "Thông báo");
                return;
            }
            if (_folderContext.GetByName(txtName.Text) != null)
            {
                CommonMethod.ShowMessageWarning("Tên kịch bản đã tồn tại!", "Thông báo");
                return;
            }
            if (_script != null)
            {
                _script.Name = txtName.Text;
                _script.Type = txtType.SelectedItem.ToString();
                if (_folderContext.Update(_script))
                {
                    CommonMethod.ShowMessageSuccess(txtName.Text + " đã được cập nhật thành công!", "Thông báo");
                }
            }
            else
            {
                Script folder = new Script
                {
                    Id = Guid.NewGuid(),
                    Name = txtName.Text,
                    DateCreate = DateTime.Now.ToString("dd/MM/yyyy"),
                    Type = txtType.SelectedItem.ToString().Trim(),
                    Config = string.Empty,
                };
                if (_folderContext.Add(folder))
                {
                    CommonMethod.ShowMessageSuccess(txtName.Text + " đã được thêm thành công!", "Thông báo");
                }
            }
            Close();
        }
    }
}
