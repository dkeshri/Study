using Contract;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentFailedConsumer>();
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        //cfg.ReceiveEndpoint("rollback-queue", e => e.ConfigureConsumer<PaymentFailedConsumer>(context));

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.Run();

public class PaymentFailedConsumer : IConsumer<PaymentFailed>
{
    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        Console.WriteLine($"Rolling back order {context.Message.OrderId} due to payment failure: {context.Message.Reason}");
    }
}