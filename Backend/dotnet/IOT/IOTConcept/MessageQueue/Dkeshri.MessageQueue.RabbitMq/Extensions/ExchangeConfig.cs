﻿using Dkeshri.MessageQueue.Constants;

namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public class ExchangeConfig
    {
        public string? ExchangeName { get; set; }
        public bool IsDurable { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public IDictionary<string, object> Arguments { get; }
        public string ExchangeType { get; set; } = RabbitMQ.Client.ExchangeType.Direct;
        public ExchangeConfig()
        {
            Arguments = new Dictionary<string, object>();
        }
    }
}
