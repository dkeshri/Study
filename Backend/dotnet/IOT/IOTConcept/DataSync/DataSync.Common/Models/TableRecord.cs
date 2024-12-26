using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Models
{
#nullable disable
    public class TableRecord
    {
        public Guid Id { get; set; }
        public long ChangeVersion { get; set; }
        public string Operation { get; set; }
        public object Data { get; set; }
    }
}
