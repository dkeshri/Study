using IOTConcept.Influxdb.Interfaces;
using IOTConcept.Influxdb.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace IOTConcept.Influxdb
{
    public static class ServiceCollectionExtensions
    {
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
