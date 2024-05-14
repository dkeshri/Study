using MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.Configuration;
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
        private readonly IModel _channel;
        private readonly string _exchangeName; 
        public SendMessage(IRabbitMqConnection rabbitMqConnection)
        {
            _channel = rabbitMqConnection.Channel;
            _exchangeName = rabbitMqConnection.ExchangeName;
        }

        public void SendToQueue(string message)
        {
            _channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            PublishMessage(_channel, message);
        }

        public void SendToExchange(string message,string?routingKey)
        {
            _channel.ExchangeDeclare(_exchangeName,ExchangeType.Direct);
            PublishMessage(_channel, message,_exchangeName,routingKey ?? string.Empty);
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
