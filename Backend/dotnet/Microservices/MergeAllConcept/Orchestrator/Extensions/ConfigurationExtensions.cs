using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestrator.Extensions
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

        public static RabbitMqConfiguration? GetRabbitMqConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>();
        }
    }
}
