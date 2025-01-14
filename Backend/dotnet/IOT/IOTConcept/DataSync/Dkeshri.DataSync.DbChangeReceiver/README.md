# Data-Sync-Receiver

This will subscribe to Message Broker like `RabbitMq` Queue. On Message received, Apply changes to `MSSQL` Database.
This package only support `RabbitMQ`.
# Installation Steps

## Pre-requisite

Message Broker need to be running. (RabbitMq)

## How to Use
This package is using the `IServiceCollection` to setup. There is an Extension `AddDbChangeReceiver` Method is use to setup.

you need to provide Message Broker Details (like rabbitMq) and MsSql Connection details to work this package.

**Step 1**
```csharp
services.AddDbChangeReceiver((config) =>
{
    config.AddRabbitMqBroker((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "RabbitMqhostName/IP";
        rabbitMqConfig.Port = 5672;
        rabbitMqConfig.QueueName = "DataSyncQueue";
        rabbitMqConfig.UserName = "guest"; // user Name of RabbitMq
        rabbitMqConfig.Password = "guest"; // password Name of RabbitMq
    });

    config.AddDataLayer((dbType, config) =>
    {
        dbType = DatabaseType.MSSQL;
        config.ConnectionString = "Server=hostIP;Database=DatabaseName;User Id=userId;Password=DbPassword;Encrypt=False";
        config.TransactionTimeOutInSec = 30;
    });
});
```

**Step 2 :**After Service Configuration call `UseDbChangeReceiver` Extension Method of `Ihost`

```csharp
var host = builder.UseConsoleLifetime().Build();
host.UseDbChangeReceiver();
```

**Example:**

Lets say we have .Net Core `Console Application`, Use below code in `Program.cs` file and run the application.

```csharp
using Dkeshri.DataSync.DbChangeReceiver.Extenstions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddDbChangeReceiver((config) =>
    {
        config.AddRabbitMqBroker((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "RabbitMqhostName/IP";
            rabbitMqConfig.Port = 5672;
            rabbitMqConfig.QueueName = "DataSyncQueue";
            rabbitMqConfig.UserName = "guest"; // user Name of RabbitMq
            rabbitMqConfig.Password = "guest"; // password Name of RabbitMq
        });

        config.AddDataLayer((dbType, config) =>
        {
            dbType = DatabaseType.MSSQL;
            config.ConnectionString = "Server=hostIP;Database=DatabaseName;User Id=userId;Password=DbPassword;Encrypt=False";
            config.TransactionTimeOutInSec = 30;
        });
    });
});

var host = builder.UseConsoleLifetime().Build();

host.UseDbChangeReceiver();

host.RunAsync().Wait();
```