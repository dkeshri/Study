using Contract;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Configure MassTransit with Saga
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<PaymentConsumer>();
    x.AddConsumer<PaymentFailedConsumer>();

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


public class PaymentConsumer : IConsumer<ProcessPayment>
{
    public async Task Consume(ConsumeContext<ProcessPayment> context)
    {
        var success = new Random().Next(2) == 0;
        if (success)
        {
            Console.WriteLine($"Payment successful for Order {context.Message.OrderId}");
            await context.Publish(new PaymentProcessed(context.Message.OrderId));
        }
        else
        {
            Console.WriteLine($"Payment failed for Order {context.Message.OrderId}");
            await context.Publish(new PaymentFailed(context.Message.OrderId, "Insufficient funds"));
        }
    }
}

public class PaymentFailedConsumer : IConsumer<RollbackOrder>
{
    public async Task Consume(ConsumeContext<RollbackOrder> context)
    {
        Console.WriteLine($"Rolling back order {context.Message.OrderId}");
    }
}