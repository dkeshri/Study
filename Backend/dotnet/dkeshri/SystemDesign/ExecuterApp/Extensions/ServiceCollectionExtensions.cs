using Dkeshri.SystemDesign.LowLevel;
using MessageQueue.RabbitMq.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuterApp.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services) 
        {
            services.UseFactoryMethod();
        }
        public static void AddRabbitQ(this IServiceCollection services)
        {
            services.AddRbbitMqServices();
        }
    }
}
