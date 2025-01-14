using Dkeshri.DataSync.Common.Models;

namespace Dkeshri.DataSync.DBChangeEmitter.Interfaces
{
    internal interface IDatabaseChangeTrackerService
    {
        public  Task<IReadOnlyCollection<TableChanges>> GetChangesOfTrackedTableAsync();
        public  void UpdateTableChangeVersion(TableChanges tableChanges);
        public void EnableChangeTrackingOnTables();
        public bool IsDatabaseExistAndChangeTrackingEnabled();

        public void ApplyMigration();

    }
}
