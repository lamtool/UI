using Sunny.Subdy.Common.Models;

namespace Sunny.Subd.Core.Email
{
    public class EmailService
    {
        private string _site;
        public EmailService(string site)
        {
            _site = site;
        }
        public async Task<string> GetCode(string email)
        {
            string code =string.Empty;
            switch (_site)
            {
                case RegistrationType.Domain_ShopVia:
                    {
                        string[] lines = email.Split('|');
                        string token = lines[2];
                        string clien = lines[3];
                        string emailValue = lines[0];
                        return await Shopvia1s.GetCode(emailValue, token, clien);
                    }
                case RegistrationType.Domain_Getnada:
                    {
                        return await GetnadaService.GetCode(email);
                    }
                case RegistrationType.Domain_TempMail:
                    {
                        return await TempMailService.GetCode(email);
                    }
                default:
                    {
                        throw new NotSupportedException($"Email service '{_site}' is not supported.");
                    }
            }
        }
        public async Task<string> GetEmail(string email)
        {
            string code = string.Empty;
            switch (_site)
            {
                case RegistrationType.Domain_ShopVia:
                    {
                        return await Shopvia1s.GetEmail(email);
                    }
                case RegistrationType.Domain_Getnada:
                    {
                        return await GetnadaService.GetEmail();
                    }
                case RegistrationType.Domain_TempMail:
                    {
                        return await TempMailService.GetEmail();
                    }
                default:
                    {
                        throw new NotSupportedException($"Email service '{_site}' is not supported.");
                    }
            }
        }
    }
}
