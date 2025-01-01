using MessageQueue.RabbitMq.Handlers;
using MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using MessageQueue.RabbitMq.Services;
using Microsoft.Extensions.DependencyInjection;


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
            services.AddSingleton<IMessageReceiver,MessageReceiverHandler>();
            services.AddHostedService<MessageReceiverFromQueueService>();
        }
        public static void AddRbbitMqMessageReceiverServiceForExchange(this IServiceCollection services)
        {
            services.AddHostedService<ReceiveMessageFromExchangeService>();
        }
    }
}
