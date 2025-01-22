using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Dkeshri.MessageQueue.RabbitMq.Logic
{
    internal class MessageBrokerInitializer : IStartup
    {
        private readonly IRabbitMqConnection _connection;
        private readonly QueueConfig queueConfig;
        private readonly ExchangeConfig exchangeConfig;
        public MessageBrokerInitializer(IRabbitMqConnection rabbitMqConnection)
        {
            _connection = rabbitMqConnection;
            queueConfig = rabbitMqConnection.Queue;
            exchangeConfig = rabbitMqConnection.Exchange;
        }
        public void OnStart()
        {
            IModel? channel = _connection.Channel;
            if (channel == null)
            {
                Console.WriteLine($"Error: Create exchange : {exchangeConfig.ExchangeName}, channel is set to null!");
                return;
            }

            if (!string.IsNullOrEmpty(exchangeConfig.ExchangeName))
            {
                if (channel.IsOpen) 
                {
                    channel.ExchangeDeclare(
                        exchange: exchangeConfig.ExchangeName,
                        type: exchangeConfig.ExchangeType,
                        durable: exchangeConfig.IsDurable,
                        autoDelete: exchangeConfig.AutoDelete,
                        arguments: exchangeConfig.Arguments
                    );
                    Console.WriteLine($"Exchange: {exchangeConfig.ExchangeName} created!");
                }
                else
                {
                    Console.WriteLine($"Error: Can't Create Exchange: {exchangeConfig.ExchangeName}, channel is not open!");
                }
                
            }

            if (!string.IsNullOrEmpty(queueConfig.QueueName)) 
            {
                if (channel.IsOpen)
                {
                    channel.QueueDeclare(queue: queueConfig.QueueName,
                         durable: queueConfig.IsDurable,
                         exclusive: queueConfig.IsExclusive,
                         autoDelete: queueConfig.IsAutoDelete,
                         arguments: queueConfig.Arguments);
                    Console.WriteLine($"Queue: {queueConfig.QueueName} created!");
                }
                else
                {
                    Console.WriteLine($"Error: Can't Create Queue: {queueConfig.QueueName}, channel is not open!");
                }
            }
        }
    }
}
