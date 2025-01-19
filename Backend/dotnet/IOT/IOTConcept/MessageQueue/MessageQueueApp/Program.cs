using Dkeshri.MessageQueue.Extensions;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    var env = context.HostingEnvironment;
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables(); // Read from environment variables
});

builder.ConfigureServices((hostContext, services) =>
{
    services.AddMessageBroker(messageBroker =>
    {
        messageBroker.ClientProvidedName = "Sender";
        messageBroker.RegisterSenderServices = true;
        messageBroker.AddRabbitMqServices(config =>
        {
            config.HostName = "localhost";
            config.Port = 5672;
            config.UserName = "guest";
            config.Password = "guest";
            config.QueueName = "DataSyncQueue";
            config.IsQueueDurable = true;
        });

    });
});

builder.RunConsoleAsync().Wait();