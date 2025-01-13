using DataSync.Common.Data.DataContext;
using DataSync.Common.Interfaces.DataContext;
using DataSync.Common.Interfaces.Repositories;
using DataSync.Common.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataLayer(this IServiceCollection services, Action<DbConfig> action)
        {
            DbConfig dbConfig = new DbConfig();
            action.Invoke(dbConfig);
            services.AddDataLayer(dbConfig);
        }

        public static void AddDataLayer(this IServiceCollection services, DbConfig config)
        {
            services.AddSingleton<IDataContext>(sp =>
            {
                return new DataSyncDbContext(config);
            });
            LoadRepositories(services);
        }
        private static void LoadRepositories(IServiceCollection services)
        {
            services.AddSingleton<IChangeTrackerRepository,ChangeTrackerRepository>();
            services.AddSingleton<IApplyDbChangeRepository,ApplyDbChangeRepository>();
        }
    }
}
