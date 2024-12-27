﻿using DataSync.Common.Extensions;
using DataSync.DBChangeEmitter.Extensions;
using DataSync.DBChangeEmitter.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((context, config) =>
{
    // Clear default sources and add custom sources
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Read from appsettings.json
          .AddEnvironmentVariables(); // Read from environment variables
});

builder.ConfigureServices((hostContext, services) =>
{
    services.AddDataLayer();
    services.AddServices();
    services.AddHostedService<DbChangeEmitterService>();

});


builder.RunConsoleAsync().Wait();