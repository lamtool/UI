namespace Sunny.Subdy.Data.Models
{
    public class ScriptAction
    {
        [AppDbContext.SqlKey]
        public long Id { get; set; }

        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Config { get; set; } = "";
        public long ScriptId { get; set; }
    }
}
