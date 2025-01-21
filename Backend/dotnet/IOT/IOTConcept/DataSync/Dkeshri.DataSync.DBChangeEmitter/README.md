# Data-Sync-Emmiter

This application help to track the change in `MsSql` Database changes and send that changes to `RabbitMq` Message broker. On Start of this application it create a Table called **ChangeTrackers**, Which contain list of tables that will be tracked. There is hosted service which check the Database change in every **10 secs** and Send that changes to RabbiMq queue (default_queue: DataSyncQueue).

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

## How to use

This package uses the `IServiceCollection` to setup. There is an Extension `AddDataSyncDbChangeEmitter` Method is use to setup. 

You need to provide Message Broker Details (like `rabbitMq`) and `MsSql` Connection details to work this package.

**Publish Message To Queue**

If you want to publish message directly to `Queue`. Then no need to provide Exchange Name, Provide only Queue name.

```csharp
services.AddDataSyncDbChangeEmitter((config) =>
{

    config.AddDataLayer((dbType, config) =>
    {
        dbType = DatabaseType.MSSQL;
        config.ConnectionString = "Server=hostIp;Database=DatabaseName;User Id=userid;Password=YourDbPassword;Encrypt=False";
        config.TransactionTimeOutInSec = 30;
    });

    config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "rabbitMqHostIp";
        rabbitMqConfig.Port = 5672;
        rabbitMqConfig.UserName = "username";
        rabbitMqConfig.Password = "password";
        rabbitMqConfig.Queue.QueueName = "DataSyncQueue";
    });
});
```

**Publish Message to Exchange**

To Publish message to Exchange you need to set `UseExchangeToSendMessage` Property to `true`, and `ExchangeRoutingKey` as below code.



```csharp
config.MessageBroker.ExchangeRoutingKey = "YourRoutingKey";
config.MessageBroker.UseExchangeToSendMessage = true;
```

```csharp
services.AddDataSyncDbChangeEmitter((config) =>
{

    config.AddDataLayer((dbType, config) =>
    {
        dbType = DatabaseType.MSSQL;
        config.ConnectionString = "Server=hostIp;Database=DatabaseName;User Id=userid;Password=YourDbPassword;Encrypt=False";
        config.TransactionTimeOutInSec = 30;
    });

    // To Publish message on Exchange need below Properties.
    config.MessageBroker.ExchangeRoutingKey = "RouitngKey";
    config.MessageBroker.UseExchangeToSendMessage = true;

    config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "rabbitMqHostIp";
        rabbitMqConfig.Port = 5672;
        rabbitMqConfig.UserName = "username";
        rabbitMqConfig.Password = "password";
        rabbitMqConfig.Exchange.ExchangeName = "ExchangeName";
    });
});
```

**Full Example For Queue** 

Lets say we have .Net Core `Console Application`, Use below code in `Program.cs` file and run the application.

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

        config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "rabbitMqHostIp";
            rabbitMqConfig.Port = 5672;
            rabbitMqConfig.UserName = "username";
            rabbitMqConfig.Password = "password";
            rabbitMqConfig.Queue.QueueName = "YourQueueName";
        }); 
    });
});
builder.RunConsoleAsync().Wait();
```

**Full Example For Exchange**

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

        config.MessageBroker.ExchangeRoutingKey = "YourRoutingKey";
        config.MessageBroker.UseExchangeToSendMessage = true;
        config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "rabbitMqHostIp";
            rabbitMqConfig.Port = 5672;
            rabbitMqConfig.UserName = "username";
            rabbitMqConfig.Password = "password";
            rabbitMqConfig.Exchange.ExchangeName = "ExchangeName";
        }); 
    });
});
builder.RunConsoleAsync().Wait();
```





## Configuration.

After running this application, it will perform **database migration**, creating a table named `ChangeTrackers` in your database.

You need to insert your table into the `ChangeTrackers` table. This table contains two columns: `TableName` and `ChangeVersion`. You need to set initial value as `0` to `ChangeVersion` column.

use below query to insert tableName.

```sql
INSERT Into ChangeTrackers (TableName,ChangeVersion)
VALUES('YourTableName',0);
```

> Note: Make sure dependent tableName should be there in ChangeTrackers.

**Example**

let say you have two tables `orders` and `ordersSummary` tables, `Orders` table has foreign refrance of `ordersSummary` table then you have to insert both tableName (`orders` and `OrdersSummary`) in `ChangeTrackers` Table.

> After inserting tables in `ChangeTrackers` you need to restart Emitter Application, to Enable Change tracker on newly added Tables.

** If don't want to restert Emitter Application**

Run below query to manually enable

```sql
ALTER TABLE TableName
ENABLE CHANGE_TRACKING;
```
