using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace MessageQueue.RabbitMq.Logic
{
    internal class MessageSender : IMessageSender
    {
        private readonly IRabbitMqConnection _connection;
        private readonly QueueConfig queueConfig;
        private readonly ExchangeConfig exchangeConfig;
        public MessageSender(IRabbitMqConnection rabbitMqConnection)
        {
            _connection = rabbitMqConnection;
            queueConfig = rabbitMqConnection.Queue;
            exchangeConfig = rabbitMqConnection.Exchange;
        }

        public bool SendToQueue(string message) => SendToQueue(queueConfig.QueueName, message);

        public bool SendToQueue(string queueName, string message)
        {
            IModel? channel = _connection.Channel;
            if(channel == null || channel.IsClosed)
            {
                Console.WriteLine($"Error: Can not publish message to queue : {queueName}, channel is null or closed!");
                return false;
            }
            return PublishMessage(channel, message, queueName);
        }

        public bool SendToExchange(string message,string? routingKey)
        {
            IModel? channel = _connection.Channel;
            if (channel == null)
            {
                Console.WriteLine($"Error: Can not publish message to exchange : {exchangeConfig.ExchangeName}, channel is set to null!");
                return false;
            }
            return PublishMessage(channel, message, routingKey ?? string.Empty, exchangeConfig.ExchangeName);
        }

        private bool PublishMessage(IModel channel, string message, string routingKey) 
            => PublishMessage(channel,message,routingKey, null);
        
        private bool PublishMessage(IModel channel, string message, string routingKey, string? exchangeName)
        {
            _connection.EnableConfirmIfNotSelected();
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName ?? string.Empty,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);
            return channel.WaitForConfirms(TimeSpan.FromSeconds(3));
        }

    }
}
