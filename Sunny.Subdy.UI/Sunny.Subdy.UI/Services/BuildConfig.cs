using AutoAndroid;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.Server;

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
            await server.StartServer();

        }
    }
}
