using Dkeshri.DataSync.Common.Models;
using MediatR;


namespace Dkeshri.DataSync.DbChangeReceiver.Notifications
{
    internal class TableChangesNotification : INotification
    {
        public IReadOnlyCollection<TableChanges> TableChanges { get; set; } = null!;
    }
}
