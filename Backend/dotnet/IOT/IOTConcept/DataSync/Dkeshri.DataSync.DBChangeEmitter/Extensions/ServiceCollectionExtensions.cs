using Microsoft.Extensions.DependencyInjection;
using MessageQueue.RabbitMq.Extensions;
using Dkeshri.DataSync.Common.Extensions;
using Dkeshri.DataSync.DBChangeEmitter.Interfaces;
using Dkeshri.DataSync.DBChangeEmitter.Services;

namespace Dkeshri.DataSync.DBChangeEmitter.Extensions
{
    public static class ServiceCollectionExtensions
    {
        
        public static void AddDataSyncDbChangeEmitter(this IServiceCollection services, Action<DbChangeEmitterConfig> configuration) 
        {
            DbChangeEmitterConfig dbChangeEmitterConfig = new DbChangeEmitterConfig();
            configuration.Invoke(dbChangeEmitterConfig);
            services.AddDataSyncDbChangeEmitter(dbChangeEmitterConfig);
        }
        public static void AddDataSyncDbChangeEmitter(this IServiceCollection services, DbChangeEmitterConfig config)
        {
            if (config.DatabaseType == DatabaseType.MSSQL)
            {
                services.AddDataLayer((dbConfig) =>
                {
                    dbConfig.ConnectionString = config.DbConfig.ConnectionString;
                    dbConfig.TransactionTimeOutInSec = config.DbConfig.TransactionTimeOutInSec;
                });
            }
            services.AddDbChangeEmitterServices();

            if (config.IsRabbitMqConfigured)
            {
                services.AddRabbitMqMessageBrocker(config.RabbitMqConfig);
            }
        }
        public static void AddRabbitMqBroker(this DbChangeEmitterConfig configuration, Action<RabbitMqConfig> config)
        {
            RabbitMqConfig rabbitMqConfig = new RabbitMqConfig();
            config.Invoke(rabbitMqConfig);
            configuration.RabbitMqConfig = rabbitMqConfig;
            configuration.IsRabbitMqConfigured = true;
        }

        public static void AddDataLayer(this DbChangeEmitterConfig configuration, Action<DatabaseType, DbConfig> config)
        {
            DbConfig dbConfig = new DbConfig();
            DatabaseType databaseType = new DatabaseType();
            config.Invoke(databaseType, dbConfig);
            configuration.DatabaseType = databaseType;
            configuration.DbConfig = dbConfig;
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
                ClientProvidedName = "Sender",
                RegisterSenderServices = true
            };
            services.AddRabbitMqServices(rabbitMqConfig);
        }
        private static void AddDbChangeEmitterServices(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseChangeTrackerService, DatabaseChangeTrackerService>();
            services.AddSingleton<ISendMessageToRabbitMq, SendMessageToRabbiMq>();
            services.AddSingleton<ITopologicalSorterService, TopologicalSorterService>();
            services.AddHostedService<DbChangeEmitterService>();
        }
    }
}
