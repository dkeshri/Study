using DataSync.Common.Extensions;
using DataSync.DbChangeReceiver.Extenstions;
using DataSync.DbChangeReceiver.Interfaces;
using DataSync.DbChangeReceiver.Services;
using MessageQueue.RabbitMq.Interfaces;
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
    services.AddDataLayer();
    services.AddServices();
    services.AddHandlers();
});

var host = builder.UseConsoleLifetime().Build();

using (var scope = host.Services.CreateScope())
{
    var messageHandler = scope.ServiceProvider.GetRequiredService<IRabbitMqMessageHandler>();
    var messageReceiver = scope.ServiceProvider.GetRequiredService<IMessageReceiver>();
    messageReceiver.MessageHandler = messageHandler.HandleMessage;
    
}

host.RunAsync().Wait();