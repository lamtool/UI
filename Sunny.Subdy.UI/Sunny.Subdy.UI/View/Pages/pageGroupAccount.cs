using System.Windows.Forms;
using OpenCvSharp;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.View.Controls;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pageGroupAccount : UIPage
    {
        private readonly FolderContext _folderContext;
        public pageGroupAccount()
        {
            InitializeComponent();
            this.Symbol = 559937; // Set the symbol for the page, can be used for icons
            _folderContext = new FolderContext();
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, action: new System.Action(() =>
            {
                LoadFolders(uiTextBox1.Text, txtType.SelectedItem.ToString());

            }), exists: false);
        }

        private void pageGroupAccount_Load(object sender, EventArgs e)
        {
            txtType.SelectedItem = "Tất cả";


        }
        private void LoadFolders(string? filter = null, string type = "Tất cả")
        {
            flowLayoutPanel1.Controls.Clear();
            var folders = _folderContext.GetAll();

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
                ucGroup uc = new ucGroup(folder);
                controls.Add(uc);
            }
            flowLayoutPanel1.Controls.AddRange(controls.ToArray());
        }

        private void uiPanel1_Click(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            ucFolder ucFolder = new ucFolder();
            ucFolder.ShowDialog();
            LoadFolders();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            uiTextBox1.Text = "";
            LoadFolders(uiTextBox1.Text, txtType.SelectedItem.ToString());
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            LoadFolders(uiTextBox1.Text, txtType.SelectedItem.ToString());
        }

        private void txtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFolders(uiTextBox1.Text, txtType.SelectedItem.ToString());
        }
    }
}
