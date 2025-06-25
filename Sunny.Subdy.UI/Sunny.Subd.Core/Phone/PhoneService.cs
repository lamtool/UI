using System.Diagnostics;
using Sunny.Subdy.Common.Models;

namespace Sunny.Subd.Core.Phone
{
    public class PhoneService
    {
        private string _site;
        private string _token;
        public PhoneService(string site, string token)
        {
            _site = site;
            _token = token;
        }
        public async Task<string> GetCode(string id)
        {
            string code = string.Empty;
            switch (_site)
            {
                case RegistrationType.FunOTP:
                    {
                        code = await PhoneFunotp.GetOTP(_token, id);
                        break;
                    }
                case RegistrationType.IronSim:
                    {
                        code = await PhoneIronsim.GetOTP(_token, id);
                        break;
                    }
            }
            return code;
        }
        public async Task<string> GetPhone()
        {
            string phone = string.Empty;
            switch (_site)
            {
                case RegistrationType.FunOTP:
                    {
                        phone = await PhoneFunotp.GetPhone(_token);
                        break;
                    }
                case RegistrationType.IronSim:
                    {
                        phone = await PhoneIronsim.GetPhone(_token);
                        break;
                    }
            }
            return phone;
        }
    }
}
