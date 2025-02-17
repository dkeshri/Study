using Contract;
using MassTransit;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();
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
});

var host = builder.UseConsoleLifetime().Build();
host.RunAsync().Wait();

public class PaymentFailedConsumer : IConsumer<PaymentFailed>
{
    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        Console.WriteLine($"Rolling back order {context.Message.OrderId} due to payment failure: {context.Message.Reason}");
    }
}