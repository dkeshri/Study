using DataSync.DbChangeReceiver.Extenstions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    // Prevent the application from exiting immediately
    eventArgs.Cancel = true;
    cts.Cancel();
};

AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
{
    cts.Cancel();
};

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
    services.AddDbChangeReceiver((config) =>
    {
        config.AddRabbitMqBroker((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "localhost";
            rabbitMqConfig.Port = 5672;
            rabbitMqConfig.QueueName = "DataSyncQueue";
            rabbitMqConfig.UserName = "guest";
            rabbitMqConfig.Password = "guest";
        });

        config.AddDataLayer((dbType, config) =>
        {
            dbType = DatabaseType.MSSQL;
            config.ConnectionString = "Server=localhost,1433;Database=StoreCopy;User Id=sa;Password=MsSqlServer@2023;Encrypt=False";
            config.TransactionTimeOutInSec = 30;
        });
    });
});

var host = builder.UseConsoleLifetime().Build();

host.UseDbChangeReceiver();

host.RunAsync(cts.Token).Wait();