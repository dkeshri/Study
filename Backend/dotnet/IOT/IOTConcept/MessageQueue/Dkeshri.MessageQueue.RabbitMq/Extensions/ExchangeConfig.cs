using Dkeshri.MessageQueue.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public class ExchangeConfig
    {
        public string ExchangeName { get; set; } = MessageQueueConstant.UNKNOWN;
        public bool IsDurable { get; set; } = false;
        public bool IsExclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public IDictionary<string, object>? Arguments { get; set; }
    }
}
