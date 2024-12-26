using DataSync.Common.Extensions;
using DataSync.Common.Interfaces.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Data
{
    internal class DataSyncDbContext : DataContextBase
    {
        public DataSyncDbContext(IConfiguration configuration): base(configuration)
        {
            
        }

        public override string GetConnectionString()
        {
            string? connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (connectionString == null)
            {
                 throw new Exception("Connection String can't be empty");
            }
            return connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = GetConnectionString();
                int? transationTimout = Configuration.GetDatabaseTransactionTimeOutInSec();
                bool enableDatabaseLogging = Configuration.GetIsDatabaseLoggingEnable();

                optionsBuilder.UseSqlServer(connectionString,
                    opt => opt.CommandTimeout(transationTimout));

                if (enableDatabaseLogging)
                {
                    optionsBuilder.EnableSensitiveDataLogging()
                        .LogTo(Console.WriteLine, LogLevel.Information);
                }

            }
        }
    }
}
