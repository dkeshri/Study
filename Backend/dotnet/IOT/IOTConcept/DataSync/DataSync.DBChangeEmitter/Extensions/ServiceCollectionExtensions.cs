using DataSync.DBChangeEmitter.Interfaces;
using DataSync.DBChangeEmitter.Services;
using Microsoft.Extensions.DependencyInjection;
using MessageQueue.RabbitMq.Extensions;
using Microsoft.Extensions.Configuration;

namespace DataSync.DBChangeEmitter.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseChangeTrackerService, DatabaseChangeTrackerService>();
            services.AddSingleton<ISendMessageToRabbitMq, SendMessageToRabbiMq>();
            services.AddSingleton<ITopologicalSorterService, TopologicalSorterService>();
        }
        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRbbitMqServices(configuration);
        }
    }
}
