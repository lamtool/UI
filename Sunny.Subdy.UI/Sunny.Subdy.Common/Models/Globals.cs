using Sunny.Subdy.Common.API.Model;
using System.Windows.Forms;

namespace Sunny.Subdy.Common.Models
{
    public class Globals
    {
        public static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
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
                Fields.MailAdress,
            };

            return listField;
        }
        public static string DeviceId = Guid.NewGuid().ToString();
        public static string NameApp = "LamToolAutoPhone";
        public static DataGridView DataGridView { get; set; } = new DataGridView();
        public static User User { get; set; }

    }
}
