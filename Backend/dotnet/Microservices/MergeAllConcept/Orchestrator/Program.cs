using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestrator;
using Orchestrator.Data;
using Orchestrator.Extensions;
using Orchestrator.States;



var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((context, config) =>
{
    var env = context.HostingEnvironment;
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Read from appsettings.json
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables(); // Read from environment variables
});
builder.ConfigureServices((hostContext, services) =>
{
    string dbConnectionString = hostContext.Configuration.GetDbConnectionString();
    int dbTransationTimeOut = hostContext.Configuration.GetDbTransactionTimeOutInSec();
    var rabbitMqConfiguration = hostContext.Configuration.GetRabbitMqConfiguration();
    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(dbConnectionString);
    });
        
        
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.AddSagaStateMachine<OrderSaga, OrderState>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<AppDbContext>();
            r.LockStatementProvider = new SqlServerLockStatementProvider();
        });

        if(rabbitMqConfiguration != null)
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfiguration.HostName, "/", h =>
                {
                    h.Username(rabbitMqConfiguration.UserName);
                    h.Password(rabbitMqConfiguration.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        }
        
    });

});
var host = builder.UseConsoleLifetime().Build();
host.RunAsync().Wait();