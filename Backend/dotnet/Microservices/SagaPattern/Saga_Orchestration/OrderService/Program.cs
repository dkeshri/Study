﻿using Contract;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(services =>
{
    services.AddHostedService<CreateOrderBackgroundService>();

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

public class PaymentFailedConsumer : IConsumer<CancelOrder>
{
    public async Task Consume(ConsumeContext<CancelOrder> context)
    {
        Console.WriteLine($"Cancling order {context.Message.OrderId}");
        await context.Publish(new OrderCanceled(context.Message.OrderId));
    }
}