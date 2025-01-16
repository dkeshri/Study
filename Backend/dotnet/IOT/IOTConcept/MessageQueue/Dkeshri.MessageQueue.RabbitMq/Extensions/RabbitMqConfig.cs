namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; } = null!;
        public int Port { get; set; }
        public string ExchangeName { get; set; } = null!;
        public string QueueName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        internal string ClientProvidedName { get; set; } = null!;
        public string Topic { get; set; } = null!;
        internal bool RegisterSenderServices { get; set; } = false;
        internal bool RegisterReceiverServices { get; set;} = false;

    }
}
