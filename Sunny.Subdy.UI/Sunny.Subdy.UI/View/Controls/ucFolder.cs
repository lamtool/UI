using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class ucFolder : Form
    {
        Folder _folder;
        public ucFolder(Folder folder = null)
        {
            InitializeComponent();
            if (folder != null)
            {
                txtDate.Text = folder.DateCreate;
                txtName.Text = folder.Name;
                txtType.SelectedItem = folder.Type;
                uiLabel1.Text = "Cập nhật nhóm: " + folder.Name;
                _folder = folder;
            }
            else
            {
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtName.Text = string.Empty;
                txtType.SelectedItem = "";
            }
        }

        private void ucFolder_Load(object sender, EventArgs e)
        {
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            FolderContext _folderContext = new FolderContext();
            if (string.IsNullOrEmpty(txtName.Text))
            {
                CommonMethod.ShowMessageWarning("Tên nhóm không được bỏ trống!", "Thông báo");
                return;
            }
            if (_folderContext.GetByName(txtName.Text) != null)
            {
                CommonMethod.ShowMessageWarning("Tên nhóm đã tồn tại!", "Thông báo");
                return;
            }
            if (_folder != null)
            {
                _folder.Name = txtName.Text;
                _folder.Type = txtType.SelectedItem.ToString();
                if (_folderContext.Update(_folder))
                {
                    CommonMethod.ShowMessageSuccess(txtName.Text + " đã được cập nhật thành công!", "Thông báo");
                }
            }
            else
            {
                Folder folder = new Folder
                {
                    Id = Guid.NewGuid(),
                    Name = txtName.Text,
                    DateCreate = DateTime.Now.ToString("dd/MM/yyyy"),
                    Count = "0",
                    Type = txtType.SelectedItem.ToString().Trim(),
                    IsView = true
                };
                if (_folderContext.Add(folder))
                {
                    CommonMethod.ShowMessageSuccess(txtName.Text + " đã được thêm thành công!", "Thông báo");
                }
            }
            Close();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
