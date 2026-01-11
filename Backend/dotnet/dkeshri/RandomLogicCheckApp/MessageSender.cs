using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomLogicCheckApp
{
    internal class MessageSender
    {
        IConnectionFactory connectionFactory;
        IConnection connection;
        IModel channel;
        public MessageSender()
        {
            connectionFactory =  new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                ClientProvidedName = "ShyamSir"
            };

            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();

        }

        public void SendMessage(string message)
        {
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
