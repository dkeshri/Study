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
            services.AddSingleton<ISendMessage, SendMessage>();
            
        }

        public static void AddRbbitMqMessageReceiverServiceForQueue(this IServiceCollection services) 
        {
            services.AddHostedService<ReceiveMessageFromQueueService>();
        }
        public static void AddRbbitMqMessageReceiverServiceForExchange(this IServiceCollection services)
        {
            services.AddHostedService<ReceiveMessageFromExchangeService>();
        }
    }
}
