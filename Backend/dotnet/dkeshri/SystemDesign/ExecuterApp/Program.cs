using Dkeshri.MessageQueue.Extensions;
using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using ExecuterApp.Extensions;
using ExecuterApp.Interfaces;
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
    services.AddMessageBroker(messageBroker =>
    {
        messageBroker.RegisterReceiverServices = true;
        messageBroker.ClientProvidedName = "Receiver";
        messageBroker.AddRabbitMqServices((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "localhost";
            rabbitMqConfig.Port = 5672;
            rabbitMqConfig.QueueName = "DataSyncQueue";
            rabbitMqConfig.UserName = "guest";
            rabbitMqConfig.Password = "guest";
        });
    });
});


var host = builder.UseConsoleLifetime().Build();

using (var scope = host.Services.CreateScope())
{
    var messageReceiver = scope.ServiceProvider.GetRequiredService<IMessageReceiver>();
    messageReceiver.MessageHandler = (message) =>
    {
        // Process Message and retuen boolean value after message processed.
        Console.WriteLine(message);

        // message will only acknowledge to RabbitMq Service if return true.

        return true; 
    };
}

host.RunAsync().Wait();