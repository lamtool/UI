using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Services
{
    public class MainService
    {
        private DeviceModel _device { get; set; }
        private Account _account { get; set; }
        private ConfigModel _config { get; set; }
        private CancellationToken _ct;
        public MainService(DeviceModel device, Account account, ConfigModel config, CancellationToken ct)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device), "Device cannot be null");
            _account = account ?? throw new ArgumentNullException(nameof(account), "Account cannot be null");
            _config = config ?? throw new ArgumentNullException(nameof(config), "Config cannot be null");
            _ct = ct;
        }

        public async Task RunAsync()
        {


        }
    }
}
