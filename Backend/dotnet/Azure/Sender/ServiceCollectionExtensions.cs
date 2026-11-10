using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sender
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddServiceBusClient(this IServiceCollection services,IConfiguration configuration)
        {  

            services.AddSingleton(_ =>
            {
                var serviceBusConnectionString = configuration["ServiceBusConnection"];
                if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
                {
                    throw new InvalidOperationException(
                        "Missing configuration value 'ServiceBusConnection'. Set it under Values in local.settings.json or as an app setting in Azure.");
                }
                return new ServiceBusClient(serviceBusConnectionString);
            });
        }
    }
}
