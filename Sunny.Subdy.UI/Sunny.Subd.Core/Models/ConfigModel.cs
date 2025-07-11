using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Models
{
    public class ConfigModel
    {
        public Script Script { get; set; } = new Script();
        public JsonHelper SettingGeneral { get; set; }
    }
}
