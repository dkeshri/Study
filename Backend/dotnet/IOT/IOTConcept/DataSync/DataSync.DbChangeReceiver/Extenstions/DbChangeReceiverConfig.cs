using DataSync.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Extenstions
{
    public class DbChangeReceiverConfig
    {
        internal RabbitMqConfig RabbitMqConfig { get; set; } = null!;
        public DatabaseType DatabaseType { get; set; }
        public DbConfig DbConfig { get; set; } = null!;
    }
}
