using Contract.Data.Context;
using InventeryService.Consumers;
using InventeryService.Data;
using InventeryService.Data.Interfaces.Repositories;
using InventeryService.Data.Repositories;
using InventeryService.Middleware;
using MassTransit;

namespace InventeryService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDataContext, InventoryDbContext>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<PrometheusMetricsMiddleware>();
        }

        public static void AddMassTransit(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<InventoryConsumer>();
                var rabbitMqConfiguration = configuration.GetRabbitMqConfiguration();
                if (rabbitMqConfiguration != null)
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(rabbitMqConfiguration.HostName, "/", h =>
                        {
                            h.Username(rabbitMqConfiguration.UserName);
                            h.Password(rabbitMqConfiguration.Password);
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                }
            });
        }
    }
}
