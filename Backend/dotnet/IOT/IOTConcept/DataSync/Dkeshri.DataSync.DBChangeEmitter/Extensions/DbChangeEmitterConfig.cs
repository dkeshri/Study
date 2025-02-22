﻿using Dkeshri.DataSync.Common.Extensions;
using Dkeshri.MessageQueue.Extensions;

namespace Dkeshri.DataSync.DBChangeEmitter.Extensions
{
    public class DbChangeEmitterConfig
    {
        public DatabaseType DatabaseType { get; set; }
        public DbConfig DbConfig { get; set; } = null!;
        public MessageBroker MessageBroker { get; set; } = null!;
        public string MessageRoutingKey {  get; set; } = "EmitterToReceiver";
    }
}
