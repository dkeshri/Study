using MassTransit;
using Microsoft.Extensions.Configuration;
using PaymentService.Data;
using PaymentService.Data.Interfaces;
using PaymentService.Data.Repositories;

namespace PaymentService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryData>();
            services.AddSingleton<IPaymentRepository, PaymentRepository>();
        }
        public static void AddMassTransit(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
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
