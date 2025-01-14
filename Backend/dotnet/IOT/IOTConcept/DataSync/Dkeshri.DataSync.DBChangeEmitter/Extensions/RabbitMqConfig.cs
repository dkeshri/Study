namespace Dkeshri.DataSync.DBChangeEmitter.Extensions
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; } = null!;

        public int Port { get; set; }

        public string ExchangeName { get; set; } = null!;

        public string QueueName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Topic { get; set; } = null!;

    }
}
