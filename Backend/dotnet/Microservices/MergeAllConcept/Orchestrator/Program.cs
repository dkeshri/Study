using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestrator;
using Orchestrator.Data;
using Orchestrator.States;



var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(services =>
{
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer("Server=localhost,1433;Database=OrchestratorDb;User Id=sa;Password=MsSqlServer@2023;TrustServerCertificate=True;Encrypt=False;"));
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.AddSagaStateMachine<OrderSaga, OrderState>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<AppDbContext>();
            r.LockStatementProvider = new SqlServerLockStatementProvider();
        });

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host("rabbitmq-service", "/", h =>
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