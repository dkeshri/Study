using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestrator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestrator.Extensions
{
    internal static class HostExtensions
    {
        public static bool MigrateDatabase(this IHost host)
        {
            bool isMigrationSuccess = false;
            using (var scope = host.Services.CreateScope())
            {
                using (var DataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    try
                    {

                        if (DataContext.Database.GetPendingMigrations().Any())
                        {
                            Console.WriteLine("DB migration started....");
                            DataContext.Database.Migrate();
                            isMigrationSuccess = true;
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
