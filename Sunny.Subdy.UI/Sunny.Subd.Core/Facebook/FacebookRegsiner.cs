using AutoAndroid;
using Sunny.Subd.Core.Email;
using Sunny.Subd.Core.Gmail;
using Sunny.Subd.Core.Models;
using Sunny.Subd.Core.Phone;
using Sunny.Subd.Core.Proxies;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Sunny.Subdy.Data.Context;
    using Sunny.UI;

    public class FacebookRegsiner
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
        private readonly EmailService _emailService;
        private AccountContext _accountContext = new AccountContext();
        public FacebookRegsiner(DeviceModel device, JsonHelper settingRegsiner, JsonHelper settingGeneral, CancellationToken ct, Folder folder)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _settingRegsiner = settingRegsiner ?? throw new ArgumentNullException(nameof(settingRegsiner));
            _settingGeneral = settingGeneral ?? throw new ArgumentNullException(nameof(settingGeneral));
            _ct = ct;
            _folder = folder ?? throw new ArgumentNullException(nameof(folder));

            _client = new ADBClient(_device);
            _typeRegister = RegistrationType.AllTypes[_settingRegsiner.GetIntType("uiComboBox1", 0)];
            _timeOut = _settingRegsiner.GetIntType("numericUpDown1", 30) * 1000 * 60;
            _gmailService = new GmailService(_client);

            string sitePhone = RegistrationType.PhoneNumberTypes[_settingRegsiner.GetIntType("comboBox1", 0)];
            string tokenPhone = _settingRegsiner.GetValuesFromInputString("textBox1", string.Empty).Trim();
            _phoneService = new PhoneService(sitePhone, tokenPhone);

            string siteEmail = RegistrationType.EmailTypes[_settingRegsiner.GetIntType("cbb_Email", 0)];
            _emailService = new EmailService(siteEmail);
        }

        public async Task RegisterAsync()
        {
            while (!_ct.IsCancellationRequested)
            {
                if (!await ConnectAndPrepareDevice()) continue;


                _account = GetAccount();

                if (!await HandleInitialLogin()) continue;

                _stopwatch.Restart();
                string logAccount = string.Empty;
                try
                {

                    var message = await ImportInfo();
                    if (message.SubdyEnum == SubdyEnum.Success)
                    {
                        _account.State = "LIVE";
                        _account.Status = "Đăng ký thành công!";
                        _accountContext.Add(_account);
                    }
                }
                catch (Exception ex)
                {
                    _account.State = "DIE";
                    LogManager.Error(ex); // Ghi log lỗi để dễ debug
                }
                string line = FacebookHander.GetAuthenticationInfo(_client);
                if (!string.IsNullOrEmpty(line) && line.Contains("|"))
                {
                    string[] lines = line.Split('|');
                    _account.Uid = lines[0];
                    _account.Cookie = lines[1];
                    _account.Token = lines[2];
                }
                logAccount = $"{_account.Uid}|{_account.Password}|{_account.TowFA}|{_account.Email}|{_account.PassMail}|{_account.Cookie}|{_account.Token}|{_account.State}|{_account.Status}";
                LogManager.LogRegsiner.Add(logAccount);
            }
            _device.Status = "Đã hoàn thành.";
        }

        private async Task<bool> ConnectAndPrepareDevice()
        {
            if (!_client.Connect()) return false;

            _client.AppClear(FacebookHander.Package());

            _client.GrantAppPermissions(FacebookHander.Package());

            _client.SetSize();

            ChangeInfo();

            await ChangeProxy();

            return IsInternet();
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
            return SubdyHelper.RandomPassword(SubdyHelper.RandomValue(7, 18));
        }

        private void ChangeInfo()
        {
            if (!_settingGeneral.GetBooleanValue("checkBox1", true)) return;

            List<string> brands = _settingGeneral.GetValuesFromInputString("textBox1", DeviceServices.Brands).Split('|').ToList();
            _client.ChangInfo("", false, brands[_random.Next(brands.Count)], SubdyHelper.Countries[_settingGeneral.GetIntType("cbbScript", 0)]);
        }

        private async Task ChangeProxy()
        {
            _client.Shell("settings put global http_proxy :0");
            string proxy = string.Empty;
            var proxyType = ProxyService.ProxyTypes[_settingGeneral.GetIntType("cbb_ListTypeProxy", 0)];
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

            if (string.IsNullOrEmpty(proxy)) return;

            _client.ConnectProxy(proxy);
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

        private bool IsInternet()
        {
            int attempts = _settingGeneral.GetIntType("nud_IndexFailProxy", 5);
            for (int i = 0; i < attempts; i++)
            {
                if (_client.IsDeviceConnectedToInternet()) return true;
            }
            return true;
        }

        private async Task<bool> HandleInitialLogin()
        {
            switch (_typeRegister)
            {
                case RegistrationType.Gmail_BaitPhoneNumber:
                case RegistrationType.Gmail:
                    return await LoginGmailAsync();
                default:
                    return true;
            }
        }

        private async Task<bool> LoginGmailAsync()
        {
            if (GmailService.Gmails.Count == 0) return false;

            string value;
            lock (GmailService.Gmails)
            {
                value = GmailService.Gmails[0];
                GmailService.Gmails.RemoveAt(0);
            }

            if (string.IsNullOrEmpty(value)) return false;

            string[] parts = value.Split('|');
            string email = parts[0].Trim();
            string password = parts.Length > 1 ? parts[1].Trim() : string.Empty;
            _account.Email = email;
            _account.PassMail = password;

            return _gmailService.Login(email, password);
        }

        private async Task<SubdyExtension> ImportInfo()
        {
            string currentCase = string.Empty;
            List<string> caseFacebooks = FacebookHander.Regsiner_Facebook();
            while (_stopwatch.ElapsedMilliseconds < _timeOut && !_ct.IsCancellationRequested)
            {
                if (!EnsureAppRunning()) continue;

                currentCase = _client.FindElement("", caseFacebooks, 120);
                if (string.IsNullOrEmpty(currentCase))
                {
                    _client.AppStart(FacebookHander.Package(), true, true, true);
                    _client.Delay(5);
                    continue;
                }

                _client.LogHelper.SUCCESS($"Đang xử lý case [{currentCase}]...");

                if (IsConfirmationCase(currentCase))
                {
                    await HandleConfirmationCode();
                    continue;
                }

                switch (currentCase)
                {
                    case var c when XpathManager.Get(XpathType.Loading).Contains(c):
                        break;
                    case var c when XpathManager.Get(XpathType.ExistEmail).Contains(c):
                        return new SubdyExtension(SubdyEnum.EmailExist, "Đã có tài khoản thêm mail này rồi.");
                    case "//*[@text=\"Sign up with mobile number\"]":
                    case "//*[@text=\"Email của bạn là gì?\"]":
                    case "//*[@text=\"What's your email?\"]":
                        await HandleEmailInput();
                        break;
                    case "//*[@text=\"What is your mobile number?\"]":
                    case "//*[@text=\"Sign up with email\"]":
                        await HandlePhoneInput();
                        break;
                    case "//*[@text=\"Select your name\"]":
                    case "//*[@text=\"Choose your name\"]":
                        HandleNameSelection();
                        break;
                    case var c when XpathManager.Get(XpathType.Success).Contains(c):
                        return new SubdyExtension(SubdyEnum.Success, "Đăng ký thành công!");
                    case "//*[@text=\"I agree\"]":
                        _client.ElementWithAttributes(currentCase, 5);
                        return await Agreement();
                    case var c when XpathManager.Get(XpathType.NavigationButton).Contains(c):
                    case var x when XpathManager.Get(XpathType.Confim_Register).Contains(x):
                        _client.ElementWithAttributes(currentCase, 5);
                        break;
                    case "//*[@text=\"Enter the confirmation code\"]":
                        await HandleConfirmationCode();
                        break;
                    case "//*[@text=\"What's your name?\"]":
                        HandleNameInput();
                        break;
                    case "//*[@text=\"When is your date of birth?\"]":
                        HandleDateOfBirth();
                        break;
                    case "//*[@text=\"SET\"]":
                    case "//*[@text=\"Set date\"]":
                        HandleDatePicker();
                        break;
                    case "//*[@text=\"Male\"]":
                    case "//*[@text=\"Female\"]":
                    case "//*[@text=\"What is your gender?\"]":
                        HandleGenderSelection();
                        break;
                    case "//*[@text=\"Create a password\"]":
                        HandlePasswordInput();
                        break;
                }
            }

            return new SubdyExtension(SubdyEnum.Error, "Đã xảy ra lỗi khi đăng ký.");
        }

        private bool EnsureAppRunning()
        {
            if (!_client.IsRunningApp(FacebookHander.Package()))
            {
                _client.AppStart(FacebookHander.Package(), true, true, true);
                _client.Delay(5);
                return false;
            }
            return true;
        }

        private bool IsConfirmationCase(string currentCase)
        {
            return currentCase == $"//*[contains(@text, \"confirm your account\") and contains(@text, \"{_account.Email}\")]" ||
                   currentCase == $"//*[contains(@text, \"confirm your account\") and contains(@text, \"{_account.Phone?.Split("|")[0]}\")]";
        }

        private async Task HandleConfirmationCode()
        {
            _client.Delay(10);
            string code = await GetCode();
            if (string.IsNullOrEmpty(code))
            {
                _client.LogHelper.ERROR("Không nhận được mã xác nhận.");
                throw new SubdyExtension(SubdyEnum.Stop, "Không nhận được mã xác nhận.");
            }

            if (_typeRegister == RegistrationType.Gmail_BaitPhoneNumber || _typeRegister == RegistrationType.Gmail)
            {
                _client.Shell("input keyevent KEYCODE_APP_SWITCH");
                _client.ElementWithAttributes("//*[@content-desc=\"Facebook\"]");
            }

            _client.Delay(2);
            _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", code, timeout: 10);
            _client.ElementWithAttributes(new List<string> { "//*[@text=\"Next\"]", "//*[@content-desc=\"Next\"]" }, 5);
            _client.Delay(10);
        }

        private async Task HandleEmailInput()
        {
            if (!_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), timeoutInSeconds: 1, click: false)) return;
            switch (_typeRegister)
            {
                case RegistrationType.Domain_BaitPhoneNumber:
                case RegistrationType.Gmail_BaitPhoneNumber:
                case RegistrationType.PhoneNumber:
                    _client.ElementWithAttributes(new List<string> { "//*[@text=\"Sign up with mobile number\"]" }, 5);
                    break;
                default:
                    await GetEmail();
                    if (string.IsNullOrEmpty(_account.Email)) return;
                    _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", _account.Email.Split('|')[0], timeout: 5);
                    _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
                    break;
            }
        }

        private async Task HandlePhoneInput()
        {
            if (!_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), timeoutInSeconds: 1, click: false)) return;
            switch (_typeRegister)
            {
                case RegistrationType.Domain_BaitPhoneNumber:
                case RegistrationType.Gmail_BaitPhoneNumber:
                case RegistrationType.PhoneNumber:
                    await GetPhone();
                    if (string.IsNullOrEmpty(_account.Phone)) return;
                    _client.ElementWithAttributes(new List<string> { "//*[@text=\"Sign up with mobile number\"]" }, 5);

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


                    // Nếu chưa bắt đầu bằng 84 hoặc +84 → thêm vào
                    if (!rawPhone.StartsWith("84") && !rawPhone.StartsWith("+84"))
                    {
                        rawPhone = "+84" + rawPhone;
                    }
                    else if (!rawPhone.StartsWith("+")) // Đã có 84 nhưng thiếu "+"
                    {
                        rawPhone = "+" + rawPhone;
                    }

                    _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", rawPhone, timeout: 5);
                    _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
                    break;
                default:
                    _client.ElementWithAttributes(new List<string> { "//*[@text=\"Sign up with email\"]" }, 5);
                    break;
            }
            //_client.Delay(10);
        }

        private void HandleNameSelection()
        {
            if (!_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), timeoutInSeconds: 1, click: false)) return;
            _client.ElementWithAttributes("//*[@class=\"android.widget.RadioButton\"]", 5);
            _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
        }

        private void HandleNameInput()
        {
            if (!_client.ElementWithAttributes("//*[@class=\"android.widget.EditText\"]", click: false) || !_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), click: false)) return;

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
        }

        private void HandleDateOfBirth()
        {
            var elementsBirth = _client.FindElements(10, "", "//*[contains(@text, 'Date of birth') and contains(@text, 'years old')]");
            if (!elementsBirth.Any()) return;

            string dateText = elementsBirth.First().Attributes["text"].Value;
            int age = ExtractAgeFromText(dateText);
            if (age < 18)
                _client.ElementWithAttributes("//*[contains(@text, 'Date of birth') and contains(@text, 'years old')]", 5);
        }

        private int ExtractAgeFromText(string text)
        {
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(text);
            return match.Success ? Convert.ToInt32(match.Value) : 0;
        }

        private void HandleDatePicker()
        {
            if (!_client.ElementWithAttributes(_client.FindElement("", new List<string> { "//*[@text=\"SET\"]", "//*[@text=\"Next\"]" }, 5), 1, click: false)) return;

            var elementsDate = _client.FindBounds("", "//*[@resource-id=\"android:id/numberpicker_input\"]");
            if (elementsDate.Count != 3) return;

            for (int i = 0; i < elementsDate.Count; i++)
            {
                string element = elementsDate[i];
                int indexRandom = i == 2 ? 42 : 12;
                int indexMin = i == 2 ? 18 : 1;
                var point = new RectangleArea(element).GetCenterPoint();
                //var point = _client.FindPoint("//*[@resource-id=\"android:id/numberpicker_input\"]", 1, element);
                _client.Swipe(point.X, point.Y - 100, point.X, point.Y + 100, _random.Next(7, 12), _random.Next(indexMin, indexRandom));
            }

            _client.ElementWithAttributes(new List<string> { "//*[@text=\"SET\"]" }, 5);
            _client.Delay(2);
            _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
        }

        private void HandleGenderSelection()
        {
            if (!_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), click: false)) return;

            List<string> genderOptions = new List<string> { "//*[@text=\"Male\"]", "//*[@text=\"Female\"]" };
            if (_client.ElementWithAttributes(genderOptions[_random.Next(genderOptions.Count)], 5))
                _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
        }

        private void HandlePasswordInput()
        {
            if (!_client.ElementWithAttributes("//*[@class=\"android.widget.EditText\"]", timeoutInSeconds: 1, click: false) || !_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), timeoutInSeconds: 1, click: false)) return;

            _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", _account.Password, timeout: 5);
            _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
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
                    RegistrationType.Gmail_BaitPhoneNumber or RegistrationType.Gmail => _gmailService.GetCode(),
                    RegistrationType.PhoneNumber => await _phoneService.GetCode(_account.Phone?.Split("|")[0]),
                    RegistrationType.Domain or RegistrationType.Domain_BaitPhoneNumber => await _emailService.GetCode(_account.Email),
                    _ => string.Empty
                };

                if (!string.IsNullOrEmpty(code)) return code;

                _client.LogHelper.ERROR("Không nhận được mã xác nhận.");
                await Task.Delay(interval);
            }

            return string.Empty;
        }

        private async Task<string> GetEmail()
        {
            if (_typeRegister is RegistrationType.Domain_BaitPhoneNumber or RegistrationType.Domain)
            {
                string token = _settingRegsiner.GetValuesFromInputString("textBox2", string.Empty).Trim();
                _account.Email = await _emailService.GetEmail(token);
                return _account.Email;
            }
            return string.Empty;
        }

        private async Task<string> GetPhone()
        {
            string phone = _typeRegister switch
            {
                RegistrationType.PhoneNumber => await _phoneService.GetPhone(),
                RegistrationType.Gmail_BaitPhoneNumber or RegistrationType.Domain_BaitPhoneNumber => GetRandomPhoneNumber(),
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

        private string GetRandomPhoneNumber()
        {
            string country = SubdyHelper.Countries[_settingGeneral.GetIntType("cbbScript", 0)];
            string countryCode = country == "Random" ? "84" : country.Split('|')[1].Trim();
            return countryCode + SubdyHelper.RandomString("123456789", 9);
        }

        private async Task<SubdyExtension> Agreement()
        {
            List<string> xpaths = BuildAgreementXPaths();
            var listLogin = BuildLoginList(xpaths);

            while (_stopwatch.ElapsedMilliseconds < _timeOut && !_ct.IsCancellationRequested)
            {
                string currentCase = _client.FindElement("", listLogin, 120);
                if (string.IsNullOrEmpty(currentCase))
                {
                    _client.AppStart(FacebookHander.Package(), true, true, true);
                    _client.Delay(15);
                    continue;
                }

                if (IsConfirmationCase(currentCase))
                {
                    if (currentCase == $"//*[contains(@text, \"confirm your account\") and contains(@text, \"{_account.Phone?.Split("|")[0]}\")]")
                    {
                        switch (_typeRegister)
                        {
                            case RegistrationType.Gmail_BaitPhoneNumber:
                            case RegistrationType.Domain_BaitPhoneNumber:
                                {
                                    _client.ElementWithAttributes("//*[@text=\"I didn’t get the code\"]", 5);
                                    _client.Delay(5);
                                    if (!_client.ElementWithAttributes("//*[@text=\"Confirm by email\"]", 25)) continue;
                                    await GetEmail();
                                    if (string.IsNullOrEmpty(_account.Email))
                                    {
                                        _client.LogHelper.ERROR("Không nhận được email.");
                                        return new SubdyExtension(SubdyEnum.Error, "Không nhận được email.");
                                    }
                                    _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", _account.Email.Split('|')[0], timeout: 25);
                                    _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
                                    _client.Delay(10);
                                    break;
                                }
                        }
                    }



                    await HandleConfirmationCode();
                    continue;
                }

                switch (currentCase)
                {
                    case "//*[@text=\"I agree\"]":
                    case var x when XpathManager.Get(XpathType.NavigationButton).Contains(x):
                        _client.ElementWithAttributes(currentCase, 5);
                        _client.Delay(1);
                        break;
                    case var x when new List<string> { "//*[@text=\"I didn’t get the code\"]", "//*[@text=\"Confirm by email\"]" }.Contains(x):
                        if (_typeRegister is RegistrationType.Domain_BaitPhoneNumber or RegistrationType.Gmail_BaitPhoneNumber)
                        {
                            _client.ElementWithAttributes(currentCase, 5);
                            _client.Delay(5);
                            break;
                        }
                        await HandleConfirmationCode();
                        break;
                    case "//*[@text=\"Enter an email\"]":
                        await HandleEmailInputForAgreement();
                        break;
                    case "//*[@text=\"We couldn't create an account for you\"]":
                    case var x when XpathManager.Combine(XpathType.CP282, XpathType.CP956, XpathType.Captcha, XpathType.ExistEmail).Contains(x):
                        throw new SubdyExtension(SubdyEnum.CP_282, $"Tài khoản bị - [{currentCase}]");
                    case var x when XpathManager.Get(XpathType.Success).Contains(x):
                        _client.Delay(5);
                        if (!_client.ElementWithAttributes(XpathManager.Get(XpathType.Success), click: false)) continue;
                        return new SubdyExtension(SubdyEnum.Success, "LIVE");
                    case "//*[@text=\"Enter the confirmation code\"]":
                        {
                            await HandleConfirmationCode();
                            break;
                        }
                }
            }

            return new SubdyExtension(SubdyEnum.Error, "Đã xảy ra lỗi khi đăng ký.");
        }

        private List<string> BuildAgreementXPaths()
        {
            List<string> xpaths = new List<string>();
            if (!string.IsNullOrEmpty(_account.Email))
            {
                xpaths.Add($"//*[contains(@text, \"confirm your account\") and contains(@text, \"{_account.Email}\")]");

            }
            if (!string.IsNullOrEmpty(_account.Phone))
            {
                xpaths.Add($"//*[contains(@text, \"confirm your account\") and contains(@text, \"{_account.Phone?.Split("|")[0]}\")]");
            }
            xpaths.Add("//*[@text=\"We couldn't create an account for you\"]");
            xpaths.AddRange(new List<string>
        {
                "//*[@text=\"Enter an email\"]",
            "//*[@text=\"Couldn't create account\"]",
            "//*[@text=\"Enter email\"]",
            "//*[@text=\"Confirm with email\"]",
            "//*[@text=\"I didn't receive a code\"]",
            "//*[@text=\"I agree\"]",
            "//*[@text=\"Please log in again.\"]",
            "//*[@text=\"Sign up\"]",
            "//*[@text=\"Create new account\"]",
            "//*[@content-desc=\"Create new account\"]",
            "//*[@content-desc=\"Join Facebook\"]",
            "//*[@text=\"Get started\"]",
            "//*[@content-desc=\"No, create new account\"]",
            "//*[@text=\"Enter the confirmation code\"]",
        });
            return xpaths;
        }

        private List<string> BuildLoginList(List<string> xpaths)
        {
            var listLogin = new List<string>();
            listLogin.AddRange(XpathManager.Combine(XpathType.CP282, XpathType.CP956, XpathType.ExistEmail, XpathType.Success));
            listLogin.AddRange(xpaths);
            listLogin.AddRange(XpathManager.Get(XpathType.NavigationButton));
            return listLogin;
        }

        private async Task HandleEmailInputForAgreement()
        {
            if (!_client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), timeoutInSeconds: 1, click: false)) return;
            if (_typeRegister is RegistrationType.Domain or RegistrationType.Gmail_BaitPhoneNumber or RegistrationType.Domain_BaitPhoneNumber or RegistrationType.Gmail)
            {
                await GetEmail();
                if (string.IsNullOrEmpty(_account.Email))
                {
                    _client.LogHelper.ERROR("Không nhận được email.");
                    return;
                }
                _client.ElementWithAttributes(_client.FindElement("", new List<string> { "//*[@text=\"Next\"]" }, 1), click: false);
                _client.SendTextSlow("//*[@class=\"android.widget.EditText\"]", _account.Email.Split('|')[0], timeout: 5);
            }
            _client.ElementWithAttributes(XpathManager.Get(XpathType.NavigationButton), 5);
        }
    }
}
