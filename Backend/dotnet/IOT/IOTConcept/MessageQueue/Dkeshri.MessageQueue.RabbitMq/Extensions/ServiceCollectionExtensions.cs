using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Handlers;
using Dkeshri.MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using MessageQueue.RabbitMq.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Dkeshri.MessageQueue.RabbitMq.Extensions
{
    public static class ServiceCollectionExtensions
    {      
        public static void AddRabbitMqServices(this IServiceCollection services, Action<RabbitMqConfig> configuration)
        {
            RabbitMqConfig rabbitMqConfig = new RabbitMqConfig();
            configuration.Invoke(rabbitMqConfig);
            services.AddRabbitMqServices(rabbitMqConfig);
        }

        public static void AddRabbitMqServices(this IServiceCollection services, RabbitMqConfig rabbitMqConfig)
        {
            services.AddSingleton<IRabbitMqConnection>(sp =>
            {
                return new RabbitMqConnection(rabbitMqConfig);
            });

            rabbitMqConfig.AddRequestedServices(services);
        }

        private static void AddRequestedServices(this RabbitMqConfig config, IServiceCollection services)
        {
            if (config.RegisterSenderServices)
            {
                services.AddSenderService();
            }

            if (config.RegisterReceiverServices) 
            { 
                services.AddRbbitMqMessageReceiverServiceForQueue();
            }
        }
        private static void AddSenderService(this IServiceCollection services)
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
