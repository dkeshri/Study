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
    }
}
