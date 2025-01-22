namespace Dkeshri.MessageQueue.Interfaces
{
    public interface IMessageSender
    {
        public bool SendToQueue(string message);
        public bool SendToQueue(string queueName, string message);
        public bool SendToExchange(string message, string? routingKey);
    }
}
