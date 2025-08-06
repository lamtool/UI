using Sunny.Subd.Core.Gmail;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.API.Captchas;
using Sunny.Subdy.Common.API.Jobs.TuongTacCheo;
using Sunny.Subdy.Common.API.Jobs.VipIG;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.ControlViews.Convertes;
using Sunny.Subdy.UI.View.Forms;
using Sunny.Subdy.UI.View.Forms.Actions;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            uiDataGridView2.SelectionChanged += DataGridView_SelectionChanged;
            uiDataGridView2.CellFormatting += uiDataGridView1_CellFormatting;
            uiDataGridView2.AllowUserToResizeRows = false;
            ControlHelper.LoadConfigColums(uiDataGridView2, new List<string> { "IsView", "Running", "ColorType", "Id" });


            var folder = _folders?.First();
            if (folder != null && folder.Type == PlatformModel.Instagram)
            {
                LoadControlModelHelper.LoadToolStripAccount(folder.Type, "", "", toolStrip1, new JobHistoryContext());
            }
            if (folder != null && folder.Type == "TuongTacCheo")
            {
                tanToolStripMenuItem.Visible = true;
                List<string> list = new List<string>
                {
                    "Checked",
                    "Column1",
                    "uidDataGridViewTextBoxColumn",
                    "passwordDataGridViewTextBoxColumn",
                    "cookieDataGridViewTextBoxColumn",
                    "tokenDataGridViewTextBoxColumn",
                    "resultDataGridViewTextBoxColumn",
                    "stateDataGridViewTextBoxColumn",
                    "statusDataGridViewTextBoxColumn"
                };
                foreach (DataGridViewColumn column in uiDataGridView2.Columns)
                {
                    string name = column.Name;
                    column.Visible = list.Contains(name);
                }
            }
            else if (folder != null && folder.Type == "VipIG")
            {
                tanToolStripMenuItem.Text = "VipIG";
                tanToolStripMenuItem.Visible = true;
                getTokenToolStripMenuItem.Text = "Đăng nhập";
                kiểmTraXuToolStripMenuItem.Visible = false;
                getCookieToolStripMenuItem.Visible = false;
                đăngKíTàiKhoảnToolStripMenuItem.Visible = true;
                List<string> list = new List<string>
                {
                    "Checked",
                    "Column1",
                    "uidDataGridViewTextBoxColumn",
                    "passwordDataGridViewTextBoxColumn",
                    "cookieDataGridViewTextBoxColumn",
                    "tokenDataGridViewTextBoxColumn",
                    "resultDataGridViewTextBoxColumn",
                    "stateDataGridViewTextBoxColumn",
                    "statusDataGridViewTextBoxColumn"
                };
                foreach (DataGridViewColumn column in uiDataGridView2.Columns)
                {
                    string name = column.Name;
                    column.Visible = list.Contains(name);
                }
                toolStripButton1.Visible = false;
            }
        }

        private void UcdgvAccount_Load(object sender, EventArgs e)
        {

        }

        private void uiDataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var dgv = sender as Sunny.UI.UIDataGridView;
            var row = dgv.Rows[e.RowIndex];
            Color textColor = Color.Black;
            // Giả sử bạn có cột tên là "Status"
            if (row.Cells["ColorType"].Value != null && !string.IsNullOrEmpty(row.Cells["ColorType"].Value.ToString()) && int.TryParse(row.Cells["ColorType"].Value.ToString(), out int type))
            {
                switch (type)
                {
                    case 1:
                        textColor = Color.Red;
                        break;
                    case 2:
                        textColor = Color.Green;
                        break;
                    case 0:
                    default:
                        // Giữ màu mặc định
                        break;
                }
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.ForeColor = textColor;
                    cell.Style.SelectionForeColor = textColor; // Đặt màu chữ khi chọn
                }
            }
            if (row.Cells["ColorType"].Value != null && !string.IsNullOrEmpty(row.Cells["ColorType"].Value.ToString()) && bool.TryParse(row.Cells["ColorType"].Value.ToString(), out bool typeBlack))
            {
                if (typeBlack)
                {
                    row.DefaultCellStyle.BackColor = Color.MediumSpringGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
            if (row.Cells["Checked"].Value != null && !string.IsNullOrEmpty(row.Cells["Checked"].Value.ToString()) && bool.TryParse(row.Cells["Checked"].Value.ToString(), out bool check))
            {
                tslChecked.Text = $"{_accounts.Count(x => x.Checked)}";
            }



        }
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int selectedRowCount = uiDataGridView2.SelectedRows.Count;
            tslSelect.Text = selectedRowCount.ToString();
        }
        private async Task uiSymbolButton1_ClickSafe()
        {
            try
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
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            _ = uiSymbolButton1_ClickSafe();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            try
            {
                _ = LoadAccount();
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }

        }


        public async Task LoadAccount()
        {
            uiDataGridView2.Invoke(() => uiDataGridView2.DataSource = null);

            if (_folders == null || !_folders.Any()) return;

            var accounts = await Task.Run(() =>
                _accountContext.GetAll(_folders.Select(x => x.Name).ToList(), true)
            );

            if (accounts == null || !accounts.Any()) return;

            _accounts = accounts;

            uiDataGridView2.Invoke((Delegate)(() =>
            {
                var bindingList = new SortableBindingList<Account>(_accounts);
                uiDataGridView2.DataSource = bindingList;

                for (int i = 0; i < uiDataGridView2.Rows.Count; i++)
                {
                    var row = uiDataGridView2.Rows[i];
                    if (!row.IsNewRow && row.Cells["Column1"] != null)
                        row.Cells["Column1"].Value = (i + 1).ToString();
                }

                var stateCounts = _accounts
                    .GroupBy(x => x.State)
                    .Select(g => (g.Key ?? "UNKNOWN", g.Count()))
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
            }));
            _accounts.Where(x => x.State == "LIVE").ToList().ForEach(x => x.ColorType = 2);
            _accounts.Where(x => x.State == "DIE").ToList().ForEach(x => x.ColorType = 1);
            _accounts.Where(x => x.State != "LIVE" && x.State != "DIE").ToList().ForEach(x => x.ColorType = 0);
        }
        private void ucdgvAccount_Load(object sender, EventArgs e)
        {
            try
            {
                _ = LoadAccount();
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }

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
                    string state = menuItem.Text.Split("(").FirstOrDefault().Trim();
                    _accounts.ForEach(x => x.Checked = x.State?.ToLower().Contains(state.ToLower()) == true);
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

        private void tắtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fCopyFields f = new fCopyFields();
            f.ShowDialog();
            if (f.IsOk)
            {
                List<string> types = new();
                foreach (System.Windows.Forms.ComboBox cbx in f.cbxs)
                {
                    types.Add(cbx.Text);
                }
                ConvertHelper.CopyFormat(string.Join("|", types), uiDataGridView2);
            }
        }

        private void mởToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", Fields.Uid), uiDataGridView2);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", Fields.Password), uiDataGridView2);
        }

        private void fAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", Fields._2FA), uiDataGridView2);
        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", Fields.Email), uiDataGridView2);
        }

        private void mậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", Fields.PassMail), uiDataGridView2);
        }

        private void cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", Fields.Cookie), uiDataGridView2);
        }

        private void tokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", Fields.Token), uiDataGridView2);
        }

        private void proxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", Fields.Token), uiDataGridView2);
        }

        private void trạngTháiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertHelper.CopyFormat(string.Join("|", "Status"), uiDataGridView2);
        }

        private void satesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private async Task facebookToolStripMenuItem_ClickSafe()
        {
            var accounts = _accounts.Where(x => x.Checked).ToList();
            if (!accounts.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng chọn tài khoản để kiểm tra.");
                return;
            }

            facebookToolStripMenuItem.Enabled = false;

            try
            {
                List<string> lines = new List<string>();

                foreach (var account in accounts)
                {
                    if (_folders.First().Type == "Gmail" && !account.Uid.Contains("@gmail.com"))
                    {
                        account.Uid += "@gmail.com";
                    }

                    lines.Add(account.Uid);
                }
                if (_folders.First().Type == "Gmail")
                {
                    var values = await GmailRequest.CheckEmailsAsync(lines);
                    foreach (var account in accounts)
                    {
                        if (values.TryGetValue(account.Uid, out var status))
                        {
                            account.State = status.ToUpper();
                            account.Status = $"{DateTime.Now:HH dd/MM/yyyy} - {account.State}";
                        }
                        else
                        {
                            account.State = "UNKNOWN";
                            account.Status = $"{DateTime.Now:HH dd/MM/yyyy} - UNKNOWN";
                        }
                    }
                }
                else if (_folders.First().Type == "Facebook")
                {
                    // TODO: Thêm xử lý kiểm tra Facebook
                }
            }
            finally
            {
                facebookToolStripMenuItem.Enabled = true;
            }
        }

        private void facebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = facebookToolStripMenuItem_ClickSafe();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var excludedHeaders = new HashSet<string> { "Chọn", "#", "Uid", "Status", "IsView", "Running", "ColorType", "Id" };

            var remainingHeaders = uiDataGridView2.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => !excludedHeaders.Contains(c.HeaderText))
                .Select(c => c.HeaderText)
                .ToList();
            fViewDataGridView f = new fViewDataGridView(remainingHeaders, uiDataGridView2.Name);
            f.ShowDialog();
            ControlHelper.LoadConfigColums(uiDataGridView2, new List<string> { "IsView", "Running", "ColorType", "Id" });
        }

        private void getCookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = getCookieToolStripMenuItem_ClickSafe("cookie");
        }
        private async Task getCookieToolStripMenuItem_ClickSafe(string method)
        {
            var accounts = _accounts.Where(x => x.Checked).ToList();
            if (!accounts.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng chọn tài khoản để kiểm tra.");
                return;
            }

            facebookToolStripMenuItem.Enabled = false;
            try
            {
                if (tanToolStripMenuItem.Text == "TuongTacCheo")
                {
                    var tuongtaccheo = new TuongTacCheoClient();
                    foreach (var account in accounts)
                    {

                        try
                        {
                            if (string.IsNullOrEmpty(account.Token))
                            {
                                account.Status = "Đang lấy token";
                                account.Token = await tuongtaccheo.GetTokenByUsername(account.Uid, account.Password);
                            }
                            if (string.IsNullOrEmpty(account.Token))
                            {
                                throw new Exception("Không có token.");
                            }
                            if (method == "token")
                            {
                                _accountContext.Update(account);
                                continue;
                            }
                            if (method == "cookie")
                            {
                                account.Status = "Đang lấy cookie";
                                account.Cookie = await tuongtaccheo.GetCookie(account.Token);
                                _accountContext.Update(account);
                                continue;
                            }
                            if (method == "coin")
                            {
                                account.Status = "Đang lấy coin";
                                account.Result = await tuongtaccheo.GetCoin(account.Token);
                                _accountContext.Update(account);
                                continue;
                            }
                            _accountContext.Update(account);
                        }
                        catch (Exception ex)
                        {
                            account.Status = ex.Message;
                            account.ColorType = 1;
                        }

                    }
                }
                else if (tanToolStripMenuItem.Text == "VipIG")
                {
                    var tuongtaccheo = new VipIGClient();
                    foreach (var account in accounts)
                    {
                        string result = string.Empty;
                        try
                        {
                            if (string.IsNullOrEmpty(account.Token))
                            {
                                account.Status = "Đang lấy token";
                                result = await tuongtaccheo.LoginByUsername(account.Uid, account.Password);
                            }
                            else
                            {
                                result = await tuongtaccheo.LoginByToken(account.Token);
                            }
                            if (string.IsNullOrEmpty(result))
                            {
                                throw new Exception("Đăng nhập thất bại.");
                            }
                            string[] parts = result.Split('|');
                            account.Cookie = parts[2];
                            account.Result = parts[1];
                            account.Status = parts[0];
                            if (string.IsNullOrEmpty(account.Token))
                            {
                                account.Token = parts[3];
                            }
                            account.ColorType = 2;
                            _accountContext.Update(account);
                        }
                        catch (Exception ex)
                        {
                            account.Status = ex.Message;
                            account.ColorType = 1;
                        }

                    }
                }


            }
            finally
            {
                facebookToolStripMenuItem.Enabled = true;
            }
        }

        private void getTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = getCookieToolStripMenuItem_ClickSafe("token");
        }

        private void kiểmTraXuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = getCookieToolStripMenuItem_ClickSafe("coin");
        }

        private void cậpNhậtTokenJobServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            foreach (DataGridViewRow row in uiDataGridView2.SelectedRows)
            {
                if (row.DataBoundItem is Account account)
                {
                    ids.Add(account.Id.ToString());
                }
            }
            if (!ids.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng select dòng cần cập nhật.");
                return;
            }
            fUpdateData f = new fUpdateData(ids, fUpdateData.TokenJob);
            f.ShowDialog();
            _ = LoadAccount();
        }

        private void đăngKíTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAction_RegVipIG f = new fAction_RegVipIG();
            if (f.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            _ = đăngKíTàiKhoảnToolStripMenuItem_ClickSafe();
        }
        private async Task đăngKíTàiKhoảnToolStripMenuItem_ClickSafe()
        {
            int count = SettingsTool.GetSettings(nameof(fAction_RegVipIG)).GetIntType("numericUpDown1", 1);
            string key = SettingsTool.GetSettings(nameof(fAction_RegVipIG)).GetValuesFromInputString("textBox1", "");
            string password = SettingsTool.GetSettings(nameof(fAction_RegVipIG)).GetValuesFromInputString("textBox2", "LamTool.net");
            if (string.IsNullOrEmpty(key))
            {
                CommonMethod.ShowMessageWarning("Vui lòng nhập keycaptcha");
                return;
            }
            VipIGClient client = new VipIGClient();
            while (count > 0)
            {
                count--;
                Account account = new Account();
                account.NameFolder = _folders.FirstOrDefault().Name;
                account.Id = Guid.NewGuid();
                BindingList<Account> accounts = (BindingList<Account>)uiDataGridView2.DataSource;
                accounts.Add(account);

                try
                {
                    string username = SubdyHelper.RandomString("abcdefghijklmnopqrstuvwxyz0123456789", SubdyHelper.RandomValue(5, 30)) + "@lamtool.net";

                    account.Status = "Đang đăng ký tài khoản.";
                    string siteKey = await client.GetSiteKey();
                    if (string.IsNullOrEmpty(siteKey))
                    {
                        account.Status = "Đang đăng ký tài khoản thất bại, không lấy được sitekey.";
                        continue;
                    }
                    account.Status = "Tạo job giải captcha.";
                    string id = await GuruCaptchaClient.GetIdCaptchaV2(key, siteKey, "https://vipig.net/index.php");
                    if (id.Contains("ERROR"))
                    {
                        account.Status = $"Tạo job giải captcha thất bại. [{id}]";
                        continue;
                    }
                    string token = string.Empty;
                    account.Status = $"Đang giải captcha";
                    for (int i = 0; i < 120; i++)
                    {
                        token = await GuruCaptchaClient.GetTokenCaptchaV2(key, id);
                        if (token.Contains("ERROR"))
                        {

                            await Task.Delay(1000);
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (token.Contains("ERROR"))
                    {
                        account.Status = $"Giải captcha thất bị. [{token}]";
                        continue;
                    }
                    account.Status = "Đang đăng ký tài khoản.";
                    string value = await client.Register(username, password, token);

                    if (!value.Contains("Đăng ký thành công"))
                    {
                        account.Status = $"Đăng kí tài khoản thất bại. [{value}]";
                        continue;
                    }
                    account.Uid = username;
                    account.Password = password;
                    account.Status = $"Đăng nhập tài khoản.";
                    value = await client.LoginByUsername(username, password);
                    if (string.IsNullOrEmpty(value))
                    {
                        account.Status = $"Đăng nhập tài khoản thất bại.";
                        continue;
                    }
                    account.Cookie = value.Split('|')[2];
                    account.Token = value.Split('|')[3];
                    account.Status = $"Acti tài khoản.";
                    if (await client.CauHinh("zackdfilms"))
                    {
                        account.Status = $"Acti tài khoản thành công.";

                    }
                    else
                    {
                        account.Status = $"Acti tài khoản thất bại.";
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                }
                finally
                {
                    if (!string.IsNullOrEmpty(account.Uid))
                    {
                        string message = $"[{account.Uid}] Trừ 3 xu đăng kí tài khoản vipig.net.";
                        string status = LamToolClient.SubtractBalance(Globals.User.UserName, 1, message);
                        if (status.Contains("error"))
                        {
                            CommonMethod.ShowMessageError(status);
                            count = -1;
                        }
                        _accountContext.Add(account);

                    }

                }

            }
            CommonMethod.ShowMessageSuccess("Đã đăng ký tài khoản thành công.");
        }

        private void cậpNhậtDữLiệuHàngLoạtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            foreach (DataGridViewRow row in uiDataGridView2.SelectedRows)
            {
                if (row.DataBoundItem is Account account)
                {
                    ids.Add(account.Id.ToString());
                }
            }
            if (!ids.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng select dòng cần cập nhật.");
                return;
            }
            fAddAccount f = new fAddAccount();
            f.ShowDialog();
            _ = LoadAccount();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void xóaTkVĩnhViễnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Guid> ids = new List<Guid>();
            foreach (DataGridViewRow row in uiDataGridView2.SelectedRows)
            {
                if (row.DataBoundItem is Account account)
                {
                    ids.Add(account.Id);
                }
            }
            if (!ids.Any())
            {
                CommonMethod.ShowMessageWarning("Vui lòng select dòng cần cập nhật.");
                return;
            }
            if (!CommonMethod.ShowConfirmWarning($"Bạn có chắc chắn muốn xóa {ids.Count} tài khoản."))
            {
                return;
            }
            if (_accountContext.DeleteByIds(ids))
            {
                CommonMethod.ShowMessageSuccess($"Đã xóa thành công.");
            }
            else
            {
                CommonMethod.ShowMessageWarning($"Đã xảy ra lỗi.");
            }
            _ = LoadAccount();
        }
    }
}
