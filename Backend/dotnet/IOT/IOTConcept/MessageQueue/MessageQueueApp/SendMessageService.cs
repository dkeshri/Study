using Dkeshri.MessageQueue.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueApp
{
    internal class SendMessageService : IHostedService
    {
        ISendMessage SendMessage { get; }
        public SendMessageService(ISendMessage sendMessage)
        {
            SendMessage = sendMessage;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            for(int i = 1; i < 5; i++)
            {
                SendMessage.SendToQueue("TEste Message"+i);
                Task.Delay(1000*i);
            }
            SendMessage.SendToQueue("TEste Message");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
