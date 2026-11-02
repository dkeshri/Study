using Microsoft.EntityFrameworkCore;
using MininalApi.Data;

namespace MininalApi.Extensions
{
    public static class ServiceColllectionExtensions
    {
        public static void AddDatabase(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
            var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Database connection string is not configured.");
            serviceCollection.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
            });
        }
    }
}
