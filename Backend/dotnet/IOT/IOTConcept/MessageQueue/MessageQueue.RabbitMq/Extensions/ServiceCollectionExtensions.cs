using MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRbbitMqServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<ISendMessage, SendMessage>();
            services.AddHostedService<ReceiveMessageFromQueueService>();
            services.AddHostedService<ReceiveMessageFromExchangeService>();
        }
    }
}
