using Dkeshri.MessageQueue.Constants;

namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public class QueueConfig
    {
        public string QueueName { get; set; } = MessageQueueConstant.UNKNOWN;
        public bool IsDurable { get; set; } = false;
        public bool IsExclusive { get; set; } = false;
        public bool IsAutoDelete { get; set; } = false;
        public string ExchangeName { get; set; } = string.Empty;
        public string[] RoutingKeys { get; set; } = {string.Empty};
        public IDictionary<string, object>? Arguments { get; set; }
    }
}
