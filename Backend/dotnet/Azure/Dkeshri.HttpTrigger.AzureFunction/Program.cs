using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dkeshri.HttpTrigger.AzureFunction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults( builder =>
                {
                    builder.Services.AddScoped<TestClass>();
                })
                .Build();

            host.Run();
        }
    }
}
