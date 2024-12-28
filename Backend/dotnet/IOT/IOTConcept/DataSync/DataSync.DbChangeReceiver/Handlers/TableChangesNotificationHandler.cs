using DataSync.Common.Models;
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
        public TableChangesNotificationHandler()
        {
            
        }

        public Task Handle(TableChangesNotification notification, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<TableChanges> tableChanges = notification.TableChanges;
            Console.WriteLine("Received Message from.......... RabitMq");

            foreach (var tableChange in tableChanges) {
                Console.WriteLine($"************ {tableChange.TableName} ************");
                Console.WriteLine($"Total Records: {tableChange.Records.Count}\n");
                foreach (var record in tableChange.Records)
                {
                    Console.WriteLine(record.Data.ToString());
                }
                Console.WriteLine($"\n************ {tableChange.TableName} End ************\n");
            }
            return Task.CompletedTask;
        }
    }
}
