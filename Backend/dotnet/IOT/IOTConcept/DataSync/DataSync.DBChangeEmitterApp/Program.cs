﻿using Dkeshri.DataSync.DBChangeEmitter.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using DataSync.DBChangeEmitterApp.Extensions;

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

    string dbConnectionString = hostContext.Configuration.GetDbConnectionString();
    int dbTransationTimeOut = hostContext.Configuration.GetDbTransactionTimeOutInSec();
    var rabbitMqConfiguration = hostContext.Configuration.GetRabbitMqConfiguration();

    services.AddDataSyncDbChangeEmitter((config) =>
    {
        
        config.AddDataLayer((dbType, config) =>
        {
            dbType = DatabaseType.MSSQL;
            config.ConnectionString = dbConnectionString;
            config.TransactionTimeOutInSec = dbTransationTimeOut;
        });
        
        if(rabbitMqConfiguration != null)
        {
            config.MessageBroker.ClientProvidedName = rabbitMqConfiguration.ClientProvidedName;
            config.MessageBroker.ExchangeRoutingKey = rabbitMqConfiguration.Exchange.RoutingKey;
            config.MessageBroker.AddRabbitMqServices((rabbitMqConfig) =>
            {
                rabbitMqConfig.HostName = rabbitMqConfiguration.HostName;
                rabbitMqConfig.Port = rabbitMqConfiguration.Port;
                rabbitMqConfig.UserName = rabbitMqConfiguration.UserName;
                rabbitMqConfig.Password = rabbitMqConfiguration.Password;
                rabbitMqConfig.Exchange.ExchangeName = rabbitMqConfiguration.Exchange.Name;
                rabbitMqConfig.Exchange.IsDurable = rabbitMqConfiguration.Exchange.IsDurable;
                rabbitMqConfig.Exchange.AutoDelete = rabbitMqConfiguration.Exchange.IsAutoDelete;
            });
        }
        
    });

});


builder.RunConsoleAsync().Wait();