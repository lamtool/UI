using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook.ScriptActions
{
    public class FbSpamXuHandler : IActionHandler
    {
        public string TypeAction => Sunny.Subdy.Common.Models.TypeAction.FB_SpamXu;

        public async Task<SubdyExtension> ExecuteAsync(string json, Account account, ADBClient device)
        {
            // Ví dụ: json là nội dung comment đơn thuần
            string content = json.Trim('"'); // Nếu json là chuỗi string thuần, cần bỏ dấu " khi deserialize

            Console.WriteLine($"💬 [{account.UserName}] Comment: \"{content}\"");

            // Gửi tap + nhập nội dung + gửi comment qua ADBClient


            return new SubdyExtension(SubdyEnum.None, $"Đã gửi comment: \"{content}\"");
        }
    }
}
