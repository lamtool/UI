using AutoAndroid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subdy.UI.Services
{
    public class BuildConfig
    {
        public async Task Build()
        {
            ADBHelper.InitADB();
            DeviceServices.GetDeviceModels();

        }
    }
}
