using Dkeshri.DataSync.Common.Data.Entities;
using Dkeshri.DataSync.Common.Models;
namespace Dkeshri.DataSync.Common.Interfaces.Repositories
{
    public interface IChangeTrackerRepository
    {
        public IReadOnlyCollection<ChangeTracker> GetTrackedTables();
        public long GetTableChangeVersion(string tableName);
        public long GetDbChangeTrackingCurrentVersion();
        public Task<long> GetDbChangeTrackingCurrentVersionAsync();

        public IReadOnlyCollection<TableRecord>? GetChangedTableRecords(ChangeTracker trackingTable);
        public Task<IReadOnlyCollection<TableRecord>?> GetChangedTableRecordsAsync(ChangeTracker trackingTable)
            => Task.FromResult(GetChangedTableRecords(trackingTable));
        public void UpdateTableChangeVersion(string tableName, long lastChangeVersion);
        public void RemoveTableFromChangeTracker(string tableName);
        public ICollection<ForeignKeyRelationship>? GetForeignRelationships();
        public void EnableChangeTrackingOnTable(string tableName);
        public bool IsDatabaseChangeTrackingEnabled();
        public bool IsDatabaseExist();

        public void ApplyMigration();

    }
}
