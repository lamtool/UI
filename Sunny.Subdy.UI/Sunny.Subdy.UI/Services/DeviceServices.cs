using AutoAndroid;
using Sunny.Subdy.UI.View.DeviceControl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sunny.Subdy.UI.Services
{
    public class DeviceServices
    {
        public static List<DeviceModel> DeviceModels = new List<DeviceModel>();
        public static List<ScrcpyDisplay> DisplayList = new List<ScrcpyDisplay>();
        public static void GetScrcpyDisplays()
        {
            DisplayList.Clear();
            //foreach (var device in DeviceModels)
            //{
            //    device.IsScrcpy = false;
            //    var display = new ScrcpyDisplay(device);
            //    display.SetSize(new System.Drawing.Size(220, 400));
            //    DisplayList.Add(display);
            //}
            for (int i = 0; i < 130; i++)
            {
                var device = new DeviceModel();
                device.Serial = $"emulator-{i}";
                var display = new ScrcpyDisplay(device);
                display.SetSize(new System.Drawing.Size(220, 400));
                DisplayList.Add(display);
            }
        }
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
                if (!device.Check || string.IsNullOrEmpty(device.Serial))
                {
                    continue;
                }
                tasks.Add(Task.Run(() => new ADBClient(device).Connect()));
            }
            await Task.WhenAll(tasks);
        }
        public static async Task ConnectScrcpies(List<ScrcpyDisplay> scrcpies)
        {
            List<Task> tasks = new List<Task>();
            foreach (var scrcpy in scrcpies)
            {
                if (scrcpy.Device.IsScrcpy)
                {
                    scrcpy.Device.IsScrcpy = false;
                    await scrcpy.Stop();
                    scrcpy.scrcpy = null;
                }
                scrcpy.Device.IsScrcpy = true;
                scrcpy.Device.PortScrcpy = Scrcpy.GetPort(scrcpy.Device.Serial);
                tasks.Add(Task.Run(async () =>
                {
                    await scrcpy.Start();

                }));
            }
            await Task.WhenAll(tasks);
        }
        public static async Task DisConnectScrcpies(List<ScrcpyDisplay> scrcpies)
        {
            List<Task> tasks = new List<Task>();
            foreach (var scrcpy in scrcpies)
            {
                scrcpy.Device.IsScrcpy = false;
                scrcpy.Device.PortScrcpy = Scrcpy.GetPort(scrcpy.Device.Serial);
                tasks.Add(Task.Run(async () =>
                {
                    await scrcpy.Stop();
                }));
            }
            await Task.WhenAll(tasks);
        }
    }
}
