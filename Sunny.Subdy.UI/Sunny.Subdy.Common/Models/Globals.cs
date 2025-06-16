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
        public static List<string> ListJobFacebook = new List<string>()
        {
            "fb_feel",
            "fb_follow",
            "fb_like_page",
            "fb_review",
            "fb_join_group",
            "fb_like_comment",
            "fb_share",
            "fb_share_content",
            "fb_comment",
        };
        public static string DeviceId = Guid.NewGuid().ToString();
        public static string NameApp = "LamToolAutoPhone";
    }
}
