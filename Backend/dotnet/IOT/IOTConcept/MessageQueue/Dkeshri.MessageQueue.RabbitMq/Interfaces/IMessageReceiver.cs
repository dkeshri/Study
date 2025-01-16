using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Dkeshri.MessageQueue.RabbitMq.Interfaces
{
    public interface IMessageReceiver
    {
        public Action<object?, BasicDeliverEventArgs, IModel>? MessageHandler { get; set; }
        internal void HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel);
    }
}
