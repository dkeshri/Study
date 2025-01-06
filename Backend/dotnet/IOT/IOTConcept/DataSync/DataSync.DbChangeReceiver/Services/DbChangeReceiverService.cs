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
        private IRabbitMqMessageHandler _messageHandler;
        private IMessageReceiver _messageReceiver;
        public DbChangeReceiverService(IMessageReceiver messageReceiver, IRabbitMqMessageHandler rabbitMqMessageHandler)
        {
            _messageHandler = rabbitMqMessageHandler;
            _messageReceiver = messageReceiver;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Data-Sync-Receiver Service is starting");
            _messageReceiver.MessageHandler = _messageHandler.HandleMessage;
            Console.WriteLine("Data-Sync-Receiver Service has started!");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose(true);
            Console.WriteLine("DbChange Receiver Reciver Hosted Service");
            return Task.CompletedTask;

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
                
            }
            _isDisposing = true;
        }
    }
}
