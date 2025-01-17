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
        private IMessageHandler? _messageHandler;
        public RabbitMqMessageBroker(IRabbitMqConnection rabbitMqConnection,IMessageHandler? messageHandler = null)
        {
            _connection = rabbitMqConnection;
            _messageHandler = messageHandler;
        }

        public override IMessageReceiver CreateReceiver()
        {
            return _messageHandler as IMessageReceiver
               ?? throw new InvalidCastException("The injected IMessageHandler does not implement IMessageReceiver.");
        }

        public override ISendMessage CreateSender()
        {
            return new SendMessage(_connection);
        }
    }
}
