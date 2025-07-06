using System.Runtime.InteropServices;
using System.Text;

namespace Sunny.Subdy.Common.Helper
{
    public static class HttpRequestHelper
    {
        [DllImport("HttpRequestLib.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr HttpRequestW(
            string method, string url, string headers,
            string body, string proxy, string proxyUser, string proxyPass);

        [DllImport("HttpRequestLib.dll")]
        private static extern void FreeResponse(IntPtr ptr);

        public static string Request(
            string method, string url,
            string headers = "", string body = "",
            string proxy = "", string user = "", string pass = "")
        {
            IntPtr ptr = HttpRequestW(method, url, headers, body, proxy, user, pass);
            if (ptr == IntPtr.Zero) return null;
            string response = Marshal.PtrToStringUni(ptr);
            FreeResponse(ptr);
            return response;
        }

        private static string BuildHeader(Dictionary<string, string> headerDict)
        {
            if (headerDict == null || headerDict.Count == 0) return "";
            var sb = new StringBuilder();
            foreach (var kv in headerDict)
            {
                sb.Append($"{kv.Key}: {kv.Value}\r\n");
            }
            return sb.ToString();
        }

        private static string BuildFormData(Dictionary<string, string> bodyDict)
        {
            if (bodyDict == null || bodyDict.Count == 0) return "";
            var list = new List<string>();
            foreach (var kv in bodyDict)
            {
                list.Add($"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}");
            }
            return string.Join("&", list);
        }

        public static string GET(string url, string proxy = "", Dictionary<string, string> headers = null)
        {
            string headerText = BuildHeader(headers);
            return Request("GET", url, headerText, "", proxy, "", "");
        }

        public static string POST(string url, string proxy = "", Dictionary<string, string> headers = null, Dictionary<string, string> body = null)
        {
            string headerText = BuildHeader(headers);
            string bodyText = BuildFormData(body);

            if (headers == null || !headers.ContainsKey("Content-Type"))
            {
                headerText += "Content-Type: application/x-www-form-urlencoded\r\n";
            }

            return Request("POST", url, headerText, bodyText, proxy, "", "");
        }

        // Optional: add proxy auth overloads if needed
        public static string GET(string url, string proxy, string user, string pass, Dictionary<string, string> headers = null)
        {
            string headerText = BuildHeader(headers);
            return Request("GET", url, headerText, "", proxy, user, pass);
        }

        public static string POST(string url, string proxy, string user, string pass, Dictionary<string, string> headers = null, Dictionary<string, string> body = null)
        {
            string headerText = BuildHeader(headers);
            string bodyText = BuildFormData(body);

            if (headers == null || !headers.ContainsKey("Content-Type"))
            {
                headerText += "Content-Type: application/x-www-form-urlencoded\r\n";
            }

            return Request("POST", url, headerText, bodyText, proxy, user, pass);
        }
    }
}
