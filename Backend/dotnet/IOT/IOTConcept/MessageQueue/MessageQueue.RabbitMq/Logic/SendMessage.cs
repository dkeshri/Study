using MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Logic
{
    public class SendMessage : ISendMessage
    {
        private readonly IConnection _connection;
        private readonly string directExchangeName = "weather_direct";
        public SendMessage(IConnection connection)
        {
            _connection = connection;
        }

        public void SendToQueue(string message)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            PublishMessage(channel, message);
        }

        public void SendToExchange(string message,string?routingKey)
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(directExchangeName,ExchangeType.Direct);
            PublishMessage(channel, message,directExchangeName,routingKey ?? string.Empty);
        }

        private void PublishMessage(IModel channel, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: string.Empty,
                                routingKey: "hello",
                                basicProperties: null,
                                body: body);

        }
        private void PublishMessage(IModel channel, string message, string exchangeName, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);

        }
    }
}
