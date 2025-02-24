using Contract.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.Extensions
{
    public static class MigrationManager
    {
        public static bool MigrateDatabase(this IHost host)
        {
            bool isMigrationSuccess = false;
            using (var scope = host.Services.CreateScope())
            {
                using (var DataContext = scope.ServiceProvider.GetRequiredService<IDataContext>())
                {
                    try
                    {

                        if (DataContext.DbContext.Database.GetPendingMigrations().Any())
                        {
                            Console.WriteLine("DB migration started....");
                            DataContext.DbContext.Database.Migrate();

                            Console.WriteLine("DB migration successfully completed.");
                        }
                        else
                        {
                            Console.WriteLine("DB migration: Not needed. Already up to date...");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return isMigrationSuccess;
        }
    }
}
