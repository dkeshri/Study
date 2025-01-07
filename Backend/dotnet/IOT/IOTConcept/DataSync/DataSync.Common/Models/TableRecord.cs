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
        public long ChangeVersion { get; set; }
        public string Operation { get; set; }
        public string Data { get; set; }
        
    }
}
