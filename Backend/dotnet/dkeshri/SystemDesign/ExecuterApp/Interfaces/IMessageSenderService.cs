using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuterApp.Interfaces
{
    internal interface IMessageSenderService
    {
        void SendMessageToQueue();
    }
}
