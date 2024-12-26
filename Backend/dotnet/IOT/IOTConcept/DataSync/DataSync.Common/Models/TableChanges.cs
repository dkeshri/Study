using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Models
{
    public class TableChanges
    {
        public string TableName { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public IReadOnlyCollection<TableRecord> Records { get; set; } = null!;

    }
}
