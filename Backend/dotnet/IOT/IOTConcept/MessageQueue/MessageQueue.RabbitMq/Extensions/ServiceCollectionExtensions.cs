using MessageQueue.RabbitMq.Handlers;
using MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using MessageQueue.RabbitMq.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace MessageQueue.RabbitMq.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRbbitMqServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMqConnection>(sp =>
            {
                return new RabbitMqConnection(configuration);
            });

            AddSenderService(services);
        }

        public static void AddRbbitMqServices(this IServiceCollection services, RabbitMqConfig rabbitMqConfig)
        {
            services.AddSingleton<IRabbitMqConnection>(sp =>
            {
                return new RabbitMqConnection(rabbitMqConfig);
            });

            AddSenderService(services);
        }

        private static void AddSenderService(IServiceCollection services)
        {
            services.AddSingleton<ISendMessage, SendMessage>();
        }

        public static void AddRbbitMqMessageReceiverServiceForQueue(this IServiceCollection services) 
        {
            services.AddSingleton<IMessageReceiver,MessageReceiverHandler>();
            services.AddHostedService<MessageReceiverFromQueueService>();
        }
        public static void AddRbbitMqMessageReceiverServiceForExchange(this IServiceCollection services)
        {
            services.AddHostedService<ReceiveMessageFromExchangeService>();
        }
    }
}
