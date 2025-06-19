using System.Collections.Concurrent;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Threading;
using System.Threading.Tasks;
using AutoAndroid;
using Sunny.Subd.Core.Facebook;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Proxies;
using Sunny.Subd.Core.Services;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.View.Controls;
using Sunny.Subdy.UI.View.Forms;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pageFacebook : UIPage
    {
        private FolderContext _folderContext;
        private ScriptContext _scriptContext;
        private ucdgvAccount _ucdgvAccount;
        Sunny.UI.UINavMenu _mainTabControl;
        private pageDevice _formPhone;
        private CancellationTokenSource cancellationTokenSource;
        private System.Windows.Forms.Timer _timer;
        private DateTime startTime = DateTime.Now;
        private ConcurrentQueue<Account> _accountQueue = new ConcurrentQueue<Account>();
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
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, action: new System.Action(() =>
            {
                LoadScripts();
                LoadFolders();

            }), exists: false);
            _timer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // 1 second

            };
            _timer.Tick += Timer_Tick;
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (!IsHandleCreated || IsDisposed)
                    return;
                if (this.InvokeRequired)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        TimeSpan elapsedTime = DateTime.Now - startTime;
                        tsbTimeRun.Text = elapsedTime.ToString(@"hh\:mm\:ss");
                    }));
                }
                else
                {
                    TimeSpan elapsedTime = DateTime.Now - startTime;
                    tsbTimeRun.Text = elapsedTime.ToString(@"hh\:mm\:ss");
                }
            });

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
        private async void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            await Start();
            EnableControls(true);
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
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = cancellationTokenSource.Token;
            List<Task> tasks = new List<Task>();
            _timer.Start();
            _accountQueue.Clear();
            _accountQueue = new ConcurrentQueue<Account>(_ucdgvAccount._accounts.Where(x => x.Checked).ToList());
            foreach (var device in DeviceServices.DeviceModels.Where(x => x.Check))
            {
                tasks.Add(Task.Run(async () =>
                {
                    await RunningThread(ct, device, model);
                }));
            }
            await Task.WhenAll(tasks);
            _timer.Stop();
        }

        private async Task RunningThread(CancellationToken ct, DeviceModel device, ConfigModel config)
        {
            Random random = new Random();
            JsonHelper jsonHelper = new JsonHelper(config.JsonSetting);
            ADBClient client = new ADBClient(device);
            if (client == null) return;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!ct.IsCancellationRequested)
            {

                if (!client.Connect()) continue;

                if (!client.IsDeviceConnectedToInternet()) continue;

                if (_accountQueue.TryDequeue(out Account account))
                {
                    if (account == null)
                    {
                        continue;
                    }
                    /////////////CheckLIVE////////////////
                    if (jsonHelper.GetBooleanValue("checkBox9", true) && !await FacebookRequest.CheckLive(account.Uid))
                    {
                        account.State = "DIE";
                        account.Status = "Tài khoản đã chết, không thực hiện nữa.";
                        continue;
                    }
                    /////////////////////////////////////

                    ////////////////Change//////////////////
                    if (jsonHelper.GetBooleanValue("checkBox1", true))
                    {
                        List<string> bards = jsonHelper.GetValuesFromInputString("textBox1", DeviceServices.Brands).Split('|').ToList();
                        string filePath = string.Empty;
                        if (jsonHelper.GetBooleanValue("checkBox2", true))
                        {
                            string folder = jsonHelper.GetValuesFromInputString("textBox2", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup", "Device"));
                            Directory.CreateDirectory(folder);
                            filePath = Path.Combine(folder, $"{account.Uid}.tar.gz");
                        }
                        client.ChangInfo(filePath, jsonHelper.GetBooleanValue("checkBox2", true), bards[random.Next(bards.Count)]);
                    }
                    ///////////////////////////////////

                    ////////////Proxy//////////
                    string proxy = string.Empty;
                    var typeProxy = jsonHelper.GetIntType("cbb_ListTypeProxy", 0);
                    if (typeProxy < 0)
                    {
                        typeProxy = 0; // Default to "Không đổi IP"
                    }
                    string proxyType = ProxyService.Proxies[typeProxy];
                    switch (proxyType)
                    {
                        case "4G Mobile":
                            {
                                client.Disable4G();
                                client.Enabel4G();
                                break;
                            }
                        case "Proxy đã gán cho tài khoản":
                            {
                                proxy = account.Proxy;
                                break;
                            }
                        default:
                            break; // Fallback if no valid type is matched
                    }
                    if (!string.IsNullOrEmpty(proxy))
                    {
                        client.ConnectProxy(proxy);
                    }
                    ////////////Proxy//////////
                    int delayConnectProxy = jsonHelper.GetIntType("numericUpDown3", 10);
                    for (int i = 1; i <= delayConnectProxy; i++)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            break;
                        }
                        account.Status = $"Delay kết nối proxy [{i}/{delayConnectProxy}]";
                        client.Device.Status = $"Delay kết nối proxy [{i}/{delayConnectProxy}]";
                        await Task.Delay(1000);
                    }
                    /////////////////////////////////////
                    MainService service = new MainService("Facebook", client, account, config, ct);
                    try
                    {
                        account.Running = true;
                        await service.RunAsync();
                    }
                    catch (Exception ex)
                    {
                        HanderCase(ex, account);
                    }
                    finally
                    {
                        client.Shell("rm -rf /storage/emulated/0/*.jpg /storage/emulated/0/*.png /storage/emulated/0/*.jpeg");
                        client.Shell("rm -rf /storage/emulated/0/*.tar.gz");
                        client.Shell("rm -rf /sdcard/*.tar.gz");
                        client.AppClear(FacebookHander.Package());
                        client.Shell("input keyevent 223");
                        client.LogHelper.SUCCESS("Đã hoàn thành!");
                        account.Running = false;
                    }
                }



            }
        }
        private void HanderCase(Exception ex, Account account)
        {
            SubdyExtension subdyExtension = null;
            if (ex is SubdyExtension extension)
            {
                subdyExtension = extension;
            }
            else
            {
                subdyExtension = new SubdyExtension(SubdyEnum.Error, ex.Message);
            }
            switch (subdyExtension.SubdyEnum)
            {
                case SubdyEnum.Stop:
                    {
                        account.Status = "Đã dừng lại.";
                        break;
                    }
                case SubdyEnum.Error:
                    {
                        account.Status = "Lỗi: " + subdyExtension.Message;
                        break;
                    }
                case SubdyEnum.CP_282:
                    {
                        account.Status = "Lỗi CP_282: " + subdyExtension.Message;
                        account.State = "CP_282";
                        break;
                    }
                case SubdyEnum.CP_956:
                    {
                        account.Status = "Lỗi CP_956: " + subdyExtension.Message;
                        account.State = "CP_956";
                        break;
                    }
                case SubdyEnum.LogOut:
                    {
                        account.Status = "Đăng xuất: " + subdyExtension.Message;
                        account.State = "LogOut";
                        break;
                    }
                case SubdyEnum.Captcha:
                    {
                        account.Status = "Captcha: " + subdyExtension.Message;
                        account.State = "Captcha";
                        break;
                    }
                case SubdyEnum.Block:
                    {
                        account.Status = "Tài khoản bị chặn: " + subdyExtension.Message;
                        account.State = "Block";
                        break;
                    }
            }
            new AccountContext().Update(account);
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
            model.JsonSetting = SettingsTool.GetSettings(nameof(pageSetting), true).GetJsonString();
            return model;
        }
        private async void cbx_Folders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ucdgvAccount != null && !string.IsNullOrEmpty(cbx_Folders.Text))
            {
                List<Data.Models.Folder> folders = new List<Data.Models.Folder>();
                if (cbx_Folders.Text == "Tất cả các nhóm")
                {
                    folders = _folderContext.GetAll();
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

        }
    }
}
