using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.ControlViews.Convertes;
using Sunny.Subdy.UI.View.Forms;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class ucdgvAccount : UserControl
    {
        public List<Folder> _folders;
        private AccountContext _accountContext;
        public List<Account> _accounts;
        public ucdgvAccount(List<Folder> folders)
        {
            InitializeComponent();
            _folders = folders;
            _accountContext = new AccountContext();
            uiDataGridView2.AutoGenerateColumns = false;
            _accounts = new List<Account>();
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
            _accounts = _accountContext.GetAll(_folders.Select(x => x.Name).ToList(), true);
            if (_accounts == null || !_accounts.Any())
            {
                return;
            }
            SortableBindingList<Account> bindingList = new SortableBindingList<Account>(_accounts);
            uiDataGridView2.DataSource = bindingList;
            for (int i = 0; i < uiDataGridView2.Rows.Count; i++)
            {
                var row = uiDataGridView2.Rows[i];
                if (!row.IsNewRow && row.Cells["Column1"] != null)
                    row.Cells["Column1"].Value = (i + 1).ToString();
            }
            var stateCounts = _accounts
      .GroupBy(x => x.State)
      .Select(g => (g.Key ?? "UNKNOWN", g.Count())) // tránh null
      .ToList();

            var menuItems = AddSate("State", stateCounts);
            satesToolStripMenuItem.DropDownItems.Clear();
            satesToolStripMenuItem.DropDownItems.AddRange(menuItems.ToArray());
            int otherCount = 0;

            foreach (var stateCount in stateCounts)
            {
                switch (stateCount.Item1)
                {
                    case "LIVE":
                        uiLabel4.Text = stateCount.Item2.ToString();
                        break;
                    case "DIE":
                        uiLabel6.Text = stateCount.Item2.ToString();
                        break;
                    default:
                        otherCount += stateCount.Item2;
                        break;
                }
            }
            uiLabel8.Text = otherCount.ToString();
            uiLabel1.Text = _accounts.Count.ToString();

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
        private List<ToolStripMenuItem> AddSate(string type, List<(string, int)> items)
        {
            List<ToolStripMenuItem> toolStripMenuItems = new List<ToolStripMenuItem>();
            int i = 0;
            foreach (var item in items)
            {
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem
                {
                    Name = $"{type}_{i}",
                    Text = $"{item.Item1} ({item.Item2})",
                    Tag = item.Item1,
                    Image = Properties.Resources.done_all_30
                };

                toolStripMenuItem.Click += toolStripMenuItem_Click;
                toolStripMenuItems.Add(toolStripMenuItem);
                i++;
            }
            return toolStripMenuItems;
        }
        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                string[] parts = menuItem.Name.Split('_');
                if (parts.Length == 2 && parts[0] == "State")
                {
                    string state = parts[1];
                    uiDataGridView2.DataSource = _accountContext.GetAll($"SELECT * FROM Account WHERE State = '{state}'");
                }
            }
        }

        private void tấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _accounts.ForEach(x => x.Checked = true);
        }

        private void bôiĐenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _accounts.ForEach(x => x.Checked = false);
            foreach (DataGridViewRow row in uiDataGridView2.SelectedRows)
            {
                if (row.DataBoundItem is Account account)
                {
                    account.Checked = true;
                }
            }
        }

        private void bỏChọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _accounts.ForEach(x => x.Checked = false);
        }
    }
}
