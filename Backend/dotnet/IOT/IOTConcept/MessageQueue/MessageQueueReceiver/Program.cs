﻿using Dkeshri.MessageQueue.Extensions;
using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        messageBroker.ClientProvidedName = "ReceiverApp";
        messageBroker.RegisterReceiverServices = true;
        messageBroker.AddRabbitMqServices(config =>
        {
            config.HostName = "localhost";
            config.Port = 5672;
            config.UserName = "guest";
            config.Password = "guest";
        }).UseQueue(queueConfig =>
        {
            queueConfig.QueueName = "TestQueue";
            queueConfig.IsDurable = true;
        });

    });
});


var host = builder.UseConsoleLifetime().Build();

using (IServiceScope serviceScope = host.Services.CreateScope())
{
    var startup = serviceScope.ServiceProvider.GetRequiredService<IStartup>();
    startup.OnStart();

    serviceScope.ServiceProvider.GetRequiredService<IMessageReceiver>().MessageHandler = (message) =>
    {
        Console.WriteLine(message);
        return false;
    };
}

host.RunAsync().Wait();
