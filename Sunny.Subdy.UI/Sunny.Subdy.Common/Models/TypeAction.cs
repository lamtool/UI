namespace Sunny.Subdy.Common.Models
{
    public class TypeAction
    {
        public const string FB_SpamXu = "FB_SpamXu";
        public static string GetNameAction(string actionType)
        {
            return actionType switch
            {
                FB_SpamXu => "Spam Xu Facebook",
                // Add other action types here as needed
                _ => "Unknown Action"
            };
        }
    }
}
