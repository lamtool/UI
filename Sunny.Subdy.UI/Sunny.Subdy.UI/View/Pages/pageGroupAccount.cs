using Sunny.Subdy.Data.Context;
using Sunny.Subdy.UI.View.Controls;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pageGroupAccount : UIPage
    {
        private readonly FolderContext _folderContext;
        private readonly pageFacebook pageFacebook;
        public pageGroupAccount(pageFacebook pageFacebook)
        {
            InitializeComponent();
            this.Symbol = 559937;
            _folderContext = new FolderContext();
            if (string.IsNullOrEmpty(txtType.Text))
            {
                txtType.SelectedIndex = 0;
            }
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, action: new System.Action(() =>
            {
                LoadFolders(uiTextBox1.Text, txtType.Text.Trim());

            }), exists: false);
            this.pageFacebook = pageFacebook;
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
            pageFacebook.LoadFolders();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            uiTextBox1.Text = "";
            LoadFolders(uiTextBox1.Text, txtType.Text.Trim());
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            LoadFolders(uiTextBox1.Text, txtType.Text.Trim());
        }

        private void txtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFolders(uiTextBox1.Text, txtType.Text.Trim());
        }
    }
}
