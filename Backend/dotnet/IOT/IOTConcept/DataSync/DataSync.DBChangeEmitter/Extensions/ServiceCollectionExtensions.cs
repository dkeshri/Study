
using DataSync.DBChangeEmitter.Interfaces;
using DataSync.DBChangeEmitter.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseChangeTrackerService, DatabaseChangeTrackerService>();
        }
    }
}
