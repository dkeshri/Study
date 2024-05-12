using MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Logic
{
    public class SendMessageToDirectExchange : SendMessage, ISendMessageToDirectExchange
    {
        public SendMessageToDirectExchange(IConnection connection) : base(connection)
        {
            
        }

        public override void Send(string message)
        {
            using var channel = Connection.CreateModel();
            channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            PublishMessage(channel, message, null);
        }
    }
}
