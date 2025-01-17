using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Dkeshri.MessageQueue.RabbitMq.Interfaces
{
    public interface IMessageHandler
    {
        internal void HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel);
    }
}
