using Dkeshri.DataSync.Common.Extensions;
using Dkeshri.DataSync.DbChangeReceiver.Handlers;
using Dkeshri.DataSync.DbChangeReceiver.Interfaces;
using Dkeshri.DataSync.DbChangeReceiver.Services;
using Dkeshri.MessageQueue.Extensions;
using Dkeshri.MessageQueue.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dkeshri.DataSync.DbChangeReceiver.Extenstions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddDbChangeReceiver(this IServiceCollection services, Action<DbChangeReceiverConfig> configuration)
        {
            DbChangeReceiverConfig dbChangeReceiverConfig = new DbChangeReceiverConfig();
            MessageBroker messageBroker = new MessageBroker(services);
            messageBroker.RegisterReceiverServices = true;
            messageBroker.ClientProvidedName = "Receiver";
            dbChangeReceiverConfig.MessageBroker = messageBroker;
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
            services.AddMessageBroker(configuration.MessageBroker);
            services.AddReveiverServices();
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
                    var messagrBrokerStartup = scope.ServiceProvider.GetRequiredService<IStartup>();
                    messagrBrokerStartup.OnStart();
                    messageReceiver.MessageHandler = messageHandler.HandleMessage;

                }
            }

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
