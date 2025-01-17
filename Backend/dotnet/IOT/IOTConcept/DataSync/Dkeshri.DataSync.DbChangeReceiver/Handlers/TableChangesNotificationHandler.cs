using Dkeshri.DataSync.Common.Models;
using Dkeshri.DataSync.DbChangeReceiver.Interfaces;
using Dkeshri.DataSync.DbChangeReceiver.Notifications;
using MediatR;

namespace Dkeshri.DataSync.DbChangeReceiver.Handlers
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
            foreach (var tableChanges in tablesChanges) {
                _dbChangeApplyService.ApplyTableChanges(tableChanges);
            }
            return Task.CompletedTask;
        }
    }
}
