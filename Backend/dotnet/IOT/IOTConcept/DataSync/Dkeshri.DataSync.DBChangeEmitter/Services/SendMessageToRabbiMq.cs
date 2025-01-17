using Dkeshri.DataSync.Common.Models;
using Dkeshri.DataSync.DBChangeEmitter.Interfaces;
using Dkeshri.MessageQueue.Interfaces;
using System.Text.Json;

namespace Dkeshri.DataSync.DBChangeEmitter.Services
{
    internal class SendMessageToRabbiMq : ISendMessageToRabbitMq
    {
        private ISendMessage SendMessage { get; }
        public SendMessageToRabbiMq(ISendMessage sendMessage)
        {
            SendMessage = sendMessage;
        }
        public bool SendMessageToRabbitMq(IReadOnlyCollection<TableChanges> tableChanges)
        {
            string message = SerializeJsonObject(tableChanges);
            Console.WriteLine(message);
            return SendMessage.SendToQueue(message);
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
