using DataSync.Common.Services;
using DataSync.DBChangeEmitter.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Services
{
    internal class DbChangeEmitterService : HostedTimerService
    {
        private int testCounter = 0;
        IDatabaseChangeTrackerService DatabaseChangeTrackerService { get; }
        public DbChangeEmitterService(IDatabaseChangeTrackerService databaseChangeTrackerService):base(TimeSpan.FromSeconds(4))
        {
            DatabaseChangeTrackerService = databaseChangeTrackerService;
        }

        protected override Task OperationToPerforme(CancellationToken cancellationToken)
        {
            Console.WriteLine(testCounter++);
            string tableName = "Orders";
            long version = DatabaseChangeTrackerService.GetTableChangeVersion(tableName);
            Console.WriteLine($"Table {tableName} ChangeVersion is : {version}");
            return Task.CompletedTask;
        }
    }
}
