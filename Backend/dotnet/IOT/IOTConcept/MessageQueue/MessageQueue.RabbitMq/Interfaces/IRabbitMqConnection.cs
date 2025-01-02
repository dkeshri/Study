using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Interfaces
{
    public interface IRabbitMqConnection
    {
        IModel? Channel { get; }
        string ExchangeName { get; }
        string QueueName {  get; }
        internal void EnableConfirmIfNotSelected();

    }
}
