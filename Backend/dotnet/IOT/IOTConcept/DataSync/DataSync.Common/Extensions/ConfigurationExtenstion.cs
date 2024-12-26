using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Extensions
{
    public static class ConfigurationExtenstion
    {
        public static string GetDatabaseSchema(this IConfiguration configuration)
        {
            return configuration.GetSection("Servers:DatabaseServer:Database:Schema").Value!;
        }

        public static int? GetDatabaseTransactionTimeOutInSec(this IConfiguration configuration)
        {
            string transactionTimeOutinSecString = configuration.GetSection("Servers:DatabaseServer:Database:TransactionTimeOutInSec").Value;
            if (!int.TryParse(transactionTimeOutinSecString, out var transationTimeOutInSec))
            {
                return null;
            }
            return transationTimeOutInSec;
        }
        public static bool GetIsDatabaseLoggingEnable(this IConfiguration configuration)
        {
            return Convert.ToBoolean(configuration.GetSection("Servers:DatabaseServer:Database:EnableDatabaseLogging").Value);
        }
    }
}
