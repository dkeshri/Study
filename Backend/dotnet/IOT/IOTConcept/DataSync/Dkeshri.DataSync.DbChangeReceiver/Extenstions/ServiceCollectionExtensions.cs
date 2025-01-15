using Dkeshri.DataSync.Common.Extensions;
using Dkeshri.DataSync.DbChangeReceiver.Handlers;
using Dkeshri.DataSync.DbChangeReceiver.Interfaces;
using Dkeshri.DataSync.DbChangeReceiver.Services;
using MessageQueue.RabbitMq.Extensions;
using MessageQueue.RabbitMq.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dkeshri.DataSync.DbChangeReceiver.Extenstions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddDbChangeReceiver(this IServiceCollection services, Action<DbChangeReceiverConfig> configuration)
        {
            DbChangeReceiverConfig dbChangeReceiverConfig = new DbChangeReceiverConfig();
            configuration.Invoke(dbChangeReceiverConfig);
            services.AddDbChangeReceiver(dbChangeReceiverConfig);
        }

        public static void AddDbChangeReceiver(this IServiceCollection services, DbChangeReceiverConfig configuration)
        {
            if (configuration.DatabaseType == DatabaseType.MSSQL)
            {
                services.AddDataLayer((dbConfig) =>
                {
                    dbConfig.ConnectionString = configuration.DbConfig.ConnectionString;
                    dbConfig.TransactionTimeOutInSec = configuration.DbConfig.TransactionTimeOutInSec;
                });
            }
            if (configuration.IsRabbitMqConfigured)
            {
                services.AddRabbitMqMessageBrocker(configuration.RabbitMqConfig);
            }
            services.AddReveiverServices();
        }

        public static void AddRabbitMqBroker(this DbChangeReceiverConfig configuration, Action<RabbitMqConfig> config)
        {
            RabbitMqConfig rabbitMqConfig = new RabbitMqConfig();
            config.Invoke(rabbitMqConfig);
            configuration.RabbitMqConfig = rabbitMqConfig;
            configuration.IsRabbitMqConfigured = true;
        }
        public static void AddDataLayer(this DbChangeReceiverConfig configuration, Action<DatabaseType, DbConfig> config)
        {
            DbConfig dbConfig = new DbConfig();
            DatabaseType databaseType = new DatabaseType();
            config.Invoke(databaseType, dbConfig);
            configuration.DatabaseType = databaseType;
            configuration.DbConfig = dbConfig;
        }
        public static void UseDbChangeReceiver(this IHost host)
        {
            if (host != null)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var messageHandler = scope.ServiceProvider.GetRequiredService<IRabbitMqMessageHandler>();
                    var messageReceiver = scope.ServiceProvider.GetRequiredService<IMessageReceiver>();
                    messageReceiver.MessageHandler = messageHandler.HandleMessage;
                }
            }

        }
        private static void AddRabbitMqMessageBrocker(this IServiceCollection services, RabbitMqConfig config)
        {
            MessageQueue.RabbitMq.Extensions.RabbitMqConfig rabbitMqConfig = new()
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password,
                QueueName = config.QueueName,
                ExchangeName = config.ExchangeName,
                ClientProvidedName = "Receiver",
                RegisterReceiverServices = true
            };
            services.AddRabbitMqServices(rabbitMqConfig);
        }
        private static void AddReveiverServices(this IServiceCollection services)
        {

            services.AddMediatR(cfg => {
                // Note here we can use any class to get the current assembly.
                // here we are taking ServiceCollectionExtensions.
                cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            });
            services.AddSingleton<IDbChangeApplyService,DbChangeApplyService>();
            services.AddSingleton<IRabbitMqMessageHandler, RabbitMqMessageHandler>();
        }

    }
}
