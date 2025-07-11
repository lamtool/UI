using AutoAndroid;
using Sunny.Subd.Core.Facebook;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Phone;
using Sunny.Subd.Core.Proxies;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using System.Diagnostics;

namespace Sunny.Subd.Core.Gmail
{
    public class GmailRegsiner
    {
        private readonly Random _random = new Random();
        private readonly DeviceModel _device;
        private readonly JsonHelper _settingRegsiner;
        private readonly JsonHelper _settingGeneral;
        private readonly CancellationToken _ct;
        private readonly ADBClient _client;
        private Account _account;
        private readonly string _typeRegister;
        private readonly GmailService _gmailService;
        private readonly int _timeOut;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Folder _folder;
        private readonly PhoneService _phoneService;
        private AccountContext _accountContext = new AccountContext();
        private bool _isNVR = false;
        public GmailRegsiner(DeviceModel device, JsonHelper settingRegsiner, JsonHelper settingGeneral, CancellationToken ct, Folder folder)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _settingRegsiner = settingRegsiner ?? throw new ArgumentNullException(nameof(settingRegsiner));
            _settingGeneral = settingGeneral ?? throw new ArgumentNullException(nameof(settingGeneral));
            _ct = ct;
            _folder = folder ?? throw new ArgumentNullException(nameof(folder));

            _client = new ADBClient(_device);
            int index = _settingRegsiner.GetIntType("uiComboBox1", 0);
            _typeRegister = RegistrationType.RegGmail_AllTypes[index];

            _timeOut = _settingRegsiner.GetIntType("numericUpDown1", 30) * 1000 * 60;
            _gmailService = new GmailService(_client);

