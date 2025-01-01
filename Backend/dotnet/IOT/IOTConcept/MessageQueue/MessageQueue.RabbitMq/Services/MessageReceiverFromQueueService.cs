using MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Services
{
    internal class MessageReceiverFromQueueService : IHostedService,IDisposable
    {
        private bool _isDisposing = false;
        private readonly string _queueName;
        private readonly IRabbitMqConnection _connection;
        private IMessageReceiver _messageReceiver;
        public MessageReceiverFromQueueService(IRabbitMqConnection rabbitMqConnection,IMessageReceiver messageReceiver)
        {
            _connection = rabbitMqConnection;
            _queueName = rabbitMqConnection.QueueName;
            _messageReceiver = messageReceiver;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Rabbit MQ Message Reciver Service is starting");

            IModel? channel = _connection.Channel;
            if (channel == null || channel.IsClosed)
            {
                TryReconnectWithDelay();
            }
            else
            {
                InitMessageReciver(channel);
            }

            Console.WriteLine("Rabbit MQ Message Reciver Service has started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose(true);
            Console.WriteLine("Shutdown RabbitMq Reciver Hosted Service");
            return Task.CompletedTask;

        }

        private void InitMessageReciver(IModel channel)
        {

            try
            {
                Console.WriteLine("Initializing Receiver with RabbitMq...");
                channel.QueueDeclare(queue: _queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    _messageReceiver.HandleMessage(model, ea, channel);
                };
                channel.BasicConsume(queue: _queueName,
                                     autoAck: false,
                                     consumer: consumer);

                channel.ModelShutdown += OnChannelShutdown;
                Console.WriteLine("Receiver is connected to RabbitMq...");
            }
            catch (Exception ex)
            {
                throw new Exception("There is a problem while connecting to RabbitMq, channel is not open", ex);
            }

        }

        private void OnChannelShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Channel is Shutting down!");
            TryReconnectWithDelay();
        }

        private void TryReconnectWithDelay()
        {
            IModel? channel = _connection.Channel;
            while (channel == null || channel.IsClosed)
            {

                Console.WriteLine("Attempting to reconnect...");
                channel = _connection.Channel;
                Thread.Sleep(TimeSpan.FromSeconds(10));
                if (channel != null && channel.IsOpen)
                {
                    InitMessageReciver(channel);
                    break;
                }
            }

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
