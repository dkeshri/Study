using Dkeshri.DataSync.Common.Extensions;
using Dkeshri.MessageQueue.Extensions;

namespace Dkeshri.DataSync.DBChangeEmitter.Extensions
{
    public class DbChangeEmitterConfig
    {
        internal RabbitMqConfig RabbitMqConfig { get; set; } = null!;
        public DatabaseType DatabaseType { get; set; }
        public DbConfig DbConfig { get; set; } = null!;
        internal bool IsRabbitMqConfigured {  get; set; } = false;
        public MessageBroker MessageBroker { get; set; } = null!;
    }
}
