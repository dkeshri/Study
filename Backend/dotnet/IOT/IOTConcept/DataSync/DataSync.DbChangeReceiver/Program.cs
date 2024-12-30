using DataSync.DbChangeReceiver.Extenstions;
using DataSync.DbChangeReceiver.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((context, config) =>
{
    var env = context.HostingEnvironment;
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Read from appsettings.json
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables(); // Read from environment variables
});
builder.ConfigureServices((hostContext, services) =>
{
    services.AddServices();
    services.AddHandlers();
    services.AddHostedService<DbChangeReceiverService>();
});


builder.RunConsoleAsync().Wait();