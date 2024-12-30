using DataSync.DbChangeReceiver.Interfaces;
using MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Services
{
    internal class DbChangeReceiverService : IHostedService, IDisposable
    {
        private bool _isDisposing = false;
        private readonly string _queueName;
        private IRabbitMqMessageHandler _messageHandler;
        private readonly IRabbitMqConnection _connection;
        public DbChangeReceiverService(IRabbitMqConnection rabbitMqConnection, IRabbitMqMessageHandler rabbitMqMessageHandler)
        {
            _connection = rabbitMqConnection;
            _queueName = rabbitMqConnection.QueueName;
            _messageHandler = rabbitMqMessageHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Rabbit MQ Message Reciver Service is starting");
            InitMessageReciverWithAck();
            Console.WriteLine("Rabbit MQ Message Reciver Service has started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose(true);
            Console.WriteLine("Shutdown RabbitMq Reciver Hosted Service");
            return Task.CompletedTask;

        }

        private void InitMessageReciverWithAck()
        {
            IModel? channel = _connection.Channel;
            if(channel== null)
            {
                Console.WriteLine("Receiver: Not able to connect to RabbitMQ, Channel is set to null!");
                return;
            }
            channel.QueueDeclare(queue: _queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                _messageHandler.HandleMessage(model, ea, channel);
            };
            channel.BasicConsume(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);
        }
        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            
            if (_isDisposing)
            {
                return;
            }
            if (disposing)
            {
                IModel? channel = _connection.Channel;
                if (channel != null && channel.IsOpen)
                {
                    channel.Close();
                    channel.Dispose();
                }
            }
            _isDisposing = true;
        }
    }
}
