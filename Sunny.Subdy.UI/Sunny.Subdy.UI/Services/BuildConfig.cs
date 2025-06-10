using AutoAndroid;
using FFmpeg.AutoGen;
using Sunny.Subdy.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            DeviceServices.GetScrcpyDisplays();
            SubdyHttpServer server = new SubdyHttpServer();
            _= server.StartServer();

        }
    }
}
