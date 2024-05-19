using InfluxDB.Client;
using IOTConcept.Influxdb.Interfaces;
using IOTConcept.Influxdb.Logic;
using MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using RabbitMQ.Client;

namespace IOTConcept.WebApi.Extensions
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

        public static void AddInfluxDbClient(this IServiceCollection services)
        {
            services.AddSingleton<IInfluxDbMessageProcessor>(sp =>
            {
                IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
                IInfluxDbClientFactory influxDbClientFactory = new InfluxDbClientFactory(configuration);
                return new InfluxDbMessageProcessor(influxDbClientFactory);
            });
        }
    }
}
