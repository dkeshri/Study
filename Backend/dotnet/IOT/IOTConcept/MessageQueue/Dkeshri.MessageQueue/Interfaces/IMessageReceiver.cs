using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.MessageQueue.Interfaces
{
    public interface IMessageReceiver
    {
        public Predicate<string>? MessageHandler { get; set; }
    }
}
