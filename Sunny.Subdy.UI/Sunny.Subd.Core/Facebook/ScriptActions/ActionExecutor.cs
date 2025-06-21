using AutoAndroid;
using Sunny.Subd.Core.Models;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Facebook.ScriptActions
{
    public class ActionExecutor
    {
        private readonly Dictionary<string, IActionHandler> _handlers;

        public ActionExecutor(IEnumerable<IActionHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.TypeAction, StringComparer.OrdinalIgnoreCase);
        }

        public async Task<SubdyExtension> ExecuteAsync(string typeAction, string json, Account account, ADBClient device)
        {
            if (_handlers.TryGetValue(typeAction, out var handler))
            {
             return   await handler.ExecuteAsync(json, account, device);
            }
            else
            {
                Console.WriteLine($"⚠️ Không tìm thấy handler cho {typeAction}");
            }
            return new SubdyExtension(SubdyEnum.None, $"Không tìm thấy handler cho {typeAction}");
        }
    }
}
