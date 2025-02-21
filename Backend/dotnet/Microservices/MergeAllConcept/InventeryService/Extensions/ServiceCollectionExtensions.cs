using InventeryService.Consumers;
using InventeryService.Data;
using InventeryService.Data.Interfaces.Repositories;
using InventeryService.Data.Repositories;
using MassTransit;

namespace InventeryService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryData>();
            services.AddSingleton<IInventoryRepository, InventoryRepository>();
        }

        public static void AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<InventoryConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
