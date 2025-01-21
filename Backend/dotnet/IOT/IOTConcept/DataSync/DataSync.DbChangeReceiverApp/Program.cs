using Dkeshri.DataSync.DbChangeReceiver.Extenstions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using DataSync.DbChangeReceiverApp.Extensions;
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
    string dbConnectionString = hostContext.Configuration.GetDbConnectionString();
    int dbTransationTimeOut = hostContext.Configuration.GetDbTransactionTimeOutInSec();
    var rabbitMqConfiguration = hostContext.Configuration.GetRabbitMqConfiguration();
    services.AddDbChangeReceiver((config) =>
    {
        if(rabbitMqConfiguration != null)
        {
            config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
            {
                rabbitMqConfig.HostName = rabbitMqConfiguration.HostName;
                rabbitMqConfig.Port = rabbitMqConfiguration.Port;
                rabbitMqConfig.UserName = rabbitMqConfiguration.UserName;
                rabbitMqConfig.Password = rabbitMqConfiguration.Password;
                rabbitMqConfig.Queue.QueueName = rabbitMqConfiguration.Queue.Name;
            });
        }

        config.AddDataLayer((dbType, config) =>
        {
            dbType = DatabaseType.MSSQL;
            config.ConnectionString = dbConnectionString;
            config.TransactionTimeOutInSec = dbTransationTimeOut;
        });
    });
});

var host = builder.UseConsoleLifetime().Build();

host.UseDbChangeReceiver();

host.RunAsync(cts.Token).Wait();