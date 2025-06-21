using System;
using AutoAndroid;
using Sunny.Subd.Core.Proxies;
using Sunny.Subd.Core.Utils;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook
{
    public class FacebookRegsiner
    {
        DeviceModel _device;
        JsonHelper _settingRegsiner;
        JsonHelper _settingGeneral;
        CancellationToken _ct;
        private ADBClient _client;
        public FacebookRegsiner(DeviceModel device, JsonHelper settingRegsiner, JsonHelper settingGeneral, CancellationToken ct)
        {
            _device = device;
            _settingRegsiner = settingRegsiner;
            _settingGeneral = settingGeneral;
            _ct = ct;
            _client = new ADBClient(_device);
        }
        public async Task RegisterAsync()
        {
            while (!_ct.IsCancellationRequested)
            {
                if (!_client.Connect()) continue;

                _client.SetSize();

                ChangeInfo();

                await ChangeProxy();

                if (!IsInternet()) continue;


            }
            _device.Status = "Đã hoàn thành.";
        }
        private void ChangeInfo()
        {
            if (_settingGeneral.GetBooleanValue("checkBox1", true))
            {
                List<string> bards = _settingGeneral.GetValuesFromInputString("textBox1", DeviceServices.Brands).Split('|').ToList();
                string filePath = string.Empty;
                _client.ChangInfo("", false, bards[SubdyHelper.RandomValue(0, bards.Count)], SubdyHelper.Countries[_settingGeneral.GetIntType("cbbScript", 0)]);
            }

        }
        private async Task ChangeProxy()
        {
            string proxy = string.Empty;
            var proxyType = ProxyService.ProxyTypes[_settingGeneral.GetIntType("cbb_ListTypeProxy", 0)];
            switch (proxyType)
            {
                case ProxyService.NoIP:
                case ProxyService.ProxyAssigned:
                    return;
                case ProxyService.Mobile4G:
                    {
                        _client.EnabePlane();
                        _client.Disable4G();
                        break;
                    }
                case ProxyService.KiotProxy:
                    {
                        string key = ProxyService.GetProxy();
                        proxy = await ProxyKiot.NewProxy(key);
                        if (string.IsNullOrEmpty(proxy))
                        {
                            proxy = await ProxyKiot.GetProxy(key);
                        }
                        break;
                    }
                case ProxyService.WWProxy:
                    {
                        string key = ProxyService.GetProxy();
                        proxy = await ProxyWWW.NewProxy(key);
                        if (string.IsNullOrEmpty(proxy))
                        {
                            proxy = await ProxyWWW.GetProxy(key);
                        }
                        break;
                    }
                case ProxyService.ProxyMart:
                    {
                        string key = ProxyService.GetProxy();
                        proxy = await ProxyMart.NewProxy(key);
                        if (string.IsNullOrEmpty(proxy))
                        {
                            proxy = await ProxyMart.GetProxy(key);
                        }
                        break;
                    }
                case ProxyService.CustomProxy:
                    {
                        string line = ProxyService.GetProxy();
                        if (string.IsNullOrEmpty(line))
                        {
                            return;
                        }
                        var parts = line.Split('|');
                        string link = parts.Length > 1 ? parts[1].Trim() : string.Empty;
                        proxy = parts[0].Trim();
                        if (string.IsNullOrEmpty(link))
                        {
                            break;
                        }
                        await RequestService.Get(link);
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
        private bool IsInternet()
        {
            for (int i = 0; i < _settingGeneral.GetIntType("nud_IndexFailProxy", 5); i++)
            {
                if (_client.IsDeviceConnectedToInternet()) return true;
            }
            return false;
        }

        private bool LoginGmail()
        {
            return false;
        }
       
    }
}
