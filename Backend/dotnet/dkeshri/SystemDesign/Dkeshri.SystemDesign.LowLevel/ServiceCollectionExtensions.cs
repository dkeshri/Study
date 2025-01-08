using Dkeshri.SystemDesign.LowLevel.DesignPatterns.Creational.FactoryMethod;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel
{
    public static class ServiceCollectionExtensions
    {
        public static void UseFactoryMethod(this IServiceCollection services)
        {
            services.AddScoped<BakeryFactory, CakeFactory>();
        }
    }
}
