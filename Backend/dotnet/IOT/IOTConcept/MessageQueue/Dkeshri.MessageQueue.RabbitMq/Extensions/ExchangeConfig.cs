using Dkeshri.MessageQueue.Constants;

namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public class ExchangeConfig
    {
        public string ExchangeName { get; set; } = MessageQueueConstant.UNKNOWN;
        public bool IsDurable { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public string[] RoutingKeys { get; set; } = { string.Empty };
        public IDictionary<string, object>? Arguments { get; set; }
        public string ExchangeType { get; set; } = RabbitMQ.Client.ExchangeType.Direct;
    }
}
