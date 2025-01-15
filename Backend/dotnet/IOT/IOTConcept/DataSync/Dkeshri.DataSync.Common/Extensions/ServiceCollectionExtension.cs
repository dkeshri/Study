using Dkeshri.DataSync.Common.Data.DataContext;
using Dkeshri.DataSync.Common.Interfaces.DataContext;
using Dkeshri.DataSync.Common.Interfaces.Repositories;
using Dkeshri.DataSync.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dkeshri.DataSync.Common.Extensions
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
            services.AddSingleton<DbConfig>(config);
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
