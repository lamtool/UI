namespace Sunny.Subdy.Common.Models
{
    public class RegistrationType
    {
        public const string NVR = "NVR";
        public const string Domain = "Domain";
        public const string Gmail = "Gmail";
        public const string PhoneNumber = "PhoneNumber";
        public const string Domain_BaitPhoneNumber = "Domain mồi số điện thoại";
        public const string Gmail_BaitPhoneNumber = "Gmail mồi số điện thoại";
        public const string PhoneNumber_BaitPhoneNumber = "PhoneNumber mồi số điện thoại";
        public static List<string> RegFacebook_AllTypes = new List<string>
        {
            Domain,
            Gmail,
            PhoneNumber,
            Domain_BaitPhoneNumber,
            Gmail_BaitPhoneNumber,
            PhoneNumber_BaitPhoneNumber
        };


        public const string Domain_TempMail = "https://temp-mail.io/";
        public const string Domain_Getnada = "https://inboxes.com";
        public const string Domain_MailTM = "https://mail.tm/";
        public const string Domain_ShopVia = "http://shopvia1s.com";
        public static List<string> EmailTypes = new List<string>
        {
            Domain_TempMail,
            Domain_Getnada,
            Domain_MailTM,
            Domain_ShopVia,
        };

        public const string IronSim = "https://ironsim.com/";
        public const string FunOTP = "https://funotp.com";
        public static List<string> PhoneNumberTypes = new List<string>
        {
            IronSim,
            FunOTP,
        };


        public const string RegFacebook = "RegFacebook";
        public const string RegGmail = "RegGmail";

        public static List<string> RegGmail_AllTypes = new List<string>
        {
            NVR,
            PhoneNumber,
        };
    }
}
