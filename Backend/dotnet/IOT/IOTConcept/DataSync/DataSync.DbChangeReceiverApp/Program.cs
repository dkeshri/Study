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
    string rabbitMqHost = hostContext.Configuration.GetRabbitMqHostName();
    int rabbitMqPort = hostContext.Configuration.GetRabbitMqHostPort();
    string rabbitMqUsername = hostContext.Configuration.GetRabbitMqUserName();
    string rabbitMqPassword = hostContext.Configuration.GetRabbitMqPassword();
    string rabbitMqQueueName = hostContext.Configuration.GetRabbitMqQueueName();
    string rabbitMqExchangeName = hostContext.Configuration.GetRabbitMqExchangeName();
    string rabbitMqClientproviderName = hostContext.Configuration.GetRabbitMqClientProvidedName();
    int dbTransationTimeOut = hostContext.Configuration.GetDbTransactionTimeOutInSec();
    services.AddDbChangeReceiver((config) =>
    {
        config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = rabbitMqHost;
            rabbitMqConfig.Port = rabbitMqPort;
            rabbitMqConfig.QueueName = rabbitMqQueueName;
            rabbitMqConfig.UserName = rabbitMqUsername;
            rabbitMqConfig.Password = rabbitMqPassword;
        });

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