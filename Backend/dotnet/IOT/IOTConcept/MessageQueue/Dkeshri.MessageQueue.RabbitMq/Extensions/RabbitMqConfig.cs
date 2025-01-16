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
        public string ClientProvidedName { get; set; } = null!;
        public string Topic { get; set; } = null!;
        public bool RegisterSenderServices { get; set; } = false;
        public bool RegisterReceiverServices { get; set;} = false;

    }
}
