using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace MessageQueue.RabbitMq.Logic
{
    internal class SendMessage : ISendMessage
    {
        private readonly string _exchangeName; 
        private readonly string _queueName;
        private readonly IRabbitMqConnection _connection;
        public SendMessage(IRabbitMqConnection rabbitMqConnection)
        {
            _connection = rabbitMqConnection;
            _exchangeName = rabbitMqConnection.ExchangeName;
            _queueName = rabbitMqConnection.QueueName;
        }

        public bool SendToQueue(string message) => SendToQueue(_queueName, message);

        public bool SendToQueue(string queueName, string message)
        {
            IModel? channel = _connection.Channel;
            if(channel == null || channel.IsClosed)
            {
                Console.WriteLine($"Error: Can not publish message to queue : {queueName}, channel is null or closed!");
                return false;
            }

            _connection.EnableConfirmIfNotSelected();

            channel.QueueDeclare(queue: queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            return PublishMessage(channel, message, queueName);
        }

        public bool SendToExchange(string message,string?routingKey)
        {
            IModel? channel = _connection.Channel;
            if (channel == null)
            {
                Console.WriteLine($"Error: Can not publish message to exchange : {_exchangeName}, channel is set to null!");
                return false;
            }
            _connection.EnableConfirmIfNotSelected();
            channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            return PublishMessage(channel, message,routingKey ?? string.Empty, _exchangeName);
        }

        private bool PublishMessage(IModel channel, string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: string.Empty,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);
            return channel.WaitForConfirms(TimeSpan.FromSeconds(3));

        }
        private bool PublishMessage(IModel channel, string message, string routingKey, string exchangeName)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);
            return channel.WaitForConfirms(TimeSpan.FromSeconds(3));
        }

    }
}
