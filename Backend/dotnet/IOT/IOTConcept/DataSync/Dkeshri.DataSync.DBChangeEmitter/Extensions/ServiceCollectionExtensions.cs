using Microsoft.Extensions.DependencyInjection;
using Dkeshri.DataSync.Common.Extensions;
using Dkeshri.MessageQueue.Extensions;
using Dkeshri.DataSync.DBChangeEmitter.Interfaces;
using Dkeshri.DataSync.DBChangeEmitter.Services;

namespace Dkeshri.DataSync.DBChangeEmitter.Extensions
{
    public static class ServiceCollectionExtensions
    {
        
        public static void AddDataSyncDbChangeEmitter(this IServiceCollection services, Action<DbChangeEmitterConfig> configuration) 
        {
            DbChangeEmitterConfig dbChangeEmitterConfig = new DbChangeEmitterConfig();
            MessageBroker messageBroker = new MessageBroker(services);
            messageBroker.RegisterSenderServices = true;
            messageBroker.ClientProvidedName = "Sender";
            dbChangeEmitterConfig.MessageBroker = messageBroker;
            dbChangeEmitterConfig.MessageBroker.ExchangeRoutingKey = "EmitterToReceiver";
            configuration.Invoke(dbChangeEmitterConfig);
            services.AddDataSyncDbChangeEmitter(dbChangeEmitterConfig);
        }
        public static void AddDataSyncDbChangeEmitter(this IServiceCollection services, DbChangeEmitterConfig config)
        {
            services.AddSingleton(config);
            if (config.DatabaseType == DatabaseType.MSSQL)
            {
                services.AddDataLayer((dbConfig) =>
                {
                    dbConfig.ConnectionString = config.DbConfig.ConnectionString;
                    dbConfig.TransactionTimeOutInSec = config.DbConfig.TransactionTimeOutInSec;
                });
            }
            
            services.AddMessageBroker(config.MessageBroker);
            services.AddDbChangeEmitterServices();
        }

        public static void AddDataLayer(this DbChangeEmitterConfig configuration, Action<DatabaseType, DbConfig> config)
        {
            DbConfig dbConfig = new DbConfig();
            DatabaseType databaseType = new DatabaseType();
            config.Invoke(databaseType, dbConfig);
            configuration.DatabaseType = databaseType;
            configuration.DbConfig = dbConfig;
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
