using Sunny.Subdy.Data.Models;

namespace Sunny.Subd.Core.Models
{
    public class ConfigModel
    {
        public Script Script { get; set; } = new Script();
        public List<ScriptAction> Actions { get; set; } = new List<ScriptAction>();
        public string JsonSetting { get; set; } = "";
    }
}
