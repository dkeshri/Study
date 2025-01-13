using DataSync.Common.Data.Entities;
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

namespace DataSync.Common.Data.DataContext
{
    internal class DataSyncDbContext : DataContextBase
    {
        public DataSyncDbContext(DbConfig configuration) : base(configuration)
        {

        }

        public override string GetConnectionString()
        {
            string? connectionString = Configuration.ConnectionString;
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
                int? transationTimout = Configuration.TransactionTimeOutInSec;

                optionsBuilder.UseSqlServer(connectionString,
                    opt => opt.CommandTimeout(transationTimout));

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChangeTracker>(entity =>
            {
                entity.HasNoKey(); // Configures this entity as keyless
                entity.HasIndex(tc => tc.TableName)
                      .IsUnique(); // Configures TableName as a unique key
            });
        }
        public virtual DbSet<ChangeTracker> ChangeTrackers { get; set; }
    }
}
