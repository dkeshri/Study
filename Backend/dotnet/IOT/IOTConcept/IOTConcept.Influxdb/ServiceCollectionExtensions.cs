using IOTConcept.Influxdb.Interfaces;
using IOTConcept.Influxdb.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace IOTConcept.Influxdb
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfluxDbClient(this IServiceCollection services, IConfiguration configuration)
        {
            // this option can be provided in any class that has been configued in IServiceCollection. 
            services.AddOptions<InfluxDbConfigurationOptions>()
                .Bind(configuration.GetSection(InfluxDbConfigurationOptions.InfluxDb)); 

            services.AddSingleton<IInfluxDbMessageProcessor>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<InfluxDbConfigurationOptions>>();
                IInfluxDbClientFactory influxDbClientFactory = new InfluxDbClientFactory(options);
                return new InfluxDbMessageProcessor(influxDbClientFactory);
            });
        }
    }
}
