using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subdy.Data.Models
{
    public class Script
    {
        [AppDbContext.SqlKey]
        public long Id { get; set; }

        public string Name { get; set; } = "";
        public string Config { get; set; } = ""; // Danh sách Id hành động dạng string
        public string DateCreate { get; set; } = "";
    }
}
