using Dkeshri.DataSync.Common.Models;
using Dkeshri.DataSync.DBChangeEmitter.Extensions;
using Dkeshri.DataSync.DBChangeEmitter.Interfaces;
using Dkeshri.MessageQueue.Interfaces;
using System.Text.Json;

namespace Dkeshri.DataSync.DBChangeEmitter.Services
{
    internal class SendMessageToRabbiMq : ISendMessageToRabbitMq
    {
        private IMessageSender SendMessage { get; }

        private readonly string? _routingKey;
        public SendMessageToRabbiMq(IMessageSender sendMessage, DbChangeEmitterConfig config)
        {
            SendMessage = sendMessage;
            _routingKey = config.MessageRoutingKey;
        }
        public bool SendMessageToRabbitMq(IReadOnlyCollection<TableChanges> tableChanges)
        {
            string message = SerializeJsonObject(tableChanges);
            Console.WriteLine(message);
            return SendMessage.SendToExchange(message, _routingKey);
        }

        private string SerializeJsonObject(IReadOnlyCollection<TableChanges> tableChanges)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string jsonString = JsonSerializer.Serialize(tableChanges, options);
            return jsonString;
        }
    }
}
