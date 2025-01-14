using Dkeshri.DataSync.Common.Data.Entities;

namespace Dkeshri.DataSync.DBChangeEmitter.Interfaces
{
    internal interface ITopologicalSorterService
    {
        public IReadOnlyCollection<ChangeTracker> TopologicalSort(IReadOnlyCollection<ChangeTracker> changeTrackers);
    }
}
