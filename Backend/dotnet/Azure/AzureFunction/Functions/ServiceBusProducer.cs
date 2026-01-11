using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace AzureFunction.Functions;

public class ServiceBusProducer
{
    private readonly ILogger _logger;

    public ServiceBusProducer(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ServiceBusProducer>();
    }

    [Function("ServiceBusProducer")]
    public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);

        string connectionString = "Endpoint=sb://localhost:5672;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
        await using var client = new ServiceBusClient(connectionString);

        string topicName = "topic.1";

        var order = new
        {
            OrderId = Guid.NewGuid(),
            Amount = 1500,
            CreatedAt = DateTime.UtcNow
        };

        var message = new ServiceBusMessage(JsonSerializer.Serialize(order))
        {
            MessageId = order.OrderId.ToString(),
            ContentType = "application/json"
        };

        ServiceBusSender sender = client.CreateSender(topicName);
        await sender.SendMessageAsync(message);


        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}