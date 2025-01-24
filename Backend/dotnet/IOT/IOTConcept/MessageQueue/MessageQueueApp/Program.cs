using Dkeshri.MessageQueue.Extensions;
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
        }).UseExchange(exchangeConfig =>
        {
            exchangeConfig.ExchangeName = "Deepak";
            exchangeConfig.IsDurable = true;
        });

    });
});

var host = builder.UseConsoleLifetime().Build();

using (IServiceScope serviceScope = host.Services.CreateScope())
{
    var startup = serviceScope.ServiceProvider.GetRequiredService<IStartup>();
    startup.OnStart();
    IMessageSender messageSender= serviceScope.ServiceProvider.GetRequiredService<IMessageSender>();
    string? message = "";
    do
    {
        string routingKey = "deepakRouting";
        messageSender.SendToExchange(message, routingKey);
        Console.WriteLine("Enter Message to send: ");
        message = Console.ReadLine();
        
    }
    while (!string.IsNullOrEmpty(message));
}
   

host.RunAsync().Wait();