            string sitePhone = RegistrationType.PhoneNumberTypes[_settingRegsiner.GetIntType("comboBox1", 0)];
            string tokenPhone = _settingRegsiner.GetValuesFromInputString("textBox1", string.Empty).Trim();
            _phoneService = new PhoneService(sitePhone, tokenPhone);

        }
        private void writeFile(string message)
        {
            string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
            string logFolderPath = Path.Combine("Logs_Create", dateFolder, _folder.Type);

            Directory.CreateDirectory(logFolderPath);

            string liveFilePath = Path.Combine(logFolderPath, "live.txt");
            string dieFilePath = Path.Combine(logFolderPath, "die.txt");
            using (StreamWriter liveWriter = new StreamWriter(liveFilePath, true))
            using (StreamWriter dieWriter = new StreamWriter(dieFilePath, true))
            {
                if (message.Contains("LIVE"))
                {
                    liveWriter.WriteLine(message); // ghi vào file
                }
                else
                {
                    dieWriter.WriteLine(message); // ghi vào file
                }
            }
        }
        public async Task RegisterAsync()
        {
            bool save = false;
            while (!_ct.IsCancellationRequested)
            {
                if (!await ConnectAndPrepareDevice()) continue;

                if (!_gmailService.RemoveAccount()) continue;

                _account = GetAccount();

                _stopwatch.Restart();
                string logAccount = string.Empty;

                try
                {
                    _client.Shell("am force-stop com.android.settings");
                    _client.Delay(3);
                    _client.Shell("am start -a android.settings.SYNC_SETTINGS");
                    var message = await ImportInfo();
                    if (message.SubdyEnum == SubdyEnum.Success&& !string.IsNullOrEmpty(_account.Email) && !string.IsNullOrEmpty(_account.Uid))
                    {
                        _account.Id = Guid.NewGuid();
                        if (!_account.Uid.Contains("@gmail.com"))
                        {
                            _account.Uid += "@gmail.com";
                        }
                        else if (!_account.Email.Contains("@gmail.com"))
                        {
                            _account.Email += "@gmail.com";
                        }
                        _account.State = "LIVE";
                        _account.Status = "Đăng ký thành công!";
                        string status = LamToolClient.SubtractBalance(Globals.User.UserName, 10, $"[{_account.Uid}] Tạo tài khoản thành công.");
                        save = !status.Contains("error");
                        if (save)
                        {
                            _accountContext.Add(_account);
                        }

                    }
                }
                catch (Exception ex)
                {
                    _account.State = "DIE";
                    LogManager.Error(ex);
                }
                if (!_account.Uid.Contains("@gmail.com"))
                {
                    _account.Uid += "@gmail.com";
                }
                else if (!_account.Email.Contains("@gmail.com"))
                {
                    _account.Email += "@gmail.com";
                }
                if (_account.State == "LIVE" && !save)
                {
                    _account.Uid = "*****";
                    _account.Email = "******";
                }
                logAccount = $"{_account.Uid}|{_account.Password}|{_account.TowFA}|{_account.Email}|{_account.PassMail}|{_account.Cookie}|{_account.Token}|{_account.State}|{_account.Status}";
                LogManager.LogRegsiner.Add(logAccount);
                writeFile(logAccount);
            }
            _device.Status = "Đã hoàn thành.";
        }
        private List<string> XpathsGmailRegsiner = new List<string>
        {   
            
            "//*[@text=\"MORE\"]",
            "//*[@text=\"ACCEPT\"]",
            "//*[@text=\"Review your account info\"]",
            "//*[@text=\"Add phone number?\"]",
            "//*[@text=\"This phone number cannot be used for verification.\"]",
            "//*[@text=\"Enter the code\"]",
            "//*[@text=\"Privacy and Terms\"]",
            "//*[@text=\"Sorry, we could not create your Google Account.\"]",
            "//*[@text=\"There was a problem verifying your phone number\"]",
            "//*[@text=\"Create your own Gmail address\"]",
            "//*[@resource-id=\"domainSuffix\"]",
            "//*[@resource-id=\"month-label\"]",
            "//*[@resource-id=\"month\"]",
            "//*[@text=\"For my personal use\"]",
            "//*[@resource-id=\"phoneNumberId\"]",
            "//*[@text=\"Create account\"]",
            "//*[@resource-id=\"password\"]",
            "//*[@resource-id=\"firstName\"]",
            "//*[@text=\"ACCEPT\"]",
            "//*[@text=\"Google\"]",
            "//*[@text=\"Add account\"]",
            "//*[@text=\"NEXT\"]",


        };
        private List<string> _Moth = new List<string>
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December",
        };
        private List<string> _Gender = new List<string>
        {
            "Male",
            "Female",
        };
        private bool EnsureAppRunning()
        {
            if (!_client.IsRunningApp("com.android.settings") && !_client.IsRunningApp("com.google.android.gms"))
            {
                _client.Shell("am force-stop com.android.settings");
                _client.Shell("am start -a android.settings.SYNC_SETTINGS");
                _client.Delay(5);
                return false;
            }
            return true;
        }
        private async Task<SubdyExtension> ImportInfo()
        {
            string currentCase = string.Empty;
            while (_stopwatch.ElapsedMilliseconds < _timeOut && !_ct.IsCancellationRequested)
            {
                if (!EnsureAppRunning()) continue;

                currentCase = _client.FindElement("", XpathsGmailRegsiner, 120);
                if (string.IsNullOrEmpty(currentCase))
                {
                    _client.Shell("am force-stop com.android.settings");
                    _client.Delay(3);
                    _client.Shell("am start -a android.settings.SYNC_SETTINGS");
                    _client.Delay(5);
                    continue;
                }
                _client.LogHelper.SUCCESS($"Đang xử lý case [{currentCase}]...");
                switch (currentCase)
                {
                    case "//*[@text=\"More\"]":
                    case "//*[@text=\"ACCEPT\"]":
                    case "//*[@text=\"NEXT\"]":
                    case "//*[@text=\"Add account\"]":
                    case "//*[@text=\"Google\"]":
                    case "//*[@text=\"Create account\"]":
                    case "//*[@text=\"For my personal use\"]":
                        {
                            _client.ElementWithAttributes(currentCase);
                            _client.Delay(2);
                            break;
                        }
                    case "//*[@text=\"Review your account info\"]":
                        {
                            _client.ElementWithAttributes("//*[@text=\"NEXT\"]");
                            _client.Delay(2);
                            break;
                        }
                    case "//*[@text=\"Add phone number?\"]":
                        {
                            _client.Swipe(543, 1394, 555, 654, 200, 5);
                            _client.Delay(2);
                            _client.ElementWithAttributes("//*[@text=\"Yes, I’m in\"]");
                            _client.Delay(5);
                            break;
                        }
                    case "//*[@text=\"Privacy and Terms\"]":
                        {
                            _client.Swipe(543, 1394, 555, 654, 200, 5);
                            _client.Delay(2);
                            _client.ElementWithAttributes("//*[@text=\"I agree\"]");
                            _client.Delay(5);
                            break;
                        }
                    case "//*[@text=\"Enter the code\"]":
                        {
                            string code = await GetCode();
                            if (string.IsNullOrEmpty(code))
                            {
                                _client.LogHelper.ERROR("Không nhận được mã xác nhận.");
                                throw new SubdyExtension(SubdyEnum.Stop, "Không nhận được mã xác nhận.");
                            }
                            _client.SendTextADB($"//*[@resource-id=\"code\"]", code, timeout: 5);
                            _client.ElementWithAttributes("//*[@text=\"Next\"]");
                            _client.Delay(2);
                            break;
                        }
                    case "//*[@resource-id=\"firstName\"]":
                        {
                            HandleNameInput();
                            break;
                        }
                    case "//*[@resource-id=\"month\"]":
                    case "//*[@resource-id=\"month-label\"]":
                        {
                            HandleDatePicker();
                            break;
                        }
                    case "//*[@text=\"Create your own Gmail address\"]":
                    case "//*[@resource-id=\"domainSuffix\"]":
                        {
                            HandleEmailInput();
                            break;
                        }
                    case "//*[@resource-id=\"password\"]":
                        {
                            HandlePasswordInput();

                            break;
                        }
                    case "//*[@resource-id=\"phoneNumberId\"]":
                        {
                            await HandlePhoneInput();
                            break;
                        }
                    case "//*[@text=\"This phone number cannot be used for verification.\"]":
                    case "//*[@text=\"There was a problem verifying your phone number\"]":
                    case "//*[@text=\"Sorry, we could not create your Google Account.\"]":
                        {
                            return new SubdyExtension(SubdyEnum.CP_282, currentCase);
                        }
                }
                if (CheckSucess())
                {
                    return new SubdyExtension(SubdyEnum.Success, "Tạo tài khoản thành công.");
                }
            }

            return new SubdyExtension(SubdyEnum.Error, "Đã xảy ra lỗi khi đăng ký.");
        }
        private bool CheckSucess()
        {
            var accounts = _gmailService.GetAccount();
            foreach (var account in accounts)
            {
                if (string.IsNullOrEmpty(account) || !account.ToLower().Contains(_account.Email.ToLower())) continue;
                return true;
            }
            return false;
        }
        private void HandleNameInput()
        {
            if (!_client.ElementWithAttributes("//*[@resource-id=\"firstName\"]", click: false) || !_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), click: false)) return;

            bool swap = _random.Next(0, 2) == 1;
            string[] nameParts = _account.FullName.Split(' ');
            string firstName = nameParts[0];
            string lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";
            if (swap) (firstName, lastName) = (lastName, firstName);

            var elements = _client.FindElements(10, "", "//*[@class=\"android.widget.EditText\"]");
            if (!elements.Any()) return;

            if (_random.Next(0, 2) == 1)
            {
                _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", firstName, timeout: 5, xml: elements[0].OuterXml);
                if (elements.Count > 1)
                    _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", lastName, timeout: 5, xml: elements[1].OuterXml);
            }
            else
            {
                _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", lastName, timeout: 5, xml: elements[0].OuterXml);
                if (elements.Count > 1)
                    _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", firstName, timeout: 5, xml: elements[1].OuterXml);
            }
            _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
            _client.Delay(5);
        }
        private void HandleDatePicker()
        {
            if (!_client.ElementWithAttributes(new List<string> { "//*[@resource-id=\"month\"]", "//*[@resource-id=\"month-label\"]" }, 1, click: true)) return;
            List<string> moths = SubdyHelper.Shuffle(_Moth);
            foreach (var moth in moths)
            {
                if (_client.ElementWithAttributes($"//*[@text=\"{moth}\"]", 1)) break;
            }

            _client.SendTextADB($"//*[@resource-id=\"day\"]", _random.Next(1, 28).ToString(), timeout: 5);
            _client.SendTextADB($"//*[@resource-id=\"year\"]", _random.Next(1976, 2005).ToString(), timeout: 5);
            _client.ElementWithAttributes(new List<string> { "//*[@resource-id=\"gender\"]", "//*[@resource-id=\"gender-label\"]" });
            _client.ElementWithAttributes($"//*[@text=\"{_Gender[_random.Next(0, _Gender.Count)]}\"]");
            _client.ElementWithAttributes("//*[@text=\"NEXT\"]");
            _client.Delay(5);
        }
        private void HandleEmailInput()
        {
            if (!_client.ElementWithAttributes(new List<string> { "//*[@text=\"Create your own Gmail address\"]", "//*[@resource-id=\"domainSuffix\"]" }, timeoutInSeconds: 1, click: false)) return;
            _client.ElementWithAttributes("//*[@text=\"Create your own Gmail address\"]");
            _account.Email = randomMail();
            _account.Uid = _account.Email;
            _client.SendTextADB($"//*[@class=\"android.widget.EditText\"]", _account.Email, timeout: 5);
            _client.ElementWithAttributes("//*[@text=\"Next\"]");
            _client.Delay(5);
        }
        private string randomMail()
        {
            string email = string.Empty;
            email = SubdyHelper.RemoveSpecialAndVietnameseChars(_account.FullName.Split(" ").First());
            if (_random.Next(2) == 1)
            {
                email += ".";
            }
            email += SubdyHelper.RandomString("abcdefghijklmnopqrstuvwxyz0123456789", _random.Next(7, 22));
            return email;
        }
        private void HandlePasswordInput()
        {
            _client.Delay(2);
            if (!_client.ElementWithAttributes("//*[@resource-id=\"password\"]", timeoutInSeconds: 1, click: false) || !_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), timeoutInSeconds: 1, click: false)) return;
            _client.SendTextSlow($"//*[@class=\"android.widget.EditText\"]", _account.Password, timeout: 5);
            _client.Delay(2);
            _client.ElementWithAttributes("//*[@text=\"NEXT\"]");
            _client.Delay(5);
        }
        private async Task HandlePhoneInput()
        {
            _client.Delay(5);
            if (!_client.ElementWithAttributes("//*[@resource-id=\"phoneNumberId\"]") || !_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), timeoutInSeconds: 1, click: false)) return;
            switch (_typeRegister)
            {
                case RegistrationType.PhoneNumber:
                    await GetPhone();
                    if (string.IsNullOrEmpty(_account.Phone)) return;
                    if (_account.Phone.Contains("ERROR"))
                    {
                        _client.LogHelper.ERROR(_account.Phone);
                        await Task.Delay(5000);
                        return;
                    }
                    string rawPhone = _account.Phone;
                    if (_account.Phone.Contains("|"))
                    {
                        rawPhone = _account.Phone.Split('|')[1].Trim();
                    }
                    if (!rawPhone.StartsWith("1") && !rawPhone.StartsWith("0") && !rawPhone.StartsWith("84") && !rawPhone.StartsWith("+84"))
                    {
                        rawPhone = "+84" + rawPhone;
                    }
                    else if (!rawPhone.StartsWith("1") && !rawPhone.StartsWith("0") && !rawPhone.StartsWith("+")) // Đã có 84 nhưng thiếu "+"
                    {
                        rawPhone = "+" + rawPhone;
                    }
                    else if (rawPhone.StartsWith("1") && !rawPhone.StartsWith("+")) // Đã có 84 nhưng thiếu "+"
                    {
                        rawPhone = "+" + rawPhone;
                    }

                    _client.SendTextSlow($"//*[@class=\"android.widget.EditText\"]", rawPhone, timeout: 5);
                    _client.ElementWithAttributes("//*[@text=\"Next\"]");
                    _client.Delay(10);
                    break;
                default:
                    throw new SubdyExtension(SubdyEnum.Error, "VeriPhone");
            }
            //_client.Delay(10);
        }


        private async Task<bool> ConnectAndPrepareDevice()
        {
            if (!_client.Connect()) return false;

            _client.SetSize();

            ChangeInfo();

            await ChangeProxy();

            return IsInternet();
        }
        private bool IsInternet()
        {
            int attempts = _settingGeneral.GetIntType("nud_IndexFailProxy", 5);
            for (int i = 0; i < attempts; i++)
            {
                if (_client.IsDeviceConnectedToInternet()) return true;
            }
            return true;
        }
        private void ChangeInfo()
        {
            if (!_settingGeneral.GetBooleanValue("checkBox1", true)) return;
            try
            {
                List<string> brands = _settingGeneral.GetValuesFromInputString("textBox1", DeviceServices.Brands).Split('|').ToList();
                _client.ChangInfo("", false, brands[_random.Next(brands.Count)], SubdyHelper.Countries[_settingGeneral.GetIntType("cbbScript", 0)]);
            }
            catch (Exception ex)
            {

            }

        }
        private Account GetAccount()
        {
            _account = new Account();
            List<string> firstnames = GetFirstnames();
            List<string> lastnames = GetLastnames();

            _account.Password = GetPassword();
            _account.NameFolder = _folder.Name;
            _account.FullName = $"{SubdyHelper.GetStringRandom(firstnames)} {SubdyHelper.GetStringRandom(lastnames)}";

            return _account;
        }
        private async Task ChangeProxy()
        {
            _client.Shell("settings put global http_proxy :0");
            string proxy = string.Empty;
            var proxyType = ProxyService.NoIP;
            try
            {
                proxyType = ProxyService.ProxyTypes[_settingGeneral.GetIntType("cbb_ListTypeProxy", 0)];
            }
            catch (Exception ex)
            {

            }
            switch (proxyType)
            {
                case ProxyService.NoIP:
                case ProxyService.ProxyAssigned:
                    return;
                case ProxyService.Mobile4G:
                    _client.EnabePlane();
                    _client.Disable4G();
                    break;
                case ProxyService.KiotProxy:
                    proxy = await GetProxyAsync(ProxyKiot.NewProxy, ProxyKiot.GetProxy);
                    break;
                case ProxyService.WWProxy:
                    proxy = await GetProxyAsync(ProxyWWW.NewProxy, ProxyWWW.GetProxy);
                    break;
                case ProxyService.ProxyMart:
                    proxy = await GetProxyAsync(ProxyMart.NewProxy, ProxyMart.GetProxy);
                    break;
                case ProxyService.CustomProxy:
                    proxy = await GetCustomProxyAsync();
                    break;
                case ProxyService.ProxyFile:
                    {
                        proxy = ProxyService.GetProxy();
                        break;
                    }
                default:
                    return;
            }

            if (!string.IsNullOrEmpty(proxy))
            {
                _client.ConnectProxy(proxy);
            }


            _client.Delay(_settingGeneral.GetIntType("numericUpDown3", 10));
            if (proxyType == ProxyService.Mobile4G)
            {
                _client.DisablePlane();
                _client.Enabel4G();
                _client.Delay(5);
            }

        }
        private async Task<string> GetProxyAsync(Func<string, Task<string>> newProxyFunc, Func<string, Task<string>> getProxyFunc)
        {
            string key = ProxyService.GetProxy();
            string proxy = await newProxyFunc(key) ?? await getProxyFunc(key);
            return proxy;
        }
        private async Task<string> GetCustomProxyAsync()
        {
            string line = ProxyService.GetProxy();
            if (string.IsNullOrEmpty(line)) return string.Empty;

            var parts = line.Split('|');
            string proxy = parts[0].Trim();
            string link = parts.Length > 1 ? parts[1].Trim() : string.Empty;

            if (!string.IsNullOrEmpty(link)) await RequestService.Get(link);
            return proxy;
        }
        private List<string> GetFirstnames()
        {
            if (_settingRegsiner.GetBooleanValue("radioButton1", false))
                return SubdyHelper.FirstnameRandom;
            if (_settingRegsiner.GetBooleanValue("radioButton3", false))
                return File.Exists(_settingRegsiner.GetValuesFromInputString("txt_Ho", string.Empty))
                    ? File.ReadAllLines(_settingRegsiner.GetValuesFromInputString("txt_Ho", string.Empty)).ToList()
                    : SubdyHelper.FirstnameVN;
            return SubdyHelper.FirstnameVN;
        }
        private List<string> GetLastnames()
        {
            if (_settingRegsiner.GetBooleanValue("radioButton1", false))
                return SubdyHelper.LastnameRandom;
            if (_settingRegsiner.GetBooleanValue("radioButton3", false))
                return File.Exists(_settingRegsiner.GetValuesFromInputString("txt_Ten", string.Empty))
                    ? File.ReadAllLines(_settingRegsiner.GetValuesFromInputString("txt_Ten", string.Empty)).ToList()
                    : SubdyHelper.LastnameVN;
            return SubdyHelper.LastnameVN;
        }
        private string GetPassword()
        {
            if (_settingRegsiner.GetBooleanValue("radioButton2", false) && !string.IsNullOrEmpty(_settingRegsiner.GetValuesFromInputString("txtPass", "")))
                return _settingRegsiner.GetValuesFromInputString("txtPass", "").Trim();
            return SubdyHelper.RandomPassword(SubdyHelper.RandomValue(9, 18), digit: false);
        }
        private async Task<string> GetPhone()
        {
            string phone = _typeRegister switch
            {
                RegistrationType.PhoneNumber => await _phoneService.GetPhone("gmail"),
                _ => string.Empty
            };

            if (!string.IsNullOrEmpty(phone))
            {
                _account.Phone = phone;
                return phone;
            }

            _client.LogHelper.ERROR("Không nhận được số điện thoại.");
            return string.Empty;
        }
        private async Task<string> GetCode()
        {
            int timeout = 180;
            int interval = 2000;
            DateTime startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalSeconds < timeout)
            {
                string code = _typeRegister switch
                {
                    RegistrationType.PhoneNumber => await _phoneService.GetCode(_account.Phone?.Split("|")[0]),
                    _ => string.Empty
                };

                if (!string.IsNullOrEmpty(code)) return code;

                _client.LogHelper.ERROR("Không nhận được mã xác nhận.");
                await Task.Delay(interval);
            }

            return string.Empty;
        }
    }
}
