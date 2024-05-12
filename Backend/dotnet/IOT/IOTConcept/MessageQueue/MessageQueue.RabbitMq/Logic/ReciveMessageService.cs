using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Logic
{
    public class ReciveMessageService : IHostedService, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IConnection _connection;
        public ReciveMessageService(IConnection connection)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _connection = connection;

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
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            return Task.CompletedTask;

        }

        private void InitMessageReciver()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");
            };
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

        }
        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}
