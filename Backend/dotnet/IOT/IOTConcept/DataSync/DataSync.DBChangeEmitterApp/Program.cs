using DataSync.DBChangeEmitter.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((context, config) =>
{
    var env = context.HostingEnvironment;
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables(); // Read from environment variables
});

builder.ConfigureServices((hostContext, services) =>
{

    services.AddDataSyncDbChangeEmitter((config) =>
    {
        config.AddRabbitMqBroker((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "localhost";
            rabbitMqConfig.Port = 5672;
            rabbitMqConfig.QueueName = "DataSyncQueueApp";
            rabbitMqConfig.UserName = "guest";
            rabbitMqConfig.Password = "guest";
        });

        config.AddDataLayer((dbType,config) =>
        {
            dbType = DatabaseType.MSSQL;
            config.ConnectionString = "Server=localhost,1433;Database=Store;User Id=sa;Password=MsSqlServer@2023;Encrypt=False";
            config.TransactionTimeOutInSec = 30;
        });
    });

});


builder.RunConsoleAsync().Wait();