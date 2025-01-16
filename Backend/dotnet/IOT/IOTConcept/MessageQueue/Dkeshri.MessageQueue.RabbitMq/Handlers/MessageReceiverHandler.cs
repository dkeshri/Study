using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dkeshri.MessageQueue.RabbitMq.Handlers
{
    internal class MessageReceiverHandler : IMessageReceiver
    {
        public Action<object?, BasicDeliverEventArgs,IModel>? MessageHandler { get; set; }

        void IMessageReceiver.HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            MessageHandler?.Invoke(model, ea, channel);
        }
      
    }
}
