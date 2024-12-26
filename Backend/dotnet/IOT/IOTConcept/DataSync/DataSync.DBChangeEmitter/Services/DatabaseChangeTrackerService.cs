using DataSync.Common.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Services
{
    internal class DatabaseChangeTrackerService : HostedTimerService
    {
        private int testCounter = 0;
        public DatabaseChangeTrackerService():base(TimeSpan.FromSeconds(10))
        {
            
        }

        protected override Task OperationToPerforme(CancellationToken cancellationToken)
        {
            Console.WriteLine(testCounter++);
            return Task.CompletedTask;
        }
    }
}
