using Contract;
using MassTransit;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.AddConsumer<InventoryConsumer>();

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

public class InventoryConsumer : IConsumer<UpdateInventory>
{
    public async Task Consume(ConsumeContext<UpdateInventory> context)
    {
        Console.WriteLine($"Inventory updated for Order {context.Message.OrderId}");
        await context.Publish(new InventoryUpdated(context.Message.OrderId));
    }
}