using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Interfaces
{
    public interface ISendMessage
    {
        public void SendToQueue(string message);
        public void SendToExchange(string message, string? routingKey);
    }
    
}
