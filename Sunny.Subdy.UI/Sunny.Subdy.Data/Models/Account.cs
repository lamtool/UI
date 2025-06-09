namespace Sunny.Subdy.Data.Models
{
    public class Account
    {
        [AppDbContext.SqlKey]
        public long Id { get; set; }

        public string? Uid { get; set; } = "";
        public string? Password { get; set; } = "";
        public string? Phone { get; set; } = "";
        public string? TowFA { get; set; } = "";
        public string? Cookie { get; set; } = "";
        public string? Token { get; set; } = "";
        public string? Proxy { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? PassMail { get; set; } = "";
        public string? UserAgent { get; set; } = "";
        public string? FullName { get; set; } = "";
        public string? RecentInteraction { get; set; } = "";
        public string? State { get; set; } = "";
        public string? Status { get; set; } = "";
        public string? Result { get; set; } = "";
        public string? EmailAdress { get; set; } = "";
        public string? UserName { get; set; } = "";
        public string? NameFolder { get; set; }
        public bool IsView { get; set; } = true;
    }
}
