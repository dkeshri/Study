using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dkeshri.AzureFunctions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults((context, builder) =>
                {
                    builder.Services.AddScoped<DemoDependency>();
                    builder.Services.AddApplicationInsightsTelemetryWorkerService();
                    builder.Services.ConfigureFunctionsApplicationInsights();


                })
                .Build();

            host.Run();
        }
    }
}