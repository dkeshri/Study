using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplication
{
    public class ScheduledHostedService : HostedTimerService
    {
        private int counter = 0;
        public ScheduledHostedService():base(TimeSpan.FromSeconds(10))
        {
            
        }

        protected override Task OperationToPerforme(CancellationToken cancellationToken)
        {
            Console.WriteLine("Deepak "+counter++);
            return Task.CompletedTask;
        }
    }
}
