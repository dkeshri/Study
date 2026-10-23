using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;


namespace Dkeshri.MessageQueue.RabbitMq.Logic
{
    internal class RabbitMqMessageBroker : MessageBrokerFactory
    {
        private readonly IRabbitMqConnection _connection;
        public RabbitMqMessageBroker(IRabbitMqConnection rabbitMqConnection)
        {
            _connection = rabbitMqConnection;
        }

        public override IStartup CreateInitializer()
        {
            return new MessageBrokerInitializer(_connection);
        }

        public override IMessageReceiver CreateReceiver()
        {
            return new MessageReceiver();
        }

        public override IMessageSender CreateSender()
        {
            return new MessageSender(_connection);
        }
    }
}
