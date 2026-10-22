using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunction.Functions;

public class ServiceBusConsumer
{
    private readonly ILogger<ServiceBusConsumer> _logger;

    public ServiceBusConsumer(ILogger<ServiceBusConsumer> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ServiceBusConsumer))]
    public async Task Run(
        [ServiceBusTrigger("topic.1", "subscription.1", Connection = "MyServiceBus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}