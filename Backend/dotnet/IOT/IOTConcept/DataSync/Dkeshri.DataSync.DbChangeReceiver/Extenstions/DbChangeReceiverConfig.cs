using Dkeshri.DataSync.Common.Extensions;
using Dkeshri.MessageQueue.Extensions;

namespace Dkeshri.DataSync.DbChangeReceiver.Extenstions
{
    public class DbChangeReceiverConfig
    {
        public DatabaseType DatabaseType { get; set; }
        public DbConfig DbConfig { get; set; } = null!;
        public MessageBroker MessageBroker { get; set; } = null!;
    }
}
