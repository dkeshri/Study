using DataSync.Common.Interfaces.DataContext;
using DataSync.DBChangeEmitter.Data;
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
        }
    }
}
