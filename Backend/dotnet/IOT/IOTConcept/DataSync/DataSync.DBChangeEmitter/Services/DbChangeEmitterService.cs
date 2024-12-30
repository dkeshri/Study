using DataSync.Common.Models;
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
        private IDatabaseChangeTrackerService DatabaseChangeTrackerService { get; }
        private ISendMessageToRabbitMq SendMessageToRabbiMq { get; }
        public DbChangeEmitterService(IDatabaseChangeTrackerService databaseChangeTrackerService,
            ISendMessageToRabbitMq sendMessageToRabbitMq):base(TimeSpan.FromSeconds(10))
        {
            DatabaseChangeTrackerService = databaseChangeTrackerService;
            SendMessageToRabbiMq = sendMessageToRabbitMq;
        }

        protected override async Task OperationToPerforme(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<TableChanges> tableChanges = await DatabaseChangeTrackerService.GetChangesOfTrackedTableAsync();

            foreach (TableChanges tablechange in tableChanges)
            {
                Console.WriteLine($"************ {tablechange.TableName} ************");
                Console.WriteLine($"Total Records: {tablechange.Records.Count}\n" );
                foreach (var record in tablechange.Records) {
                    Console.WriteLine(record.Data.ToString());
                }
                Console.WriteLine($"\n************ {tablechange.TableName} End ************\n");
            }

            if (tableChanges.Count > 0)
            {
                Console.WriteLine("Sending.......... To RabitMq");
                SendMessageToRabbiMq.SendMessageToRabbitMq(tableChanges);
                Console.WriteLine("Sent.......... To RabitMq\n");
            }
            else
            {
                Console.WriteLine("No changes detected!");
            }
        }
    }
}
