namespace Sunny.Subdy.Data.Models
{
    public class Script
    {
        [AppDbContext.SqlKey]
        public Guid Id { get; set; }

        public string Name { get; set; } = "";
        public string Config { get; set; } = ""; // Danh sách Id hành động dạng string
        public string DateCreate { get; set; } = "";
        public string Type { get; set; } = "";
        public string JsonData { get; set; } = ""; // Dữ liệu JSON của script
    }
}
