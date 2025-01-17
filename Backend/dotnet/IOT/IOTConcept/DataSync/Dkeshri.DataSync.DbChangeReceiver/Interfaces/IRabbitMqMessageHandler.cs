namespace Dkeshri.DataSync.DbChangeReceiver.Interfaces
{
    internal interface IRabbitMqMessageHandler
    {
        public bool HandleMessage(string message);
    }
}
