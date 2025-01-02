using DataSync.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Interfaces
{
    internal interface ISendMessageToRabbitMq
    {
        bool SendMessageToRabbitMq(IReadOnlyCollection<TableChanges> tableChanges);
    }
}
