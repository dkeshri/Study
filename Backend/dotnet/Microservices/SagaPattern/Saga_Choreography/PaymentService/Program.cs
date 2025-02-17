using Contract;
using MassTransit;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.AddConsumer<PaymentProcessedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host("localhost", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            cfg.ConfigureEndpoints(context);
        });
    });
});

var host = builder.UseConsoleLifetime().Build();
host.RunAsync().Wait();

public class PaymentProcessedConsumer : IConsumer<PaymentProcessed>
{
    public async Task Consume(ConsumeContext<PaymentProcessed> context)
    {
        var success = new Random().Next(2) == 0;
        if (success)
        {
            Console.WriteLine($"Payment succeeded for Order {context.Message.OrderId}");
            await context.Publish(new InventoryUpdated(context.Message.OrderId));
        }
        else
        {
            Console.WriteLine($"Payment failed for Order {context.Message.OrderId}");
            await context.Publish(new PaymentFailed(context.Message.OrderId, "Insufficient funds"));
        }
    }
}
