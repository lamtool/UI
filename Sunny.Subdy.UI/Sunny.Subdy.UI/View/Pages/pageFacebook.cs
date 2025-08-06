using AutoAndroid;
using Sunny.Subd.Core.Facebook;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Proxies;
using Sunny.Subd.Core.Services;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.View.Controls;
using Sunny.Subdy.UI.View.Forms;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pageFacebook : UIPage
    {
        private FolderContext _folderContext;
        private ScriptContext _scriptContext;
        public ucdgvAccount _ucdgvAccount;
        Sunny.UI.UINavMenu _mainTabControl;
        private pageDevice _formPhone;
        private CancellationTokenSource cancellationTokenSource;
        public pageFacebook(Sunny.UI.UINavMenu mainTabControl, pageDevice phone)
        {
            InitializeComponent();
            this.Symbol = 161570;
            _mainTabControl = mainTabControl;
            _formPhone = phone;
            _folderContext = new FolderContext();
            _scriptContext = new ScriptContext();
            _ucdgvAccount = new ucdgvAccount(null);
            _ucdgvAccount.Dock = DockStyle.Fill;
            panel2.Controls.Add(_ucdgvAccount);
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, onLoad: new System.Action(() =>
            {
                LoadScripts();
                LoadFolders();

            }), shouldExit: false);
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
        public void LoadFolders()
        {
            cbx_Folders.Items.Clear();
            cbx_Folders.Items.Add("Tất cả các nhóm");
            var scripts = _folderContext.GetByType("Facebook");
            foreach (var script in scripts)
            {
                cbx_Folders.Items.Add(script.Name);
            }
            if (string.IsNullOrEmpty(cbx_Folders.Text))
            {
                cbx_Folders.SelectedIndex = 0;
            }
            if (scripts.Any())
            {
                cbx_Folders.Items.Add("Chọn nhiều kịch bản");
            }
        }

        private void fFacebook_Load(object sender, EventArgs e)
        {

        }

        private void fFacebook_Initialize(object sender, EventArgs e)
        {

        }
        private void EnableControls(bool enable)
        {
            groupBox1.Enabled = enable;
            groupBox2.Enabled = enable;
            uiSymbolButton3.Enabled = enable;
            uiSymbolButton4.Enabled = !enable;
        }
        private async Task uiSymbolButton3_ClickSafe()
        {
            try
            {
                EnableControls(false);
                await Start();
                EnableControls(true);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            _ = uiSymbolButton3_ClickSafe();
        }
        private bool SelectPhone()
        {
            _formPhone.ManagerDevices.groupBox2.Visible = true;
            var control = _formPhone.ManagerDevices;
            fShow fShow = new fShow(control);
            var value = fShow.ShowDialog();
            _formPhone.Invoke(() =>
            {
                _formPhone.Controls.Clear();
                _formPhone.Controls.Add(control);
            });
            _formPhone.ManagerDevices.groupBox2.Visible = false;
            if (value == DialogResult.OK)
            {
                return true;
            }

            return false;
        }
        private async Task Start()
        {
            if (_ucdgvAccount._accounts == null || _ucdgvAccount._accounts.Where(x => x.Checked).Count() == 0)
            {
                CommonMethod.ShowMessageError("Vui lòng chọn tài khoản trước khi thực hiện.");
                return;
            }
            var model = GetConfigModel();
            if (model == null)
            {
                return;
            }
            if (!SelectPhone()) return;
            fMain.StartTime = DateTime.Now;
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = cancellationTokenSource.Token;
            List<Task> tasks = new List<Task>();
            AccountServices.Accounts.Clear();
            LoadControlModelHelper.ToolStripAccount = _ucdgvAccount.toolStrip1;
            AccountServices.Accounts = _ucdgvAccount._accounts.Where(x => x.Checked).ToList();
            foreach (var device in DeviceServices.DeviceModels.Where(x => x.Check))
            {
                tasks.Add(Task.Run(async () =>
                {
                    await RunningThread(ct, device, model);
                }));
            }
            await Task.WhenAll(tasks);
            fMain.StartTime = null;
        }

        private async Task RunningThread(CancellationToken ct, DeviceModel device, ConfigModel config)
        {
            ADBClient client = new ADBClient(device);
            MainService service = new MainService(PlatformModel.Facebook, client, config, ct);
            await service.RunAsync();
        }

        private ConfigModel GetConfigModel()
        {
            if (string.IsNullOrEmpty(cbx_Scripts.Text.Trim()))
            {
                CommonMethod.ShowMessageError("Vui lòng chọn kịch bản trước khi thực hiện.");
                return null;
            }
            ConfigModel model = new ConfigModel();
            model.Script = _scriptContext.GetByName(cbx_Scripts.Text.Trim());
            model.SettingGeneral = new JsonHelper(nameof(pageSetting), false);
            return model;
        }
        private async Task cbx_Folders_SelectedIndexChangedSafe()
        {
            try
            {
                if (_ucdgvAccount != null && !string.IsNullOrEmpty(cbx_Folders.Text))
                {
                    List<Data.Models.Folder> folders = new List<Data.Models.Folder>();
                    if (cbx_Folders.Text == "Tất cả các nhóm")
                    {
                        folders = _folderContext
             .GetAll()
             .Where(x => x.Type == "Facebook")
             .ToList();
                    }
                    else if (cbx_Folders.Text == "Chọn nhiều nhóm")
                    {

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
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void cbx_Folders_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ = cbx_Folders_SelectedIndexChangedSafe();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            _mainTabControl.SelectPage(7);
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            _mainTabControl.SelectPage(6);
        }

        private void cbx_Scripts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            uiSymbolButton4.Enabled = false;
        }
    }
}
