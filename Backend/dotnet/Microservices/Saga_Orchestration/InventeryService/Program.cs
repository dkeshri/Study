using Contract;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Configure MassTransit with Saga
builder.Services.AddMassTransit(x =>
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

var app = builder.Build();
app.Run();

public class InventoryConsumer : IConsumer<UpdateInventory>
{
    public async Task Consume(ConsumeContext<UpdateInventory> context)
    {
        Console.WriteLine($"Inventory updated for Order {context.Message.OrderId}");
        await context.Publish(new InventoryUpdated(context.Message.OrderId));
    }
}