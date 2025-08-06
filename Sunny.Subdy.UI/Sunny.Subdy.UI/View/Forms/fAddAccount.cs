using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Forms
{
    public partial class fAddAccount : Form
    {
        private FormatAccountContext _formatAccountContext;
        List<ComboBox> cbxs = new List<ComboBox>();
        private bool _add = true;
        private Folder _fol;
        public fAddAccount(Folder folder)
        {
            InitializeComponent();
            _fol = folder;
            _add = true;
            _formatAccountContext = new FormatAccountContext();
            LoadFormats();
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, onLoad: new System.Action(() =>
            {
                LoadCombobox();
                txtLines.Text = "";

            }), shouldExit: false);

            List<Control> controls = new List<Control>();
            var lines = Globals.GetFieldsToImportExport();
        }
        public fAddAccount()
        {
            InitializeComponent();
            _add = false;
            _formatAccountContext = new FormatAccountContext();
            LoadFormats();
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, onLoad: new System.Action(() =>
            {
                LoadCombobox();
                txtLines.Text = "";

            }), shouldExit: false);

            List<Control> controls = new List<Control>();
            var lines = Globals.GetFieldsToImportExport();
        }
        private void LoadFormats()
        {
            txtType.Items.Clear();
            var formats = _formatAccountContext.GetAll();
            txtType.Items.Add("Mặc định");
            if (formats != null && formats.Count > 0)
            {
                foreach (var format in formats)
                {
                    txtType.Items.Add(format.Name);
                }
            }

        }
        private void LoadCombobox()
        {
            if (string.IsNullOrEmpty(txtType.Text))
            {
                txtType.SelectedIndex = 0;
            }
            var formats = _formatAccountContext.GetByName(txtType.Text);
            var fields = formats == null ? string.Empty : formats.Fields;
            List<string> listField = Globals.GetFieldsToImportExport();
            for (int i = 0; i < listField.Count - 1; i++)
            {
                ComboBox cbx = new ComboBox();
                cbx.DropDownStyle = ComboBoxStyle.DropDownList;
                cbx.Width = 90;
                cbx.Items.AddRange(listField.ToArray());
                if (i < listField.Count - 1)
                {
                    cbx.SelectedIndex = i + 1;
                }
                cbx.SelectedValueChanged += cbx_SelectedIndexChanged;
                cbxs.Add(cbx);
            }

            ControlHelper.LoadFormatFrom(fields, cbxs);
            cbxs[0].Items.Clear();
            cbxs[0].Items.Add(Fields.Uid);
            cbxs[0].SelectedIndex = 0;

            flowLayoutPanel1.Controls.AddRange(cbxs.ToArray());
        }
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            string value = string.Empty;
            if (this.ShowInputStringDialog(ref value, false, "Nhập tên nhóm:"))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    CommonMethod.ShowMessageWarning("Tên nhóm không được để trống.");
                    return;
                }
                var existingFormat = _formatAccountContext.GetByName(value);
                if (existingFormat != null)
                {
                    CommonMethod.ShowMessageError("Tên nhóm đã tồn tại, vui lòng chọn tên khác.");
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
                    CommonMethod.ShowMessageSuccess("Thêm nhóm thành công.");
                    LoadFormats();
                    LoadCombobox();
                }
                else
                {
                    CommonMethod.ShowMessageError("Thêm nhóm thất bại.");
                }
            }

        }
        private void SaveCombobox()
        {
            List<string> listFormats = cbxs.Select(cbx => cbx.Text).ToList();
            string formattedString = string.Join("|", listFormats);
            var formats = _formatAccountContext.GetByName(txtType.Text);
            if (formats == null)
            {
                _formatAccountContext.Add(new FormatAccount
                {
                    Id = Guid.NewGuid(),
                    Name = txtType.Text,
                    Fields = formattedString
                });
            }
            else
            {
                formats.Fields = formattedString;
                _formatAccountContext.Update(formats);
            }
        }
        private async Task uiSymbolButton1_ClickSafe()
        {
            try
            {
                uiSymbolButton2.Enabled = false;
                uiSymbolButton1.Enabled = false;
                SaveCombobox();
                this.ShowWaitForm(desc: "Đang lưu thông tin vui lòng đợi");
                List<string> lines = new List<string>();
                if (string.IsNullOrEmpty(txtLines.Text.Trim()))
                {
                    this.HideWaitForm();
                    CommonMethod.ShowMessageWarning("Danh sách tài khoản không được để trống.");
                    return;
                }
                lines = txtLines.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
                if (_add)
                {
                    await AddAccounts(lines);
                    _fol.Count = new AccountContext().GetAll(new List<string> { _fol.Name }, true).Count.ToString();
                    new FolderContext().Update(_fol);
                }
                else
                {
                    await UpdateAccounts(lines);
                    
                }
                this.HideWaitForm();

            }
            catch (Exception ex)
            {
                this.HideWaitForm();
                CommonMethod.ShowMessageError(ex.Message);
            }
            uiSymbolButton1.Enabled = true;
            uiSymbolButton2.Enabled = true;
        }
        private async Task UpdateAccounts(List<string> lines)
        {
            List<Account> accounts = new List<Account>();
            AccountContext accountContext = new AccountContext();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                Account account = new Account();
                string[] parts = line.Split('|');

                for (int i = 0; i < cbxs.Count && i < parts.Length; i++)
                {
                    string field = cbxs[i].SelectedItem?.ToString() ?? string.Empty;
                    string value = parts[i].Trim();

                    switch (field)
                    {
                        case Fields.Uid:
                            account.Uid = value;
                            break;
                        case Fields.Password:
                            account.Password = value;
                            break;
                        case Fields.Phone:
                            account.Phone = value;
                            break;
                        case Fields._2FA:
                            account.TowFA = value;
                            break;
                        case Fields.Cookie:
                            account.Cookie = value;
                            break;
                        case Fields.Token:
                            account.Token = value;
                            break;
                        case Fields.Proxy:
                            account.Proxy = value;
                            break;
                        case Fields.Email:
                            account.Email = value;
                            break;
                        case Fields.PassMail:
                            account.PassMail = value;
                            break;
                        case Fields.UserAgent:
                            account.UserAgent = value;
                            break;
                        case Fields.Username:
                            account.UserName = value;
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(account.Uid))
                {
                    var accountNew = accountContext.GetByUid(account.Uid);
                    if (accountNew != null)
                    {
                        if (!string.IsNullOrEmpty(account.Password)) accountNew.Password = account.Password;
                        if (!string.IsNullOrEmpty(account.Phone)) accountNew.Phone = account.Phone;
                        if (!string.IsNullOrEmpty(account.TowFA)) accountNew.TowFA = account.TowFA;
                        if (!string.IsNullOrEmpty(account.Cookie)) accountNew.Cookie = account.Cookie;
                        if (!string.IsNullOrEmpty(account.Token)) accountNew.Token = account.Token;
                        if (!string.IsNullOrEmpty(account.Proxy)) accountNew.Proxy = account.Proxy;
                        if (!string.IsNullOrEmpty(account.Email)) accountNew.Email = account.Email;
                        if (!string.IsNullOrEmpty(account.PassMail)) accountNew.PassMail = account.PassMail;
                        if (!string.IsNullOrEmpty(account.UserAgent)) accountNew.UserAgent = account.UserAgent;
                        if (!string.IsNullOrEmpty(account.UserName)) accountNew.UserName = account.UserName;
                        accounts.Add(accountNew);
                    }
                }
            }
            if (accounts.Count > 0)
            {
                if (accountContext.Update(accounts))
                {
                    this.HideWaitForm();
                    CommonMethod.ShowMessageSuccess($"Đã cập nhật {accounts.Count} tài khoản.");
                    this.Close();
                }
                else
                {
                    this.HideWaitForm();
                    CommonMethod.ShowMessageError("Cập nhật tài khoản thất bại.");
                }
            }
            else
            {
                this.HideWaitForm();
                CommonMethod.ShowMessageError("Không có tài khoản nào đã được lưu để cập nhật");
            }
        }
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            _ = uiSymbolButton1_ClickSafe();
        }
        private async Task AddAccounts(List<string> lines)
        {
            List<Account> accounts = new List<Account>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                Account account = new Account();
                string[] parts = line.Split('|');

                for (int i = 0; i < cbxs.Count && i < parts.Length; i++)
                {
                    string field = cbxs[i].SelectedItem?.ToString() ?? string.Empty;
                    string value = parts[i].Trim();

                    switch (field)
                    {
                        case Fields.Uid:
                            account.Uid = value;
                            break;
                        case Fields.Password:
                            account.Password = value;
                            break;
                        case Fields.Phone:
                            account.Phone = value;
                            break;
                        case Fields._2FA:
                            account.TowFA = value;
                            break;
                        case Fields.Cookie:
                            account.Cookie = value;
                            break;
                        case Fields.Token:
                            account.Token = value;
                            break;
                        case Fields.Proxy:
                            account.Proxy = value;
                            break;
                        case Fields.Email:
                            account.Email = value;
                            break;
                        case Fields.PassMail:
                            account.PassMail = value;
                            break;
                        case Fields.UserAgent:
                            account.UserAgent = value;
                            break;
                        case Fields.Username:
                            account.UserName = value;
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(account.Uid) || !string.IsNullOrEmpty(account.Email))
                {
                    account.Uid = string.IsNullOrEmpty(account.Uid) ? account.Email : account.Uid;
                    account.NameFolder = _fol.Name;
                    account.Id = Guid.NewGuid();
                    accounts.Add(account);
                }
            }
            var accountContext = new AccountContext();
            var accountsOld = accountContext.GetAll(new List<string> { _fol.Name }, true);
            var oldUids = new HashSet<string>(accountsOld.Select(a => a.Uid));
            var accountsToAdd = accounts
                .Where(a => !string.IsNullOrEmpty(a.Uid) && !oldUids.Contains(a.Uid))
                .ToList();
            if (accountsToAdd.Count > 0)
            {
                if (accountContext.AddRange(accountsToAdd))
                {
                    this.HideWaitForm();
                    CommonMethod.ShowMessageSuccess($"Đã thêm {accountsToAdd.Count} tài khoản mới vào nhóm '{_fol.Name}'.");
                }
                else
                {
                    this.HideWaitForm();
                    CommonMethod.ShowMessageError("Thêm tài khoản thất bại.");
                }
            }
            else
            {
                this.HideWaitForm();
                CommonMethod.ShowMessageError("Dữ liệu bị trùng.");
            }
        }
        private void cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cbx)
            {
                // Nếu SelectedItem là rỗng, không nên xử lý tiếp
                if (cbx.SelectedItem == null || string.IsNullOrWhiteSpace(cbx.SelectedItem.ToString()))
                    return;

                // Tìm combo khác có cùng giá trị được chọn
                var cbx1 = cbxs
                    .Where(c => c != cbx && c.SelectedItem?.ToString() == cbx.SelectedItem.ToString())
                    .FirstOrDefault();

                if (cbx1 != null)
                {
                    cbx1.SelectedItem = Fields.Empty;
                }
            }
        }
        private void txtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCombobox();
        }
        private void txtLines_TextChanged(object sender, EventArgs e)
        {
            label1.Text = $"Danh sách tài khoản ({txtLines.Lines.Count()}):";
            if (string.IsNullOrEmpty(txtLines.Lines.FirstOrDefault()))
            {
                return;
            }
            SelecCombobox(txtLines.Lines.FirstOrDefault());
        }
        private void SelecCombobox(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return;
            }
            var lines = data.Split('|');
            if (!lines.Any())
            {
                return;
            }
            try
            {
                foreach (var item in cbxs)
                {
                    item.SelectedItem = Fields.Empty;
                }
                for (int i = 0; i < Math.Min(lines.Length, cbxs.Count); i++)
                {
                    if (string.IsNullOrEmpty(lines[i]))
                    {
                        cbxs[i].SelectedItem = Fields.Empty;
                        continue;
                    }
                    Regex regex = new Regex(@"^\d+$");
                    if (regex.IsMatch(lines[i]))
                    {
                        cbxs[i].SelectedItem = Fields.Uid;
                        cbxs[i + 1].SelectedItem = Fields.Password;
                        continue;
                    }
                    else
                    {
                        if (lines[i].Contains("@") && lines[i].Contains(".com"))
                        {
                            cbxs[i].SelectedItem = Fields.Email;
                            cbxs[i + 1].SelectedItem = Fields.PassMail;
                            continue;
                        }
                        if (lines[i].Contains(":"))
                        {
                            cbxs[i].SelectedItem = Fields.Proxy;
                            continue;
                        }
                        if (lines[i].Contains(";"))
                        {
                            cbxs[i].SelectedItem = Fields.Cookie;
                            continue;
                        }
                        if (lines[i].Contains("@") && lines[i].Contains("."))
                        {
                            cbxs[i].SelectedItem = Fields.MailAdress;
                            continue;
                        }
                        if (lines[i].Count(c => !char.IsWhiteSpace(c)) == 32)
                        {
                            cbxs[i].SelectedItem = Fields._2FA;
                            continue;
                        }
                        if (lines[i].StartsWith("EAA"))
                        {
                            cbxs[i].SelectedItem = Fields.Token;
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            SaveCombobox();
            this.Close();
        }
    }
}
