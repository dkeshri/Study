using Dkeshri.DataSync.Common.Models;
using Dkeshri.DataSync.DbChangeReceiver.Interfaces;
using Dkeshri.DataSync.DbChangeReceiver.Notifications;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;


namespace Dkeshri.DataSync.DbChangeReceiver.Handlers
{
    internal class RabbitMqMessageHandler : IRabbitMqMessageHandler
    {
        IMediator mediator;
        public RabbitMqMessageHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public bool HandleMessage(string message)
        {
            try
            {
                Console.WriteLine(message);
                IReadOnlyCollection<TableChanges>? tableChanges = DeserializerDbChangesMessage(message);
                
                if(tableChanges== null) return true;

                TableChangesNotification notificationMessage = new TableChangesNotification()
                {
                    TableChanges = tableChanges
                };
                mediator.Publish(notificationMessage);
            }
            catch (Exception) { 
                return false;
            }
            return true;
        }

        private IReadOnlyCollection<TableChanges>? DeserializerDbChangesMessage(string message)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Allows matching property names in a case-insensitive way
            };
            List<TableChanges>? tableChanges = JsonSerializer.Deserialize<List<TableChanges>>(message, options);
            return tableChanges;

        }
    }
}
