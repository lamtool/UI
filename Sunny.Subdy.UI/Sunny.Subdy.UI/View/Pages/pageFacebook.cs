using System.Threading.Tasks;
using OpenCvSharp;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.UI.View.Controls;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pageFacebook : UIPage
    {
        private FolderContext _folderContext;
        private ScriptContext _scriptContext;
        private ucdgvAccount _ucdgvAccount;
        public pageFacebook()
        {
            InitializeComponent();
            this.Symbol = 161570;

            _folderContext = new FolderContext();
            _scriptContext = new ScriptContext();
            _ucdgvAccount = new ucdgvAccount(null);
            _ucdgvAccount.Dock = DockStyle.Fill;
            panel2.Controls.Add(_ucdgvAccount);
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, action: new System.Action(() =>
            {
                LoadScripts();
                LoadFolders();

            }), exists: false);
        }
        private void LoadScripts()
        {
            cbx_Scripts.Items.Clear();
            var scripts = _scriptContext.GetByType("Facebook");
            foreach (var script in scripts)
            {
                cbx_Scripts.Items.Add(script.Name);
            }
            if (string.IsNullOrEmpty(cbx_Scripts.Text))
            {
                cbx_Scripts.SelectedIndex = 0;
            }
        }
        private void LoadFolders()
        {
            cbx_Folders.Items.Clear();
            var scripts = _folderContext.GetByType("Facebook");
            foreach (var script in scripts)
            {
                cbx_Folders.Items.Add(script.Name);
            }
            if (string.IsNullOrEmpty(cbx_Folders.Text))
            {
                cbx_Folders.SelectedIndex = 0;
            }
        }

        private void fFacebook_Load(object sender, EventArgs e)
        {

        }

        private void fFacebook_Initialize(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {

        }

        private async void cbx_Folders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ucdgvAccount != null && !string.IsNullOrEmpty(cbx_Folders.Text))
            {
                List<Data.Models.Folder> folders = new List<Data.Models.Folder>();
                if (cbx_Folders.Text == "Tất cả các nhóm")
                {

                }
                else if (cbx_Folders.Text == "Chọn nhiều nhóm")
                {
                    folders = _folderContext.GetAll();
                }
                else
                {
                    var folder = _folderContext.GetByName(cbx_Folders.Text.Trim());
                    if (folder != null)
                    {
                        folders.Add(folder);
                    }
                }
                _ucdgvAccount._folders = folders;
                await _ucdgvAccount.LoadAccount();
            }
        }
    }
}
