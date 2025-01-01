using DataSync.Common.Models;
using DataSync.DbChangeReceiver.Interfaces;
using DataSync.DbChangeReceiver.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Handlers
{
    internal class TableChangesNotificationHandler : INotificationHandler<TableChangesNotification>
    {
        IDbChangeApplyService _dbChangeApplyService;
        public TableChangesNotificationHandler(IDbChangeApplyService dbChangeApplyService)
        {
            _dbChangeApplyService = dbChangeApplyService;
        }

        public Task Handle(TableChangesNotification notification, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<TableChanges> tablesChanges = notification.TableChanges;
            Console.WriteLine("Received Message from.......... RabitMq");
            foreach (var tableChanges in tablesChanges) {
                Console.WriteLine($"************ {tableChanges.TableName} ************");
                Console.WriteLine($"Total Records: {tableChanges.Records.Count}\n");
                foreach (var record in tableChanges.Records)
                {
                    Console.WriteLine(record.Data.ToString());
                }
                Console.WriteLine($"\n************ {tableChanges.TableName} End ************\n");
            }

            foreach (var tableChanges in tablesChanges) {
                _dbChangeApplyService.ApplyTableChanges(tableChanges);
            }
            return Task.CompletedTask;
        }
    }
}
