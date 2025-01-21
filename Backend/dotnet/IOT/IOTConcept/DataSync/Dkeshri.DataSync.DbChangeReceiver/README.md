# Data-Sync-Receiver

This will subscribe to Message Broker like `RabbitMq` Queue. On Message received, Apply changes to `MSSQL` Database.
This package only support `RabbitMQ`.
# Installation Steps

## Pre-requisite

Message Broker need to be running. (RabbitMq)

## How to Use
This package is using the `IServiceCollection` to setup. There is an Extension `AddDbChangeReceiver` Method is use to setup.

you need to provide Message Broker Details (like rabbitMq) and MsSql Connection details to work this package.

**Receive Message from Queue**

If you want to receive message that are send directly to queue then you need to set only Queue Propperties of RabbitMq Config.




**Step 1**

**For Queue**

```csharp
services.AddDbChangeReceiver((config) =>
{
    config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "rabbitMqHostIp";
        rabbitMqConfig.Port = 5672; 
        rabbitMqConfig.UserName = "userName";
        rabbitMqConfig.Password = "password";
        rabbitMqConfig.Queue.QueueName = "QueueName";
    });

    config.AddDataLayer((dbType, config) =>
    {
        dbType = DatabaseType.MSSQL;
        config.ConnectionString = "Server=hostIp;Database=DatabaseName;User Id=userid;Password=YourDbPassword;Encrypt=False";
        config.TransactionTimeOutInSec = 30;
    });
});
```

**For Exchange**

```csharp
services.AddDbChangeReceiver((config) =>
{
    config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "rabbitMqHostIp";
        rabbitMqConfig.Port = 5672; 
        rabbitMqConfig.UserName = "userName";
        rabbitMqConfig.Password = "password";
        rabbitMqConfig.Queue.QueueName = "QueueName";
        rabbitMqConfig.Queue.ExchangeName = "ExchangeName";
        rabbitMqConfig.Queue.RoutingKeys = ["RoutingKey1"];
    });

    config.AddDataLayer((dbType, config) =>
    {
        dbType = DatabaseType.MSSQL;
        config.ConnectionString = "Server=hostIp;Database=DatabaseName;User Id=userid;Password=YourDbPassword;Encrypt=False";
        config.TransactionTimeOutInSec = 30;
    });
});
```

**Step 2 :**After Service Configuration call `UseDbChangeReceiver` Extension Method of `Ihost`

```csharp
var host = builder.UseConsoleLifetime().Build();
host.UseDbChangeReceiver();
```

**Example for Queue**

Lets say we have .Net Core `Console Application`, Use below code in `Program.cs` file and run the application.

```csharp
using Dkeshri.DataSync.DbChangeReceiver.Extenstions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Dkeshri.MessageQueue.RabbitMq.Extensions;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddDbChangeReceiver((config) =>
    {
        config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "rabbitMqHostIp";
            rabbitMqConfig.Port = 5672; 
            rabbitMqConfig.UserName = "userName";
            rabbitMqConfig.Password = "password";
            rabbitMqConfig.Queue.QueueName = "QueueName";
        });

        config.AddDataLayer((dbType, config) =>
        {
            dbType = DatabaseType.MSSQL;
            config.ConnectionString = "Server=hostIp;Database=DatabaseName;User Id=userid;Password=YourDbPassword;Encrypt=False";
            config.TransactionTimeOutInSec = 30;
        });
    });
});

var host = builder.UseConsoleLifetime().Build();

host.UseDbChangeReceiver();

host.RunAsync().Wait();
```