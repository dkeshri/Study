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
    public class ReciveMessageFromQueueService : IHostedService, IDisposable
    {
        private readonly IConnection _connection;
        private IModel channel;
        private bool _isDisposing = false;
        private bool isAutoAck = false;
        public ReciveMessageFromQueueService(IConnection connection)
        {
            _connection = connection;
            channel = _connection.CreateModel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Rabbit MQ Message Reciver Service is starting");
            if (isAutoAck)
            {
                InitMessageReciverAutoAck();
            }
            else
            {
                InitMessageReciverWithAck();
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

        private void InitMessageReciverAutoAck()
        {
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

        private void InitMessageReciverWithAck()
        {
            channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($" [x] Received {message}");
                if (message.Contains("error"))
                {
                    Console.WriteLine("Error in response so rejecting it.");
                    channel.BasicReject(ea.DeliveryTag,false);
                    throw new Exception("Error in code");
                }
                Console.WriteLine($"Processed Message: {message}");
                channel.BasicAck(ea.DeliveryTag, false); 
            };

            channel.BasicConsume(queue: "hello",
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
                if (channel.IsOpen)
                {
                    channel.Close();
                    channel.Dispose();
                }
            }
            _isDisposing = true;
        }
    }
}
