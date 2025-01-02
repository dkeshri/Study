using MessageQueue.RabbitMq.Interfaces;
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

        public void SendToQueue(string message) => SendToQueue(_queueName, message);

        public void SendToQueue(string queueName, string message)
        {
            IModel? channel = _connection.Channel;
            if(channel == null || channel.IsClosed)
            {
                Console.WriteLine($"Error: Can not publish message to queue : {queueName}, channel is null or closed!");
                return;
            }
            channel.QueueDeclare(queue: queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            PublishMessage(channel, message, queueName);
        }

        public void SendToExchange(string message,string?routingKey)
        {
            IModel? channel = _connection.Channel;
            if (channel == null)
            {
                Console.WriteLine($"Error: Can not publish message to exchange : {_exchangeName}, channel is set to null!");
                return;
            }
            channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            PublishMessage(channel, message,routingKey ?? string.Empty, _exchangeName);
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
