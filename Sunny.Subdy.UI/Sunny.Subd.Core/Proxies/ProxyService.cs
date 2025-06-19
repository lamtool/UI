using Sunny.Subdy.Common.Json;

namespace Sunny.Subd.Core.Proxies
{
    public class ProxyService
    {
        public static List<string> Proxies = new List<string>
        {
            "Không đổi IP",
            "4G Mobile",
            "Proxy đã gán cho tài khoản",
        };

        public static async Task<string> GetProxyAsync()
        {
            
            // Simulate an asynchronous operation to get a proxy
            await Task.Delay(1000); // Simulating network delay
            return "http://proxy.example.com:8080"; // Return a dummy proxy URL
        }
    }
}
