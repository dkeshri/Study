using DataSync.DbChangeReceiver.Handlers;
using DataSync.DbChangeReceiver.Interfaces;
using DataSync.DbChangeReceiver.Services;
using MessageQueue.RabbitMq.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Extenstions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRbbitMqServices(configuration);
            services.AddRbbitMqMessageReceiverServiceForQueue();
            services.AddMediatR(cfg => {
                // Note here we can use any class to get the current assembly.
                // here we are taking ServiceCollectionExtensions.
                cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            });
            services.AddSingleton<IDbChangeApplyService,DbChangeApplyService>();
        }
        public static void AddHandlers(this IServiceCollection services)
        { 
            services.AddSingleton<IRabbitMqMessageHandler,RabbitMqMessageHandler>();
        }
    }
}
