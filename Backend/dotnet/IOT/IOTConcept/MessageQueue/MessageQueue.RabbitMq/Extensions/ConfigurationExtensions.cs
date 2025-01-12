using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Extensions
{
    internal static class ConfigurationExtensions
    {
        public static string? GetRabbitMqHostName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:HostName").Value;
        }
        public static string? GetRabbitMqUserName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:UserName").Value;
        }
        public static string? GetRabbitMqPassword(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:Password").Value;
        }
        public static string? GetRabbitMqExchangeName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:ExchangeName").Value;
        }
        public static string? GetRabbitMqClientProvidedName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:ClientProvidedName").Value;
        }
        public static int GetRabbitMqPort(this IConfiguration configuration)
        {
            string? value = configuration.GetSection("RabbitMq:Port").Value;
            int port;
            int.TryParse(value, out port);
            return port;
        }
        public static string? GetRabbitMqQueueName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:Queue").Value;
        }
    }
}
