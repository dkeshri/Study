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
            IReadOnlyCollection<TableChanges> tablesChanges = await DatabaseChangeTrackerService.GetChangesOfTrackedTableAsync();

            foreach (TableChanges tableChange in tablesChanges)
            {
                Console.WriteLine($"************ {tableChange.TableName} ************");
                Console.WriteLine($"Total Records: {tableChange.Records.Count}\n" );
                foreach (var record in tableChange.Records) {
                    Console.WriteLine(record.Data.ToString());
                }
                Console.WriteLine($"\n************ {tableChange.TableName} End ************\n");
            }

            if (tablesChanges.Count > 0)
            {
                Console.WriteLine("Sending.......... To RabitMq");
                SendMessageToRabbiMq.SendMessageToRabbitMq(tablesChanges);
                Console.WriteLine("Sent.......... To RabitMq\n");
                foreach(TableChanges tableChanges in tablesChanges)
                {
                    DatabaseChangeTrackerService.UpdateTableChangeVersion(tableChanges);
                }
            }
            else
            {
                Console.WriteLine("No changes detected!");
            }
        }
    }
}
