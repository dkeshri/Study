using Dkeshri.DataSync.Common.Models;

namespace Dkeshri.DataSync.DBChangeEmitter.Interfaces
{
    internal interface ISendMessageToRabbitMq
    {
        bool SendMessageToRabbitMq(IReadOnlyCollection<TableChanges> tableChanges);
    }
}
