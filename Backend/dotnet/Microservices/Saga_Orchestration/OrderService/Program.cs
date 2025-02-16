// Program.cs - Saga Orchestration Setup
using MassTransit;
using OrderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<CreateOrderService>();
// Configure MassTransit with Saga
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

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