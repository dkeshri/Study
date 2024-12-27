using DataSync.DBChangeEmitter.Interfaces;
using DataSync.DBChangeEmitter.Services;
using Microsoft.Extensions.DependencyInjection;
using MessageQueue.RabbitMq.Extensions;

namespace DataSync.DBChangeEmitter.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseChangeTrackerService, DatabaseChangeTrackerService>();
            services.AddSingleton<ISendMessageToRabbitMq, SendMessageToRabbiMq>();
        }
        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddRbbitMqServices();
        }
    }
}
