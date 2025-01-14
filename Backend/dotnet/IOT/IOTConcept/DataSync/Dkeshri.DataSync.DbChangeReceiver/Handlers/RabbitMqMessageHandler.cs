﻿using Dkeshri.DataSync.Common.Models;
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
        public void HandleMessage(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var body = ea.Body.ToArray();
            IReadOnlyCollection<TableChanges>? tableChanges = DeserializerDbChangesMessage(body);
            if (tableChanges != null)
            {
                TableChangesNotification notificationMessage = new TableChangesNotification()
                {
                    TableChanges = tableChanges
                };
                mediator.Publish(notificationMessage);
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }

        private IReadOnlyCollection<TableChanges>? DeserializerDbChangesMessage(byte[] body)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Allows matching property names in a case-insensitive way
            };
            var message = Encoding.UTF8.GetString(body);
            List<TableChanges>? tableChanges = JsonSerializer.Deserialize<List<TableChanges>>(message, options);
            return tableChanges;

        }
    }
}
