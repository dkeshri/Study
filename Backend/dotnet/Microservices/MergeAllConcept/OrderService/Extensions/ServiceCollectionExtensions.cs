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
    }
}
