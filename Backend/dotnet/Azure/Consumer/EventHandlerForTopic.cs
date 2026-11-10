using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Consumer;

public class EventHandlerForTopic
{
    private readonly ILogger<EventHandlerForTopic> _logger;

    public EventHandlerForTopic(ILogger<EventHandlerForTopic> logger)
    {
        _logger = logger;
    }

    [Function(nameof(OrderEventInventory))]
    public async Task OrderEventInventory(
        [ServiceBusTrigger("order-events","inventory-sub", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Processing inventory event...");
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }

    [Function(nameof(OrderEventPayment))]
    public async Task OrderEventPayment(
        [ServiceBusTrigger("order-events","payment-sub", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Processing payment event...");
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }

    [Function(nameof(OrderEventNotification))]
    public async Task OrderEventNotification(
        [ServiceBusTrigger("order-events","notification-sub", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Processing notification event...");
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}