using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subdy.Data.Models
{
    public class Folder
    {
        [AppDbContext.SqlKey]
        public long Id { get; set; }

        public string? Name { get; set; } = "";
        public string? DateCreate { get; set; } = "";
        public long? Count { get; set; } = 0;
        public bool IsView { get; set; } = true;
    }
}
