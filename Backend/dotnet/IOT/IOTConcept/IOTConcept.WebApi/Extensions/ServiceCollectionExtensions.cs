using MessageQueue.RabbitMq.Extensions;
using IOTConcept.Influxdb;
using IOTConcept.MediatR;
namespace IOTConcept.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRbbitMq(this IServiceCollection services)
        {
            try
            {
                services.AddRbbitMqServices();
            }catch (Exception)
            {

            }
            
        }

        public static void AddInfluxDb(this IServiceCollection services)
        {
            services.AddInfluxDbClient();
        }

        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddMediatRHandlers();
        }
    }
}
