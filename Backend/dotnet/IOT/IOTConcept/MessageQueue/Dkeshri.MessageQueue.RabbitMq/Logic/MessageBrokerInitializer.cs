using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Threading.Channels;

namespace Dkeshri.MessageQueue.RabbitMq.Logic
{
    internal class MessageBrokerInitializer : IStartup
    {
        private readonly IRabbitMqConnection _connection;
        private readonly QueueConfig queueConfig;
        private readonly ExchangeConfig exchangeConfig;
        private readonly bool _registerSenderServices;
        private readonly bool _registerReceiverServices;
        public MessageBrokerInitializer(IRabbitMqConnection rabbitMqConnection)
        {
            _connection = rabbitMqConnection;
            queueConfig = rabbitMqConnection.Queue;
            exchangeConfig = rabbitMqConnection.Exchange;
            _registerSenderServices = rabbitMqConnection.RegisterSenderServices;
            _registerReceiverServices = rabbitMqConnection.RegisterReceiverServices;

        }
        public void OnStart()
        {
            IModel? channel = _connection.Channel;
            if (channel == null)
            {
                Console.WriteLine($"Error:Channel is set to null!, Can't init RabbitMq Message Broker");
                return;
            }
            if (_registerSenderServices) 
            {
                OnSenderStart(channel);
            }

            if (_registerReceiverServices) 
            {
                OnReceiverStart(channel);
            }

        }
        private void OnReceiverStart(IModel channel)
        {

            if (string.IsNullOrEmpty(queueConfig.QueueName))
            {
                Console.WriteLine("Error: Can't Create Exchange Queue Name is null or empty");
                return;
            }

            if (!channel.IsOpen)
            {
                Console.WriteLine($"Error: Can't Create Queue: {queueConfig.QueueName}, channel is not open!");
                return;
            }
            Console.WriteLine("Creating Queue");
            channel.QueueDeclare(queue: queueConfig.QueueName,
                    durable: queueConfig.IsDurable,
                    exclusive: queueConfig.IsExclusive,
                    autoDelete: queueConfig.IsAutoDelete,
                    arguments: queueConfig.Arguments);
            Console.WriteLine($"Queue: {queueConfig.QueueName} created!");

        }

        private void OnSenderStart(IModel channel)
        {

            if (string.IsNullOrEmpty(exchangeConfig.ExchangeName))
            {
                Console.WriteLine("Error: Can't Create Exchange: ExchangeName is null or empty");
                return;
            }
            Console.WriteLine("Creating Exchange");
            if (!channel.IsOpen)
            {
                Console.WriteLine($"Error: Can't Create Exchange: {exchangeConfig.ExchangeName}, channel is not open!");
                return;
            }
            Console.WriteLine("Declaring Alternate Exchange");
            createAlternateExchange(channel);
            channel.ExchangeDeclare(
                exchange: exchangeConfig.ExchangeName,
                type: exchangeConfig.ExchangeType,
                durable: exchangeConfig.IsDurable,
                autoDelete: exchangeConfig.AutoDelete,
                arguments: new Dictionary<string, object>
                {
                    { "alternate-exchange", "alternate.exchange" }
                }
            );
            Console.WriteLine($"Exchange: {exchangeConfig.ExchangeName} created!");
        }

        private void createAlternateExchange(IModel channel)
        {
            channel.ExchangeDeclare("alternate.exchange", ExchangeType.Fanout, durable: true);
            channel.QueueDeclare("unroutable.queue", durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind("unroutable.queue", "alternate.exchange", "");
        }

    }
}
