using Sunny.Subdy.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subdy.Common.API
{
    public class API_LamTool
    {
        public static string Authentication(string username, string password)
        {
            string url = "https://lamtool.net/api/auth/login";
            var body = new Dictionary<string, string>
            {
                ["username"] = username,
                ["password"] = password
            };
            string resurl = HttpRequestHelper.POST(url, body: body);
            return "";
        }

    }
}
