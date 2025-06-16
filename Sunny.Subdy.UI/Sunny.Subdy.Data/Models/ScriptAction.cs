namespace Sunny.Subdy.Data.Models
{
    public class ScriptAction
    {
        [AppDbContext.SqlKey]
        public Guid Id { get; set; }

        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Json { get; set; } = "";
        public Guid ScriptId { get; set; }
    }
}
