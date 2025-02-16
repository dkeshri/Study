using Contract;
using MassTransit;
using OrderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // It will register the consumer with provide endpoint
        //cfg.ReceiveEndpoint("order-service-queue", e => e.ConfigureConsumer<OrderCreatedConsumer>(context));

        // It will Auto create Endpoints Baseed on IConsumer Type 
        // OrderCreated is the Type of consumer so it will create order-created Queue in RabbitMq.
        // Name formate will be decided by SetKebabCaseEndpointNameFormatter(); at line 10.
        cfg.ConfigureEndpoints(context);
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
