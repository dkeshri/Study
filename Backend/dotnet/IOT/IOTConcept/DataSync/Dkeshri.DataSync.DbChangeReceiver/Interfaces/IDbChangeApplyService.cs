using Dkeshri.DataSync.Common.Models;

namespace Dkeshri.DataSync.DbChangeReceiver.Interfaces
{
    internal interface IDbChangeApplyService
    {
        void ApplyTableChanges(TableChanges tableChanges);
    }
}
