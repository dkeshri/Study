using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Interfaces
{
    public interface IMessageReceiver
    {
        public Action<object?, BasicDeliverEventArgs, IModel>? MessageHandler { get; set; }
        internal void HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel);
    }
}
