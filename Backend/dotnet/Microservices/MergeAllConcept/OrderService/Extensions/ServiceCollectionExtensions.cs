﻿using MassTransit;
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
            services.AddSingleton<InMemoryData>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            
        }

        public static void AddMassTransit(this IServiceCollection services) 
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<PaymentFailedConsumer>();
                x.AddConsumer<UpdateOrderStatusConsumer>();
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
