using Dkeshri.SystemDesign.LowLevel;
using ExecuterApp.Interfaces;
using ExecuterApp.Services;
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
            //services.UseFactoryMethod();
            services.AddSingleton<IMessageSenderService,MessageSenderService>();
        }
        public static void AddRabbitQ(this IServiceCollection services)
        {
            //services.AddRbbitMqServices();
        }
    }
}
