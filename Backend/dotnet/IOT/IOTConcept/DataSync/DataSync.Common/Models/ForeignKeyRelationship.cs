using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Models
{
    public class ForeignKeyRelationship
    {
        public string PkTable { get; set; } = null!;
        public string FkTable { get; set; } = null!;
    }
}
