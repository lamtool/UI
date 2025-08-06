using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subdy.Common.API
{
    public class JobServices
    {
        public const string GoLike = "https://app.golike.net/";
        public const string TuongTacCheo = "https://tuongtaccheo.com/";
        public const string VipIG = "https://vipig.net/";
        public static List<string> TypesFacebook = new List<string>
        {
           // GoLike,
            TuongTacCheo,
        }; 
        public static List<string> TypesInstagram = new List<string>
        {
            GoLike,
            VipIG,
        };
    }
}
