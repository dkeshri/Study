using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Extensions
{
    public class DbConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int TransactionTimeOutInSec { get; set; } = 30;
    }
}
