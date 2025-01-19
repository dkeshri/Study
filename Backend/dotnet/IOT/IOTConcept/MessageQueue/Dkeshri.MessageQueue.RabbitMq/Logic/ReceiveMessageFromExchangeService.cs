using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;


namespace MessageQueue.RabbitMq.Logic
{
    public class ReceiveMessageFromExchangeService : IHostedService, IDisposable
    {
        private IModel? channel;
        private bool _isDisposing = false;
        private readonly string _directExchangeName;
        private readonly string queueName = "directExchangeQueue";
        private string[] routingkeys = { "key1", "key2", string.Empty };
        public ReceiveMessageFromExchangeService(IRabbitMqConnection rabbitMqConnection)
        {
            channel = rabbitMqConnection.Channel;
            _directExchangeName = rabbitMqConnection.Exchange.ExchangeName;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Rabbit MQ Message Reciver Service is starting");
            InitMessageReciver();
            Console.WriteLine("Rabbit MQ Message Reciver Service has started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose(true);
            Console.WriteLine("Shutdown RabbitMq Reciver Hosted Service");
            return Task.CompletedTask;

        }

        private void InitMessageReciver()
        {
            channel?.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            foreach (string routingkey in routingkeys)
            {
                channel.QueueBind(queueName, _directExchangeName, routingkey);
            }

            channel?.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"{nameof(ReceiveMessageFromExchangeService)} [x] Received {message}");
                if (message.Contains("error"))
                {
                    Console.WriteLine("Error in response so rejecting it.");
                    channel?.BasicReject(ea.DeliveryTag, false);
                    throw new Exception("Error in code");
                }

                if (int.TryParse(message, out var delayTime))
                    Thread.Sleep(delayTime * 1000);

                Console.WriteLine($"Processed Message: {message}");
                channel?.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

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
