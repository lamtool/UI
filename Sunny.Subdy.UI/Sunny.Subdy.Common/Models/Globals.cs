namespace Sunny.Subdy.Common.Models
{
    public class Globals
    {
        public static List<string> GetFieldsToImportExport()
        {
            List<string> listField = new List<string>
            {
                Fields.Empty,
                Fields.Uid,
                Fields.Password,
                Fields._2FA,
                Fields.Token,
                Fields.Cookie,
                Fields.Proxy,
                Fields.Email,
                Fields.PassMail,
                Fields.UserAgent,
                Fields.MailAdress,
                Fields.Username,
            };
          
            return listField;
        }
    }
}
