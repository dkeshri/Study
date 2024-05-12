using MessageQueue.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Logic
{
    public class SendMessageToQueue : SendMessage,ISendMessageToQueue
    {
        public SendMessageToQueue(IConnection connection):base(connection) { }
        
    }
}
