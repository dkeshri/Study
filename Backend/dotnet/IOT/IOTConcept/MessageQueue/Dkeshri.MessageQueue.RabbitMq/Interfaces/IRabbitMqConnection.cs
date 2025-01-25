using Dkeshri.MessageQueue.RabbitMq.Extensions;
using RabbitMQ.Client;

namespace Dkeshri.MessageQueue.RabbitMq.Interfaces
{
    public interface IRabbitMqConnection
    {
        IModel? Channel { get; }
        internal void EnableConfirmIfNotSelected();
        internal QueueConfig? Queue { get; }
        internal ExchangeConfig? Exchange { get; }
        internal bool RegisterSenderServices { get; }
        internal bool RegisterReceiverServices { get; } 

    }
}
