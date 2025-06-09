using AutoAndroid;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sunny.Subdy.UI.Services
{
    public class DeviceServices
    {
        public static List<DeviceModel> DeviceModels = new List<DeviceModel>();
        public static void GetDeviceModels()
        {
            var lines = ADBHelper.GetDevices();
            DeviceModels.Clear();
            foreach (var line in lines)
            {
                var client = new ADBClient(line);
                if (client?.Device == null) continue;
                client.Device.Index = DeviceModels.Count + 1;
                DeviceModels.Add(client.Device);
            }
        }
        public static void ADBKill()
        {
            ADBHelper.KillServer();
            GetDeviceModels();
        }
        public static void SelectAll()
        {
            DeviceModels.ForEach(device => device.Check = true);
        }
        public static void UnSelectAll()
        {
            DeviceModels.ForEach(device => device.Check = false);
        }
        public static async Task Connect()
        {
            List<Task> tasks = new List<Task>();
            foreach (var device in DeviceModels)
            {
                if (!device.Check && string.IsNullOrEmpty(device.Serial))
                {
                    continue;
                }
                tasks.Add(Task.Run(() => new ADBClient(device).Connect()));
            }
            await Task.WhenAll(tasks);
        }
    }
}
