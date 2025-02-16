using MassTransit;
using Microsoft.Extensions.Hosting;
using Orchestrator;
using Orchestrator.States;



var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(services =>
{
    services.AddMassTransit(x =>
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

});
var host = builder.UseConsoleLifetime().Build();
host.RunAsync().Wait();