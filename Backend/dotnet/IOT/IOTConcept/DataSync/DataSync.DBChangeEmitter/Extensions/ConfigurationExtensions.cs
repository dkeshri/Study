using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Extensions
{
    internal static class ConfigurationExtensions
    {
        public static string GetDatabaseSchema(this IConfiguration configuration)
        {
            return configuration.GetSection("Servers:DatabaseServer:Database:Schema").Value;
        }
        public static string GetConnectionStringFormate(this IConfiguration configuration)
        {
            return configuration.GetSection("Servers:DatabaseServer:ConnectionString").Value;
        }
        public static string GetDatabaseServerAddress(this IConfiguration configuration)
        {
            return configuration.GetSection("Servers:DatabaseServer:ServerAddress").Value;
        }
        public static string GetDatabaseName(this IConfiguration configuration)
        {
            return configuration.GetSection("Servers:DatabaseServer:Database:Name").Value;
        }
        public static string GetDatabaseUserName(this IConfiguration configuration)
        {
            return configuration.GetSection("Credientials:DatabaseServer:UserName").Value;
        }
        public static string GetDatabasePassword(this IConfiguration configuration)
        {
            return configuration.GetSection("Credientials:DatabaseServer:Password").Value;
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
