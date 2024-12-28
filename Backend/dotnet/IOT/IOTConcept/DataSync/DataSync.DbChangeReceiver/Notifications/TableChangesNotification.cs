using DataSync.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Notifications
{
    internal class TableChangesNotification : INotification
    {
        public IReadOnlyCollection<TableChanges> TableChanges { get; set; } = null!;
    }
}
