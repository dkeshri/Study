using Contract;
using MassTransit;
using OrderService;

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
builder.Services.AddHostedService<CreateOrder>();
var app = builder.Build();
app.Run();

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    ILogger<OrderCreatedConsumer> logger;
    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
    {
        this.logger = logger;
    }
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        logger.LogInformation($"Order {context.Message.OrderId} created.");
        await context.Publish(new PaymentProcessed(context.Message.OrderId));
    }
}
