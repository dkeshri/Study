using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;


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
                InitMessageReciver(channel, cancellationToken);
            }

            Console.WriteLine("Rabbit MQ Message Receiver Service has started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            _isQueueServiceStopping = true;
            Dispose(true);
            Console.WriteLine("Shutdown RabbitMq Reciver Hosted Service");
            return Task.CompletedTask;

        }

        private void InitMessageReciver(IModel? channel, CancellationToken? cancellationToken = null)
        {

            try
            {
                Console.WriteLine("Initializing Receiver with RabbitMq...");
                channel?.QueueDeclare(queue: queueConfig.QueueName,
                     durable: queueConfig.IsDurable,
                     exclusive: queueConfig.IsExclusive,
                     autoDelete: queueConfig.IsAutoDelete,
                     arguments: queueConfig.Arguments);
                channel?.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                
                if (!string.IsNullOrEmpty(queueConfig.ExchangeName))
                {
                    channel = BindQueueWithExchange(cancellationToken);
                }
                else
                {
                    Console.WriteLine($"Queue: {queueConfig.QueueName} did not bind to any Exchange, Exchnage name not provided!");
                    Console.WriteLine("Queue is standalone. Receive Messages if directly Publish to Queue!");
                }

                if (channel != null && channel.IsOpen) 
                {
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
                    InitMessageReciver(channel,cancellationToken);
                    break;
                }
            }

        }
        private IModel? BindQueueWithExchange(CancellationToken? cancellationToken = null)
        {
            Console.WriteLine("Binding Queue with Exchange");
            bool isBindToExchange = false;
            IModel? channel = _connection.Channel;
            while (!isBindToExchange && !_isQueueServiceStopping) 
            {

                try
                {
                    if (cancellationToken?.IsCancellationRequested == true)
                    {
                        _isQueueServiceStopping = true;
                        break;
                    }
                    string[] routingKey = queueConfig.RoutingKeys;
                    if(channel == null || channel.IsClosed)
                    {
                        channel = _connection.Channel;
                    }
                    foreach (var key in routingKey)
                    {
                        if (channel!=null && channel.IsOpen)
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
            return channel;
            
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
