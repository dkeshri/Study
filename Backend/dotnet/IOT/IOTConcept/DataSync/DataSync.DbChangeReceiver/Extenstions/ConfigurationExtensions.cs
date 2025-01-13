using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Extenstions
{
    internal static class ConfigurationExtensions
    {
        internal static string? GetRabbitMqHostName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:HostName").Value;
        }
        internal static string? GetRabbitMqUserName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:UserName").Value;
        }
        internal static string? GetRabbitMqPassword(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:Password").Value;
        }
        internal static string? GetRabbitMqExchangeName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:ExchangeName").Value;
        }
        internal static string? GetRabbitMqClientProvidedName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:ClientProvidedName").Value;
        }
        internal static int GetRabbitMqPort(this IConfiguration configuration)
        {
            string? value = configuration.GetSection("RabbitMq:Port").Value;
            int port;
            int.TryParse(value, out port);
            return port;
        }
        internal static string? GetRabbitMqQueueName(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq:Queue").Value;
        }
    }
}
