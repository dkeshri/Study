using InventeryService.Data;
using InventeryService.Data.Interfaces.Repositories;
using InventeryService.Data.Repositories;

namespace InventeryService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryData>();
            services.AddSingleton<IInventoryRepository, InventoryRepository>();
        }
    }
}
