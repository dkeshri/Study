using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Interfaces
{
    public abstract class SendMessage : ISendMessage
    {
        private readonly IConnection _connection;
        protected IConnection Connection { get { return _connection; } }
        protected SendMessage(IConnection connection)
        {
            _connection = connection;
        }
        public virtual void Send(string message)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            PublishMessage(channel, message,null);
        }
        protected virtual void PublishMessage(IModel channel, string message,string? routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            if(routingKey != null)
            {

            }
            else
            {
                channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
            }
            
        }
    }
}
