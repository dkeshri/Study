using Dkeshri.DataSync.Common.Models;
using Dkeshri.DataSync.DBChangeEmitter.Extensions;
using Dkeshri.DataSync.DBChangeEmitter.Interfaces;
using Dkeshri.MessageQueue.Interfaces;
using System.Text.Json;

namespace Dkeshri.DataSync.DBChangeEmitter.Services
{
    internal class SendMessageToRabbiMq : ISendMessageToRabbitMq
    {
        private ISendMessage SendMessage { get; }

        private readonly string? _routingKey;
        private readonly bool UseExchangeToSendMessage;
        public SendMessageToRabbiMq(ISendMessage sendMessage, DbChangeEmitterConfig config)
        {
            SendMessage = sendMessage;
            _routingKey = config.MessageBroker.ExchangeRoutingKey;
            UseExchangeToSendMessage = config.MessageBroker.UseExchangeToSendMessage;
        }
        public bool SendMessageToRabbitMq(IReadOnlyCollection<TableChanges> tableChanges)
        {
            string message = SerializeJsonObject(tableChanges);
            Console.WriteLine(message);
            if (UseExchangeToSendMessage) 
            {
                
                return SendMessage.SendToExchange(message, _routingKey);
            }
            else
            {
                return SendMessage.SendToQueue(message);
            }
            
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
