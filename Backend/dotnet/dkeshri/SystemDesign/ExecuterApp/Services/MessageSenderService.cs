using Dkeshri.MessageQueue.Interfaces;
using ExecuterApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuterApp.Services
{
    internal class MessageSenderService : IMessageSenderService
    {
        ISendMessage _sendMessage;
        public MessageSenderService(ISendMessage sendMessage)
        {
            _sendMessage = sendMessage;
        }

        public void SendMessageToQueue()
        {
            _sendMessage.SendToQueue("First Message to Queue");
        }
    }
}
