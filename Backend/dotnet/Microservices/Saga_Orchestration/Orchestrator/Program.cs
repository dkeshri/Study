using MassTransit;
using Orchestrator;
using Orchestrator.States;

var builder = WebApplication.CreateBuilder(args);

// Configure MassTransit with Saga
builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderSaga, OrderState>().InMemoryRepository();
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