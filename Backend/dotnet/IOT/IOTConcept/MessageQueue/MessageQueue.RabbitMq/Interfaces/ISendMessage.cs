using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Interfaces
{
    public interface ISendMessage
    {
        public void Send(string message);
    }
    public interface ISendMessageToQueue : ISendMessage
    {

    }
    public interface ISendMessageToDirectExchange : ISendMessage
    {

    }
   

}
