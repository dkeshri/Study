using DataSync.Common.Data.Entities;
using DataSync.Common.Interfaces.Repositories;
using DataSync.Common.Models;
using DataSync.DBChangeEmitter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Services
{
    internal class DatabaseChangeTrackerService : IDatabaseChangeTrackerService
    {
        IChangeTrackerRepository ChangeTrackerRepository { get; }
        ITopologicalSorterService TopologicalSorterService { get; }

        public DatabaseChangeTrackerService(IChangeTrackerRepository changeTrackerRepository,ITopologicalSorterService topologicalSorterService)
        {
            ChangeTrackerRepository = changeTrackerRepository;
            TopologicalSorterService = topologicalSorterService;
        }

        public async Task<IReadOnlyCollection<TableChanges>> GetChangesOfTrackedTableAsync()
        {
            List<TableChanges> changes = new List<TableChanges>();
            var trackingTables = ChangeTrackerRepository.GetTrackedTables();
            foreach (var table in trackingTables) 
            { 
                var tableChanges = await GetTableChangesAsync(table);
                if(tableChanges != null)
                {
                    changes.Add(tableChanges);
                }
            }
            return changes;
        }

        private async Task<TableChanges?> GetTableChangesAsync(ChangeTracker trackingTable) 
        {
            var changesForTable = await ChangeTrackerRepository.GetChangedTableRecordsAsync(trackingTable);
            if(changesForTable == null || changesForTable.Count == 0)
            {
                return null;
            }
            return new TableChanges()
            {
                TableName = trackingTable.TableName,
                CreatedOn = DateTime.UtcNow,
                Records = changesForTable
            };
        }

        public void UpdateTableChangeVersion(TableChanges tableChanges)
        {
            try
            {
                string tableName = tableChanges.TableName;
                long lastChangeVersion = tableChanges.Records.Max(x=> x.ChangeVersion);
                ChangeTrackerRepository.UpdateTableChangeVersion(tableName, lastChangeVersion);
            }
            catch (Exception ex) { 
            
            }
        }

        public void EnableChangeTrackingOnTables()
        {
            var trackingTables = ChangeTrackerRepository.GetTrackedTables();
            foreach (var trackingTable in trackingTables)
            {
                ChangeTrackerRepository.EnableChangeTrackingOnTable(trackingTable.TableName);
            }
        }
    }
}
