using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.MessageQueue.Extensions
{
    public class MessageBroker
    {
        public IServiceCollection Services { get; set; } = null!;
        public bool RegisterSenderServices { get; set; } = false;
        public bool RegisterReceiverServices { get; set; } = false;
        public string ClientProvidedName { get; set; } = "Unknown";
    }
}
