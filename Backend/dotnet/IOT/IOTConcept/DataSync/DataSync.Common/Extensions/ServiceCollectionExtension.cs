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
        public static void AddDataLayer(this IServiceCollection services)
        {
            services.AddSingleton<IDataContext, DataSyncDbContext>();
            LoadRepositories(services);
        }

        private static void LoadRepositories(IServiceCollection services)
        {
            services.AddSingleton<IChangeTrackerRepository,ChangeTrackerRepository>();
            services.AddSingleton<IApplyDbChangeRepository,ApplyDbChangeRepository>();
        }
    }
}
