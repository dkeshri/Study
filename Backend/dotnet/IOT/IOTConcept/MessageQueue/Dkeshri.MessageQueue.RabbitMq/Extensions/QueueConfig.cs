using Dkeshri.MessageQueue.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public class QueueConfig
    {
        public string QueueName { get; set; } = MessageQueueConstant.UNKNOWN;
        public bool IsDurable { get; set; } = false;
        public bool IsExclusive { get; set; } = false;
        public bool IsAutoDelete { get; set; } = false;
        public IDictionary<string, object>? Arguments { get; set; }
    }
}
