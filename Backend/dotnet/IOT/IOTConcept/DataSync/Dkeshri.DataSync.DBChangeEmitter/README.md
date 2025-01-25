# About

This library helps track changes in an `MSSQL` database and sends those changes to a message broker, such as RabbitMQ.
* When the application starts, it creates a table called `ChangeTrackers`, which contains a list of tables to be tracked.
* A hosted service runs every `10 seconds` to check for changes in the tables listed in `ChangeTrackers` and sends those changes to a Message Broker like `RabbitMq`.
* Before processing changes in the tracked tables, the tables are sorted using `topological sorting`. 
This ensures that changes to dependent tables are handled in the correct order, preventing foreign key conflicts during insert operations.

This library serves as a companion to [Dkeshri.DataSync.DbChangeReceiver](https://www.nuget.org/packages/Dkeshri.DataSync.DbChangeReceiver), which reads the messages sent to the RabbitMQ queue by this library and applies the changes to the MSSQL database.

We also provide a Docker image [dkeshri/data-sync-emitter](https://hub.docker.com/r/dkeshri/data-sync-emitter) 
and [dkeshri/data-sync-receiver](https://hub.docker.com/r/dkeshri/data-sync-receiver) that implements this library. 
You only need to supply the necessary details through environment variables.


> In the future, the library will support multiple message brokers and databases. However, currently, it supports only RabbitMQ as the message broker and MSSQL as the database.

# Installation Steps

## Pre-requisite

### Enable Change tracking on Database

> If not enabled please run below command.

```sql
ALTER DATABASE YourDatabaseName
SET CHANGE_TRACKING = ON 
(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);
```

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

## How to use

This package uses the `IServiceCollection` for setup. An extension method, `AddDataSyncDbChangeEmitter`, is provided to configure the package.

To use this package, you need to supply the connection details for both the message broker (e.g., RabbitMQ) and the MSSQL database.

* To configure the database, the library offers the `AddDataLayer` method. For message broker configuration, you need to include the [Dkeshri.MessageQueue.RabbitMq](https://www.nuget.org/packages/Dkeshri.MessageQueue.RabbitMq) package and then call AddRabbitMqServices on the config.MessageBroker property.


```csharp
services.AddDataSyncDbChangeEmitter((config) =>
{

    config.AddDataLayer((dbType, config) =>
    {
        dbType = DatabaseType.MSSQL;
        config.ConnectionString = "Server=hostIp;Database=DatabaseName;User Id=userid;Password=YourDbPassword;Encrypt=False";
        config.TransactionTimeOutInSec = 30;
    });

    config.MessageRoutingKey = "RouitngKey"; // This is required. MessageRoutingKey is any string value
    config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "rabbitMqHostIp";
        rabbitMqConfig.Port = 5672;
        rabbitMqConfig.UserName = "username";
        rabbitMqConfig.Password = "password";
    }).UseExchange(exchange =>
    {
        exchange.ExchangeName = "ExchangeName";
        exchange.IsDurable = true; // this is required for durable exchange
    });
});
```
**Full Example**

```csharp
using Dkeshri.DataSync.DBChangeEmitter.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using DataSync.DBChangeEmitterApp.Extensions;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{

    services.AddDataSyncDbChangeEmitter((config) =>
    {
        
        config.AddDataLayer((dbType, config) =>
        {
            dbType = DatabaseType.MSSQL;
            config.ConnectionString = dbConnectionString;
            config.TransactionTimeOutInSec = dbTransationTimeOut;
        });

        config.MessageRoutingKey = "RouitngKey"; // This is required. MessageRoutingKey is any string value
        config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "rabbitMqHostIp";
            rabbitMqConfig.Port = 5672;
            rabbitMqConfig.UserName = "username";
            rabbitMqConfig.Password = "password";
        }).UseExchange(exchange =>
        {
            exchange.ExchangeName = "ExchangeName";
            exchange.IsDurable = true;
        });
    });
});
builder.RunConsoleAsync().Wait();
```

## Configuration.

When this application runs, it performs a database migration and creates a table called `ChangeTrackers` in your database.

* To start tracking changes, you need to manually insert the names of the tables you want to track into the ChangeTrackers table.

* The `ChangeTrackers` table contains two columns:
    a. `TableName`: Specifies the name of the table to be tracked.
    b. `ChangeVersion`: Tracks the version of changes. You must set the initial value of the ChangeVersion column to `0`.

Use below query to insert trackable tables.
```sql
INSERT Into ChangeTrackers (TableName,ChangeVersion)
VALUES('YourTableName',0);
```
**Note**: Ensure that dependent tables are also included in the ChangeTrackers table.

**Example**

If you have two tables, `Orders` and `OrdersSummary`, where the `Orders` table has a foreign key reference to the `OrdersSummary` table, 
you must insert both table names (`Orders` and `OrdersSummary`) into the `ChangeTrackers` table.

> After adding tables to the `ChangeTrackers` table, 
you need to restart the Emitter application to enable change tracking for the newly added tables.

**If you don't want to restart the Emitter application**, 
run the following query to manually enable change tracking

```sql
ALTER TABLE TableName
ENABLE CHANGE_TRACKING;
```
