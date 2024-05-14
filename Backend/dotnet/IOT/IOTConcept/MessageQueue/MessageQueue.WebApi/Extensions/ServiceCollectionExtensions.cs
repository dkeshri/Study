using MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using RabbitMQ.Client;

namespace MessageQueue.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRbbitMqConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqConnection,RabbitMqConnection>();
            services.AddScoped<ISendMessage, SendMessage>();
            services.AddHostedService<ReceiveMessageFromQueueService>();
            services.AddHostedService<ReceiveMessageFromExchangeService>();
        }
    }
}
