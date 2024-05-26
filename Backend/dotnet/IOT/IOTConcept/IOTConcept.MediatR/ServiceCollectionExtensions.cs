using Microsoft.Extensions.DependencyInjection;

namespace IOTConcept.MediatR
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                // Note here we can use any class to get the current assembly.
                // here we are taking ServiceCollectionExtensions.
                cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
             });
        }
    }
}
