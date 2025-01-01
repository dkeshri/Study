using MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Handlers
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
