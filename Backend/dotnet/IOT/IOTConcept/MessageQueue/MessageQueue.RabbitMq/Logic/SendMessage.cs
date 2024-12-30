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
    internal class SendMessage : ISendMessage
    {
        private readonly IModel? _channel;
        private readonly string _exchangeName; 
        private readonly string _queueName;
        public SendMessage(IRabbitMqConnection rabbitMqConnection)
        {
            _channel = rabbitMqConnection.Channel;
            _exchangeName = rabbitMqConnection.ExchangeName;
            _queueName = rabbitMqConnection.QueueName;
        }

        public void SendToQueue(string message) => SendToQueue(_queueName, message);

        public void SendToQueue(string queueName, string message)
        {
            _channel?.QueueDeclare(queue: queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            if(_channel == null)
            {
                Console.WriteLine($"Error: Can not publish message to queue : {queueName}, channel is set to null!");
                return;
            }
            PublishMessage(_channel, message, queueName);
        }

        public void SendToExchange(string message,string?routingKey)
        {
            _channel?.ExchangeDeclare(_exchangeName,ExchangeType.Direct);
            if (_channel == null)
            {
                Console.WriteLine($"Error: Can not publish message to exchange : {_exchangeName}, channel is set to null!");
                return;
            }
            PublishMessage(_channel, message,routingKey ?? string.Empty, _exchangeName);
        }

        private void PublishMessage(IModel? channel, string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: string.Empty,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);

        }
        private void PublishMessage(IModel? channel, string message, string routingKey, string exchangeName)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);

        }

    }
}
