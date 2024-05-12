using MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using RabbitMQ.Client;

namespace MessageQueue.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRbbitMqConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMqConnection>();
            services.AddSingleton<IConnection>(sp => sp.GetRequiredService<RabbitMqConnection>().CreateConnection());
            services.AddScoped<ISendMessageToQueue, SendMessageToQueue>();
            services.AddHostedService<ReciveMessageService>();
        }
    }
}
