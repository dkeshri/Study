# About

This library creates a queue that binds to the exchange created by `Dkeshri.DataSync.DBChangeEmitter`. 
When a message is received in the queue from the exchange, it applies the changes to an `MSSQL` database.

* When configuring the queue using the `UseQueue` extension method, ensure that the same exchange name and routing key specified in the `DBChangeEmitter` application are provided.

This library complements [Dkeshri.DataSync.DBChangeEmitter](https://www.nuget.org/packages/Dkeshri.DataSync.DBChangeEmitter), which sends messages to a RabbitMQ exchange. 
The exchange then routes these messages to the queues bound to it using routing keys.


We also provide a Docker image [dkeshri/data-sync-emitter](https://hub.docker.com/r/dkeshri/data-sync-emitter) 
and [dkeshri/data-sync-receiver](https://hub.docker.com/r/dkeshri/data-sync-receiver) that implements this library. 
You only need to supply the necessary details through environment variables.

# Installation Steps

## Pre-requisite

> Message Broker need to be running. (RabbitMq)

you can use below docker command to setup rabbitMq

```bash
docker run -d -v rabbitmqv:/var/log/rabbitmq --hostname rmq --name RabbitMqServer \
-p 5672:5672 -p 8080:15672 rabbitmq:3.13-management
```
Port 8080 is for management portal and access by below mention Login credentials.

Click on the link for <a href='http://localhost:8080/'>Admin Portal</a>

Port `5672` is use in communication during producing and consuming of message.

**Login crediential**

Default login crediential if we not specifiy during creation of docker container

Username: `guest` and Password: `guest`

## How to Use

This package uses the `IServiceCollection` for setup. An extension method, `AddDbChangeReceiver`, is provided to configure the package.

To use this package, you need to supply the connection details for both the message broker (e.g., RabbitMQ) and the MSSQL database.

* To configure the database, the library offers the `AddDataLayer` method. 
* For message broker configuration, you need to include the [Dkeshri.MessageQueue.RabbitMq](https://www.nuget.org/packages/Dkeshri.MessageQueue.RabbitMq) package and then call AddRabbitMqServices on the config.MessageBroker property.
* To configure `Queue` properties, call the `UseQueue` extension method on the `RabbitMqConfig` object returned by the `AddRabbitMqServices` method.

**Receive Message from Queue**

**Step 1**

```csharp
services.AddDbChangeReceiver((config) =>
{
    config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "rabbitMqHostIp";
        rabbitMqConfig.Port = 5672; 
        rabbitMqConfig.UserName = "userName";
        rabbitMqConfig.Password = "password";
    }).UseQueue(queue =>
    {
        queue.QueueName = "QueueName";
        queue.ExchangeName = "ExchangeName";
        queue.RoutingKeys = ["RoutingKey1"];
        queue.IsDurable = true;
    });

    config.AddDataLayer((dbType, config) =>
    {
        dbType = DatabaseType.MSSQL;
        config.ConnectionString = "Server=hostIp;Database=DatabaseName;User Id=userid;Password=YourDbPassword;Encrypt=False";
        config.TransactionTimeOutInSec = 30;
    });
});
```

**Step 2** : After Service Configuration call `UseDbChangeReceiver` Extension Method of `Ihost`

```csharp
var host = builder.UseConsoleLifetime().Build();
host.UseDbChangeReceiver();
```

**Full Example**

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
        }).UseQueue(queue =>
        {
            queue.QueueName = "QueueName";
            queue.ExchangeName = "ExchangeName";
            queue.RoutingKeys = ["RoutingKey1"];
            queue.IsDurable = true;
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