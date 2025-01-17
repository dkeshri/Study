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

## How to use

This package uses the `IServiceCollection` to setup. There is an Extension `AddDataSyncDbChangeEmitter` Method is use to setup. 

You need to provide Message Broker Details (like `rabbitMq`) and `MsSql` Connection details to work this package.

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
        rabbitMqConfig.QueueName = "QueueName";
        rabbitMqConfig.UserName = "username";
        rabbitMqConfig.Password = "password";
    });
});
```
**Example:** 

Lets say we have .Net Core `Console Application`, Use below code in `Program.cs` file and run the application.

```csharp
using Dkeshri.DataSync.DBChangeEmitter.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{

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
            rabbitMqConfig.QueueName = "QueueName";
            rabbitMqConfig.UserName = "username";
            rabbitMqConfig.Password = "password";
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

**Example :**

let say you have two tables `orders` and `ordersSummary` tables, `Orders` table has foreign refrance of `ordersSummary` table then you have to insert both tableName (`orders` and `OrdersSummary`) in `ChangeTrackers` Table.