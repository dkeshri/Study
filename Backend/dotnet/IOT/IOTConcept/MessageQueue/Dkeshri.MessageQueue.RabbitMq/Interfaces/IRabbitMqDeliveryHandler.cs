using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Dkeshri.MessageQueue.RabbitMq.Interfaces
{
    internal interface IRabbitMqDeliveryHandler
    {
        internal void HandleDelivery(object? model, BasicDeliverEventArgs ea, IModel channel);
    }
}
