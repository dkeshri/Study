using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("order-service-queue", e => e.ConfigureConsumer<OrderCreatedConsumer>(context));
    });
});

var app = builder.Build();
app.Run();

public record OrderCreated(Guid OrderId, decimal Amount);
public record PaymentProcessed(Guid OrderId);
public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        Console.WriteLine($"Order {context.Message.OrderId} created.");
        await context.Publish(new PaymentProcessed(context.Message.OrderId));
    }
}
