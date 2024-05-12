using MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Logic
{
    public class SendMessageToQueue : ISendMessage
    {
        private readonly IConnection _connection;
        public SendMessageToQueue(IConnection connection)
        {
            _connection = connection;
        }

        public void Send(string message)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            publishMessage(channel, message);
        }
        void publishMessage(IModel channel,string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
