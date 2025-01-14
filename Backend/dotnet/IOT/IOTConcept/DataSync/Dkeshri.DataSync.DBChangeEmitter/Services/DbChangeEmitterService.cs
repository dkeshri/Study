using Dkeshri.DataSync.Common.Models;
using Dkeshri.DataSync.Common.Services;
using Dkeshri.DataSync.DBChangeEmitter.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Dkeshri.DataSync.DBChangeEmitter.Services
{
    internal class DbChangeEmitterService : HostedTimerService
    {
        private IDatabaseChangeTrackerService DatabaseChangeTrackerService { get; }
        private ISendMessageToRabbitMq SendMessageToRabbiMq { get; }

        private IHost _host;
        public DbChangeEmitterService(IDatabaseChangeTrackerService databaseChangeTrackerService,
            ISendMessageToRabbitMq sendMessageToRabbitMq,
            IHost host
            ):base(TimeSpan.FromSeconds(10))
        {
            DatabaseChangeTrackerService = databaseChangeTrackerService;
            SendMessageToRabbiMq = sendMessageToRabbitMq;
            _host = host;
        }

        protected override async Task OperationToPerforme(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<TableChanges> tablesChanges = await DatabaseChangeTrackerService.GetChangesOfTrackedTableAsync();

            if (tablesChanges.Count > 0)
            {
                Console.WriteLine("Sending.......... To RabitMq");
                bool isMessageSent = SendMessageToRabbiMq.SendMessageToRabbitMq(tablesChanges);
                if (isMessageSent) {
                    Console.WriteLine("Sent.......... To RabitMq\n");
                    foreach (TableChanges tableChanges in tablesChanges)
                    {
                        DatabaseChangeTrackerService.UpdateTableChangeVersion(tableChanges);
                    }
                }
                
            }
            else
            {
                Console.WriteLine("No changes detected!");
            }
        }

        protected override Task OnStartup(CancellationToken cancellationToken)
        {
            bool isDbExistAndChangeTrackingEnabled =  DatabaseChangeTrackerService.IsDatabaseExistAndChangeTrackingEnabled();
            if (!isDbExistAndChangeTrackingEnabled)
            {
                Console.WriteLine("Shutting down the DbChangeEmitter Application!");
                _host.StopAsync();
            }
            else
            {
                DatabaseChangeTrackerService.ApplyMigration();
                DatabaseChangeTrackerService.EnableChangeTrackingOnTables();
            }
            return Task.CompletedTask;
        }
    }
}
