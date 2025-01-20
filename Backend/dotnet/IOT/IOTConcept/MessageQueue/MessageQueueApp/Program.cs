﻿using Dkeshri.MessageQueue.Extensions;
using Dkeshri.MessageQueue.Interfaces;
using Dkeshri.MessageQueue.RabbitMq.Extensions;
using MessageQueueApp;
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
        messageBroker.ClientProvidedName = "SenderApp";
        messageBroker.RegisterSenderServices = true;
        messageBroker.AddRabbitMqServices(config =>
        {
            config.HostName = "localhost";
            config.Port = 5672;
            config.UserName = "guest";
            config.Password = "guest";
            config.Queue.QueueName = "DataSyncQueue";
            config.Queue.IsDurable = true;
        });

    });
});



var host = builder.UseConsoleLifetime().Build();

using (IServiceScope serviceScope = host.Services.CreateScope())
{
    ISendMessage messageSender= serviceScope.ServiceProvider.GetRequiredService<ISendMessage>();
    string? message = "Test";
    do
    {
       
        Console.WriteLine("Enter Message to send: ");
        message = Console.ReadLine();
        messageSender.SendToQueue(message);
    }
    while (!string.IsNullOrEmpty(message));
}
   

host.RunAsync().Wait();