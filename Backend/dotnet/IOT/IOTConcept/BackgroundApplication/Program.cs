// See https://aka.ms/new-console-template for more information
using BackgroundApplication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<MyConsoleService>();
});


builder.RunConsoleAsync().Wait();