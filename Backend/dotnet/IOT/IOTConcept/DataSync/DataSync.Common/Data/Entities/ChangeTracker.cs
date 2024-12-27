using Microsoft.EntityFrameworkCore;
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
        public string TableName { get; set; } = string.Empty;
        public long ChangeVersion { get; set; }
    }
}
