using Microsoft.Extensions.Configuration;

namespace DataSync.DbChangeReceiverApp.Extensions
{
    internal static class ConfigurationExtensions
    {
        public static string GetDbConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection")!;
        }

        public static int GetDbTransactionTimeOutInSec(this IConfiguration configuration)
        {
            int.TryParse(configuration.GetConnectionString("TransactionTimeOutInSec")!, out int timeout);
            if (timeout <= 0)
            {
                timeout = 30;
            }
            return timeout;
        }
        public static string GetRabbitMqHostName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:HostName").Value!;
        }
        public static int GetRabbitMqHostPort(this IConfiguration configuration)
        {
            int.TryParse(configuration.GetSection("RabbitMq:Port").Value!,out int port);
            return port;
        }

        public static string GetRabbitMqUserName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:UserName").Value!;
        }
        public static string GetRabbitMqPassword(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:Password").Value!;
        }
        public static string GetRabbitMqQueueName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:Queue").Value!;
        }
        public static string GetRabbitMqExchangeName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:ExchangeName").Value!;
        }
        public static string GetRabbitMqClientProvidedName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:ClientProvidedName").Value!;
        }
    }
}
