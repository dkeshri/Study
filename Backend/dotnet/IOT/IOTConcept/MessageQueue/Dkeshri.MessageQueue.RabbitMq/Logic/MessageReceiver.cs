using Dkeshri.MessageQueue.Interfaces;

namespace Dkeshri.MessageQueue.RabbitMq.Logic
{
    internal sealed class MessageReceiver : IMessageReceiver
    {
        public Predicate<string>? MessageHandler { get; set; }
    }
}
