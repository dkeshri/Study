using Azure.Messaging.ServiceBus;
using Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Sender;

public class SenderFunction
{
    private readonly ILogger<SenderFunction> _logger;

    ServiceBusClient serviceBusClient;
    public SenderFunction(ILogger<SenderFunction> logger,ServiceBusClient serviceBusClient)
    {
        _logger = logger;
        this.serviceBusClient = serviceBusClient;
    }

    [Function(nameof(SendMessageToQueue))]
    public async Task<IActionResult> SendMessageToQueue([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Send to queue
        var sender = serviceBusClient.CreateSender("order-queue");

        Order orderCreated = new Order()
        {
            OrderId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            TotalAmount = 100.0m,
            ProductId = "Product-1",
            Quantity = 1,
            CreatedAtUtc = DateTime.UtcNow
        };
        var messageBody = JsonSerializer.Serialize(orderCreated);
        var message = new ServiceBusMessage(messageBody)
            {
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString()
            };

        await sender.SendMessageAsync(message);


        Console.WriteLine("Message sent to order-queue!");

        await sender.DisposeAsync();

        return new OkObjectResult("Message sent to order-queue!");
    }

    [Function(nameof(SendMessageToTopic))]
    public async Task<IActionResult> SendMessageToTopic([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Send to topic
        var sender = serviceBusClient.CreateSender("order-events");

        Order orderCreated = new Order()
        {
            OrderId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            TotalAmount = 100.0m,
            ProductId = "Product-1",
            Quantity = 1,
            CreatedAtUtc = DateTime.UtcNow
        };


        OrderEvent orderEvent = new OrderEvent()
        {
            OrderId = orderCreated.OrderId,
            CustomerId = orderCreated.CustomerId,
            ProductId = orderCreated.ProductId,
            CreatedAtUtc = orderCreated.CreatedAtUtc
        };



        var messageBody = JsonSerializer.Serialize(orderEvent);
        var message = new ServiceBusMessage(messageBody)
        {
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString()
        };

        await sender.SendMessageAsync(message);


        Console.WriteLine("Message sent to topic: order-events !");

        await sender.DisposeAsync();

        return new OkObjectResult("Message sent to topic: order-events !");
    }

}