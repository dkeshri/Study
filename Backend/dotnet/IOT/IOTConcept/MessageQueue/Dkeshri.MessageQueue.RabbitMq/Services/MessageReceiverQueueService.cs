using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace MessageQueue.RabbitMq.Services
{
    internal class MessageReceiverQueueService : IHostedService,IDisposable
    {
        private bool _isDisposing = false;
        private bool _isQueueServiceStopping = false;
        private readonly IRabbitMqConnection _connection;
        private IMessageHandler _messageHanadler;
        private readonly QueueConfig queueConfig;
        public MessageReceiverQueueService(IRabbitMqConnection rabbitMqConnection,IMessageHandler messageHandler)
        {
            _connection = rabbitMqConnection;
            queueConfig = rabbitMqConnection.Queue;
            _messageHanadler = messageHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Rabbit MQ Message Reciver Service is starting");
            _isQueueServiceStopping = false;
            IModel? channel = _connection.Channel;
            if (channel == null || channel.IsClosed)
            {
                TryReconnectWithDelay(cancellationToken);
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

            _isQueueServiceStopping = true;
            Dispose(true);
            Console.WriteLine("Shutdown RabbitMq Reciver Hosted Service");
            return Task.CompletedTask;

        }

        private void InitMessageReciver(IModel channel)
        {

            try
            {
                Console.WriteLine("Initializing Receiver with RabbitMq...");
                channel.QueueDeclare(queue: queueConfig.QueueName,
                     durable: queueConfig.IsDurable,
                     exclusive: queueConfig.IsExclusive,
                     autoDelete: queueConfig.IsAutoDelete,
                     arguments: queueConfig.Arguments);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                
                if (!string.IsNullOrEmpty(queueConfig.ExchangeName))
                {
                    string[] routingKey = queueConfig.RoutingKeys;
                    foreach (var key in routingKey)
                    {
                        channel.QueueBind(queueConfig.QueueName, queueConfig.ExchangeName, key);
                    }
                    if (routingKey.Length == 0)
                    {
                        channel.QueueBind(queueConfig.QueueName, queueConfig.ExchangeName, string.Empty);
                    }
                }
                
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                   _messageHanadler.HandleMessage(model, ea, channel);
                };
                channel.BasicConsume(queue: queueConfig.QueueName,
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

        private void TryReconnectWithDelay(CancellationToken? cancellationToken = null)
        {
            if (_isQueueServiceStopping)
            {
                Console.WriteLine("MessageReceiverFromQueueService is Stopping Do not Attempt to reconnect!");
                return;
            }
            IModel? channel = _connection.Channel;
            while (channel == null || channel.IsClosed)
            {
                
                Console.WriteLine("Attempting to reconnect...");
                if(cancellationToken?.IsCancellationRequested == true)
                {
                    _isQueueServiceStopping = true;
                    break;
                }

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
