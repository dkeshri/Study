using DataSync.Common.Interfaces.DataContext;
using DataSync.Common.Services;
using Microsoft.Extensions.Configuration;
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
        private IDataContext dataContext;
        public DatabaseChangeTrackerService(IDataContext dataContext):base(TimeSpan.FromSeconds(4))
        {
            this.dataContext = dataContext;
        }

        protected override Task OperationToPerforme(CancellationToken cancellationToken)
        {
            Console.WriteLine(testCounter++);
            Console.WriteLine(dataContext.DbContext.Database.CanConnect());
            return Task.CompletedTask;
        }
    }
}
