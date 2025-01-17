using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dkeshri.DataSync.DbChangeReceiver.Interfaces
{
    internal interface IRabbitMqMessageHandler
    {
        public bool HandleMessage(string message);
    }
}
