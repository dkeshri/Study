namespace Dkeshri.MessageQueue.Interfaces
{
    public abstract class MessageBrokerFactory
    {
        public abstract IMessageSender CreateSender();
        public abstract IMessageReceiver CreateReceiver();
        public abstract IStartup CreateInitializer();
    }
}
