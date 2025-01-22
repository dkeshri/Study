using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;


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

        public override IStartup CreateInitializer()
        {
            return new MessageBrokerInitializer(_connection);
        }

        public override IMessageReceiver CreateReceiver()
        {
            return _messageHandler as IMessageReceiver
               ?? throw new InvalidCastException("The injected IMessageHandler does not implement IMessageReceiver.");
        }

        public override IMessageSender CreateSender()
        {
            return new MessageSender(_connection);
        }
    }
}
