using DataSync.Common.Models;
using DataSync.DBChangeEmitter.Interfaces;
using MessageQueue.RabbitMq.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Services
{
    internal class SendMessageToRabbiMq : ISendMessageToRabbitMq
    {
        private ISendMessage SendMessage { get; }
        public SendMessageToRabbiMq(ISendMessage sendMessage)
        {
            SendMessage = sendMessage;
        }
        public void SendMessageToRabbitMq(IReadOnlyCollection<TableChanges> tableChanges)
        {
            string message = SerializeJsonObject(tableChanges);
            SendMessage.SendToQueue(message);
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
