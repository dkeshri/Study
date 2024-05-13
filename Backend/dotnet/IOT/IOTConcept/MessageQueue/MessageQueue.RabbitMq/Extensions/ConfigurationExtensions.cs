using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.RabbitMq.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetRabbitMqHostName(this IConfiguration configuration)
        {
            return configuration.GetSection("MessageQueues:RabbitMq:Server:HostName").Value;
        }
        public static string GetRabbitMqUserName(this IConfiguration configuration)
        {
            return configuration.GetSection("MessageQueues:RabbitMq:Server:UserName").Value;
        }
        public static string GetRabbitMqPassword(this IConfiguration configuration)
        {
            return configuration.GetSection("MessageQueues:RabbitMq:Server:Password").Value;
        }
        public static int GetRabbitMqPort(this IConfiguration configuration)
        {
            string value = configuration.GetSection("MessageQueues:RabbitMq:Server:Port").Value;
            int port;
            int.TryParse(value, out port);
            return port;
        }
    }
}
