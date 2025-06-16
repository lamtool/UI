using System.Data;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.UI.View.Controls;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pageScript : UIPage
    {
        private ScriptContext _scriptContext;
        public pageScript()
        {
            InitializeComponent();
            this.Symbol = 557444; // Set the symbol for the page, can be used for icons
            _scriptContext = new ScriptContext();
            if(string.IsNullOrEmpty(txtType.Text.ToString()))
            {
                txtType.SelectedIndex = 0; // Default to "Tất cả" if no selection
            }
        }

        private void pageScript_Load(object sender, EventArgs e)
        {

        }
        private void LoadFolders(string? filter = null, string type = "Tất cả")
        {
            flowLayoutPanel1.Controls.Clear();
            var folders = _scriptContext.GetAll();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.Trim().ToLower();
                folders = folders.Where(f =>
                    (!string.IsNullOrEmpty(f.Name) && f.Name.ToLower().Contains(filter)) ||
                    (!string.IsNullOrEmpty(f.DateCreate) && f.DateCreate.ToLower().Contains(filter)) ||
                    (!string.IsNullOrEmpty(f.Type) && f.Type.ToLower().Contains(filter))
                ).ToList();
            }
            if (!string.IsNullOrEmpty(type) && type != "Tất cả")
            {
                type = type.Trim().ToLower();
                folders = folders.Where(f => f.Type.ToLower() == type).ToList();
            }

            List<Control> controls = new List<Control>();
            foreach (var folder in folders)
            {
                ucScipt uc = new ucScipt(folder);
                controls.Add(uc);
            }
            flowLayoutPanel1.Controls.AddRange(controls.ToArray());
        }
        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            uiTextBox1.Text = "";
            LoadFolders(uiTextBox1.Text, txtType.SelectedItem.ToString());
        }

        private void txtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFolders(uiTextBox1.Text, txtType.SelectedItem.ToString());
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            LoadFolders(uiTextBox1.Text, txtType.SelectedItem.ToString());
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            fEditScirpt ucFolder = new fEditScirpt(null);
            ucFolder.ShowDialog();
            LoadFolders();
        }
    }
}
