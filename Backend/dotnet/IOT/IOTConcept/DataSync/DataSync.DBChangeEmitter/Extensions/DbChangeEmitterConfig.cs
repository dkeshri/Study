using DataSync.Common.Extensions;

namespace DataSync.DBChangeEmitter.Extensions
{
    public class DbChangeEmitterConfig
    {
        internal RabbitMqConfig RabbitMqConfig { get; set; } = null!;
        public DatabaseType DatabaseType { get; set; }
        public DbConfig DbConfig { get; set; } = null!;
    }
}
