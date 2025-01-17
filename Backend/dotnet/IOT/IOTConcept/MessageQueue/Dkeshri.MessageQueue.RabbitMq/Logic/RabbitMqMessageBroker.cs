using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Handlers;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.MessageQueue.RabbitMq.Logic
{
    internal class RabbitMqMessageBroker : MessageBrokerFactory
    {
        private readonly IRabbitMqConnection _connection;
        public RabbitMqMessageBroker(IRabbitMqConnection rabbitMqConnection)
        {
            _connection = rabbitMqConnection;
        }

        public override IMessageReceiver CreateReceiver()
        {
            return new MessageReceiverHandler();
        }

        public override ISendMessage CreateSender()
        {
            return new SendMessage(_connection);
        }
    }
}
