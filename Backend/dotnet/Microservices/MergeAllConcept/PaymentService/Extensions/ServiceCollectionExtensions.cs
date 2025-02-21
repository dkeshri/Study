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
    }
}
