using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Consumer;

public class ProcessMessageFromQueue
{
    private readonly ILogger<ProcessMessageFromQueue> _logger;

    public ProcessMessageFromQueue(ILogger<ProcessMessageFromQueue> logger)
    {
        _logger = logger;
    }


    /* This function will be triggered when a message is received from the "order-queue" Service Bus queue.
    // The message will be automatically completed after the function successfully processes it.
    // when we have multiple instances of the same function, each instance will receive a different message from the queue, 
        allowing for parallel processing of messages.
        here we have two instances OrderQueueInstance1 and OrderQueueInstance2 of the same function, 
        each instance will receive a different message from the "order-queue" Service Bus queue, allowing for parallel processing of messages.
    */
    [Function(nameof(OrderQueueInstance1))]
    public async Task OrderQueueInstance1(
        [ServiceBusTrigger("order-queue", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }

    [Function(nameof(OrderQueueInstance2))]
    public async Task OrderQueueInstance2(
        [ServiceBusTrigger("order-queue", Connection = "ServiceBusConnection")]
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