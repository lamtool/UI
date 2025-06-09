using Microsoft.VisualBasic.Devices;
using System.Diagnostics;

namespace Sunny.Subdy.UI.Commons
{

    public class SystemUsageMonitor
    {
        public static string Ram = "0%";
        public static float GetCpuUsage()
        {
            using (var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                cpuCounter.NextValue();
                Thread.Sleep(500);
                return cpuCounter.NextValue();
            }
        }

        public static float GetRamUsage()
        {
            using (var ramCounter = new PerformanceCounter("Memory", "Available MBytes"))
            {
                float availableMb = ramCounter.NextValue();
                float totalMb = GetTotalRamInMb();
                return 100f - (availableMb / totalMb * 100f);
            }
        }

        private static float GetTotalRamInMb()
        {
            var computerInfo = new ComputerInfo();
            return computerInfo.TotalPhysicalMemory / (1024f * 1024f);
        }
    }
}
