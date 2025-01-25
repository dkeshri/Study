using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Channels;

namespace Dkeshri.MessageQueue.RabbitMq.Logic
{
    internal class MessageBrokerInitializer : IStartup
    {
        private readonly IRabbitMqConnection _connection;
        private readonly QueueConfig? queueConfig;
        private readonly ExchangeConfig? exchangeConfig;
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
            Console.WriteLine("Message Broker OnStart");
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
        private void OnSenderStart(IModel channel)
        {
            Console.WriteLine("Message Broker: Sender OnStartUp");
            if (!channel.IsOpen)
            {
                Console.WriteLine($"Error: Channel is not open!");
                return;
            }

            if (exchangeConfig != null && !string.IsNullOrEmpty(exchangeConfig.ExchangeName))
            {
                Console.WriteLine($"Creating Exchange : {exchangeConfig.ExchangeName}");
                createAlternateExchange(channel);
                channel.ExchangeDeclare(
                    exchange: exchangeConfig.ExchangeName,
                    type: exchangeConfig.ExchangeType,
                    durable: exchangeConfig.IsDurable,
                    autoDelete: exchangeConfig.AutoDelete,
                    arguments: exchangeConfig.Arguments
                );
                Console.WriteLine($"Exchange: {exchangeConfig.ExchangeName} created!");
            }

            if (queueConfig != null && !string.IsNullOrEmpty(queueConfig.QueueName))
            {
                createQueue(channel);
            }

        }

        private void createAlternateExchange(IModel channel)
        {
            exchangeConfig?.Arguments.Add("alternate-exchange", "alternate.exchange");
            Console.WriteLine("Declaring Alternate Exchange");
            channel.ExchangeDeclare("alternate.exchange", ExchangeType.Fanout, durable: true);
            channel.QueueDeclare("unroutable.queue", durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind("unroutable.queue", "alternate.exchange", "");
            Console.WriteLine("Alternate Exchange Declared! and unroutable.queue is binded.");
        }
        private void OnReceiverStart(IModel channel)
        {
            Console.WriteLine("Message Broker: Receiver OnStartUp");
            if (!channel.IsOpen)
            {
                Console.WriteLine($"Error: channel is not open!");
                return;
            }
            if (queueConfig != null && !string.IsNullOrEmpty(queueConfig.QueueName))
            {
                createQueue(channel);
            }
            
        }
        private void createQueue(IModel channel)
        {
            Console.WriteLine($"Creating Queue: {queueConfig!.QueueName}");
            createDeadLetterExchange(channel);
            channel.QueueDeclare(queue: queueConfig.QueueName,
                    durable: queueConfig.IsDurable,
                    exclusive: queueConfig.IsExclusive,
                    autoDelete: queueConfig.IsAutoDelete,
                    arguments: queueConfig.Arguments);
            Console.WriteLine($"Queue: {queueConfig.QueueName} created!");
            if (!string.IsNullOrEmpty(queueConfig.ExchangeName))
            {
                BindQueueWithExchange();
            }
            else
            {
                Console.WriteLine($"Queue: {queueConfig.QueueName} did not bind to any Exchange, Exchnage name not provided!");
                Console.WriteLine("Queue is standalone. Receive Messages if directly Publish to Queue!");
            }
        }
        private void createDeadLetterExchange(IModel channel)
        {
            queueConfig.Arguments?.Add("x-dead-letter-exchange", "dead.letter.exchange");
            Console.WriteLine("Declaring Dead-letter Exchange");
            channel.ExchangeDeclare("dead.letter.exchange", ExchangeType.Fanout, durable: true);
            channel.QueueDeclare("dead.letter.queue", durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind("dead.letter.queue", "dead.letter.exchange", "");
            Console.WriteLine("Dead-letter Declared! and dead.letter.queue is binded.");
        }
        private void BindQueueWithExchange()
        {
            Console.WriteLine("Binding Queue with Exchange");
            bool isBindToExchange = false;
            IModel? channel = _connection.Channel;
            while (!isBindToExchange)
            {

                try
                {
                    string[] routingKey = queueConfig.RoutingKeys;
                    if (channel == null || channel.IsClosed)
                    {
                        channel = _connection.Channel;
                    }
                    foreach (var key in routingKey)
                    {
                        if (channel != null && channel.IsOpen)
                        {
                            channel.QueueBind(queueConfig.QueueName, queueConfig.ExchangeName, key);
                            isBindToExchange = true;
                            Console.WriteLine($"{queueConfig.QueueName} binds to Exchange {queueConfig.ExchangeName} routing key: {key}");
                        }
                    }
                    if (routingKey.Length == 0)
                    {
                        if (channel != null && channel.IsOpen)
                        {
                            channel.QueueBind(queueConfig.QueueName, queueConfig.ExchangeName, string.Empty);
                            isBindToExchange = true;
                            Console.WriteLine($"{queueConfig.QueueName} binds to Exchange {queueConfig.ExchangeName}");
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exchange : {queueConfig.ExchangeName} does not exist");
                    Console.WriteLine("Wating for Exchange to be created! re-try to bind");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }

        }

    }
}
