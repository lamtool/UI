using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.View.Forms;
using System.ComponentModel;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class ucdgvAccount : UserControl
    {
        public List<Folder> _folders;
        private AccountContext _accountContext;
        public ucdgvAccount(List<Folder> folders)
        {
            InitializeComponent();
            _folders = folders;
            _accountContext = new AccountContext();
        }

        private async void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (_folders == null || !_folders.Any())
            {
                CommonMethod.ShowMessageSuccess("Không có thư mục nào để thêm tài khoản.");
                return;
            }
            if (_folders.Count > 1)
            {
                CommonMethod.ShowMessageSuccess("Chỉ thêm tài khoản vào 1 folder duy nhất.");
                return;
            }
            fAddAccount fAddAccount = new fAddAccount(_folders.First());
            fAddAccount.ShowDialog();
            await LoadAccount();
        }

        private async void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            await LoadAccount();
        }

        public async Task LoadAccount()
        {
            uiDataGridView2.DataSource = null;
            if (_folders == null || !_folders.Any()) return;
            var accountsOld = _accountContext.GetAll(_folders.Select(x => x.Name).ToList(), true);
            if (accountsOld == null || !accountsOld.Any())
            {
                return;
            }
            BindingList<Account> bindingList = new BindingList<Account>(accountsOld);
            uiDataGridView2.DataSource = bindingList;
            for (int i = 0; i < uiDataGridView2.Rows.Count; i++)
            {
                var row = uiDataGridView2.Rows[i];
                if (!row.IsNewRow && row.Cells["Column1"] != null)
                    row.Cells["Column1"].Value = (i + 1).ToString();
            }
            uiLabel1.Text = accountsOld.Count.ToString();
            uiLabel4.Text = accountsOld.Where(x => x.State == "LIVE").Count().ToString();
            uiLabel6.Text = accountsOld.Where(x => x.State == "DIE").Count().ToString();
            uiLabel8.Text = (Convert.ToInt32(uiLabel1.Text) - (Convert.ToInt32(uiLabel4.Text) + Convert.ToInt32(uiLabel6.Text))).ToString();
        }

        private async void ucdgvAccount_Load(object sender, EventArgs e)
        {
            await LoadAccount();
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
