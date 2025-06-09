using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subdy.Data.Models
{
    public class HistoryJob
    {
        [AppDbContext.SqlKey]
        public long Id { get; set; }
        public string? Uid { get; set; } = "";
        public string? DateTime { get; set; } = "";
        public string? Type { get; set; } = "";
        public string? IdJob { get; set; } = "";
        public string? Coin { get; set; } = "";
        public string? OjectJob { get; set; } = "";
        public string? State { get; set; } = "";
    }
}
