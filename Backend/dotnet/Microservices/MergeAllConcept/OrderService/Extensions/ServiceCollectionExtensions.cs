using Contract.Data.Context;
using MassTransit;
using OrderService.Consumers;
using OrderService.Data;
using OrderService.Data.Interfaces.Repositories;
using OrderService.Data.Repositories;

namespace OrderService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDataContext,OrderDbContext>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            
        }

        public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<PaymentFailedConsumer>();
                x.AddConsumer<UpdateOrderStatusConsumer>();
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
