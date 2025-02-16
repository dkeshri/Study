using Contract;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<InventoryUpdatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("inventory-service-queue", e => e.ConfigureConsumer<InventoryUpdatedConsumer>(context));
    });
});

var app = builder.Build();
app.Run();

public class InventoryUpdatedConsumer : IConsumer<InventoryUpdated>
{
    public async Task Consume(ConsumeContext<InventoryUpdated> context)
    {
        Console.WriteLine($"Inventory updated for Order {context.Message.OrderId}");
    }
}