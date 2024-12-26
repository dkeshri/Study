using DataSync.DBChangeEmitter.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<DatabaseChangeTrackerService>();

});


builder.RunConsoleAsync().Wait();