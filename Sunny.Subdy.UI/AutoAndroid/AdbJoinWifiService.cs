using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoAndroid
{
    public class AdbJoinWifiService
    {
        public static string PathApk = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "adbjoinwifi.apk");
        public static string Package = "com.steinwurf.adbjoinwifi";
        private ADBClient _service;
        public AdbJoinWifiService(ADBClient service)
        {
            _service = service;
        }
        public bool Install()
        {
            for (int i = 0; i < 20; i++)
            {
                if (_service.AppList().Contains(Package))
                {
                    break;
                }
                _service.InstallApp(PathApk);
                _service.Delay(2);
            }
            return true;
        }
        public bool ConnectToWifiNetwork(string wifiSSID, string wifiPassword)
        {
            Install();
            _service.Shell("shell su -c 'svc wifi enable'");
            _service.StopApp("com.steinwurf.adbjoinwifi");
            var s = _service.Shell("shell am start -n com.steinwurf.adbjoinwifi/.MainActivity -e ssid '" + wifiSSID + "' -e password_type WPA -e password '" + wifiPassword + "'");
            return true;
        }
        public void DisableWifi()
        {
            _service.Shell("shell su -c 'svc wifi disable'");
        }
        public void EnableWifi()
        {
            _service.Shell("shell su -c 'svc wifi enable'");
        }
        
    }
}
