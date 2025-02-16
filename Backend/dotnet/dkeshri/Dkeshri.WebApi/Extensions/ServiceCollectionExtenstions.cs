using Dkeshri.WebApi.Services;
using Serilog;

namespace Dkeshri.WebApi.Extensions
{
    public static class ServiceCollectionExtenstions
    {
        public static void AddLogLevelTest(this IServiceCollection services)
        {
            Log.Verbose("This is a verbose message"); // Will not be logged
            Log.Debug("This is a debug message");     // Will not be logged
            Log.Information("This is an information message"); // Will be logged
            Log.Warning("This is a warning message"); // Will be logged
            Log.Error("This is an error message");    // Will be logged
            Log.Fatal("This is a fatal message");     // Will be logged
        }

        public static void AddServices(this IServiceCollection services) 
        {
            services.AddSingleton<ISingleton,DemoService>();    
            services.AddTransient<ITransient,DemoService>();    
            services.AddScoped<IScope,DemoService>();


            services.AddTransient<LoggerService>();

        }
    }
}
