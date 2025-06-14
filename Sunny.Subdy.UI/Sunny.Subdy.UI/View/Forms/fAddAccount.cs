using OpenCvSharp;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Forms
{
    public partial class fAddAccount : Form
    {
        private FormatAccountContext _formatAccountContext;
        public fAddAccount()
        {
            InitializeComponent();
            _formatAccountContext = new FormatAccountContext();
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, action: new System.Action(() =>
            {
                LoadFolders(uiTextBox1.Text, txtType.SelectedItem.ToString());

            }), exists: false);

            List<Control> controls = new List<Control>();
            var lines = Globals.GetFieldsToImportExport();
        }
        private void LoadFormats()
        {
            txtType.Items.Clear();
            var formats = _formatAccountContext.GetAll();
            txtType.Items.Add("Tất cả");
            if (formats != null && formats.Count > 0)
            {
                foreach (var format in formats)
                {
                    txtType.Items.Add(format.Name);
                }
            }

        }
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            string value = string.Empty;
            if (this.ShowInputStringDialog(ref value, false, "Nhập tên nhóm:"))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    UIMessageBox.ShowError("Tên nhóm không được để trống.");
                    return;
                }
                var existingFormat = _formatAccountContext.GetByName(value);
                if (existingFormat != null)
                {
                    UIMessageBox.ShowError("Tên nhóm đã tồn tại.");
                    return;
                }
                var newFormat = new FormatAccount
                {
                    Id = Guid.NewGuid(),
                    Name = value,
                    Fields = string.Empty
                };
                if (_formatAccountContext.Add(newFormat))
                {
                    UIMessageBox.ShowSuccess("Thêm nhóm thành công.");
                }
                else
                {
                    UIMessageBox.ShowError("Thêm nhóm thất bại.");
                }
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
