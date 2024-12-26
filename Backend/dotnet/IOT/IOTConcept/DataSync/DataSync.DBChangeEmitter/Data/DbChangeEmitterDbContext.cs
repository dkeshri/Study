using DataSync.Common.Interfaces.DataContext;
using DataSync.DBChangeEmitter.Extensions;
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
    internal class DbChangeEmitterDbContext : DataContextBase
    {
        public DbChangeEmitterDbContext(IConfiguration configuration): base(configuration)
        {
            
        }

        public override string GetConnectionString()
        {
            string connectionFormateString = Configuration.GetConnectionStringFormate();
            string databaseServer = Configuration.GetDatabaseServerAddress();
            string databaseName = Configuration.GetDatabaseName();
            string databaseUserName = Configuration.GetDatabaseUserName();
            string databasePassword = Configuration.GetDatabasePassword();


            string connectionStringWithCrediential = string.Format(connectionFormateString, databaseServer, databaseName, databaseUserName, databasePassword);

            return connectionStringWithCrediential;
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
