using ExecuterApp.Extensions;
using ExecuterApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Read from appsettings.json
          .AddEnvironmentVariables();
});

builder.ConfigureServices((context, services) =>
{
    services.AddServices();
    services.AddHostedService<ExecuterAppHostedService>();
    services.AddRabbitQ();
});

builder.RunConsoleAsync().Wait();