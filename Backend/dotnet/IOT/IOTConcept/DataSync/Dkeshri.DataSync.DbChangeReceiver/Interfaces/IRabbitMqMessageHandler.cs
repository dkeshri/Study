using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dkeshri.DataSync.DbChangeReceiver.Interfaces
{
    internal interface IRabbitMqMessageHandler
    {
        public void HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel);
    }
}
