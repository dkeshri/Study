using Contract;
using MassTransit;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.AddConsumer<InventoryUpdatedConsumer>();

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

public class InventoryUpdatedConsumer : IConsumer<InventoryUpdated>
{
    public async Task Consume(ConsumeContext<InventoryUpdated> context)
    {
        Console.WriteLine($"Inventory updated for Order {context.Message.OrderId}");
    }
}