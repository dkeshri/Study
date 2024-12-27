using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Data.Entities
{
    public class ChangeTracker
    {
        [Key]
        public int TableName { get; set; }
        public long ChangeVersion { get; set; }
    }
}
