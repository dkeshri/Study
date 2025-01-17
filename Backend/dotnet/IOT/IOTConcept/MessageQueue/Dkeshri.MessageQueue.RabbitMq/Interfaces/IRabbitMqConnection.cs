using RabbitMQ.Client;

namespace Dkeshri.MessageQueue.RabbitMq.Interfaces
{
    public interface IRabbitMqConnection
    {
        IModel? Channel { get; }
        string ExchangeName { get; }
        string QueueName {  get; }
        internal void EnableConfirmIfNotSelected();

    }
}
