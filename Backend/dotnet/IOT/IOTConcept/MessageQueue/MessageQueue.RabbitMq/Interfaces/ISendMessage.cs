using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Interfaces
{
    public interface ISendMessage
    {
        public bool SendToQueue(string message);
        public bool SendToQueue(string queueName, string message);
        public bool SendToExchange(string message, string? routingKey);
    }
    
}
