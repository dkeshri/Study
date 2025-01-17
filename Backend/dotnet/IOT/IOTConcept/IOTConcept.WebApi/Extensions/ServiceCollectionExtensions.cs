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
                //services.AddRbbitMqServices(); // Please comment this if you don't run/start rabbit MQ Service otherwise it will throw exception.
            }
            catch (Exception)
            {

            }
            
        }

        public static void AddInfluxDb(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddInfluxDbClient(configuration);
        }

        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddMediatRHandlers();
        }
    }
}
