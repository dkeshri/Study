using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Interfaces
{
    internal interface IRabbitMqMessageHandler
    {
        public void HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel);
    }
}
