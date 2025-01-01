using DataSync.Common.Data.Entities;
using DataSync.Common.Interfaces.DataContext;
using DataSync.Common.Interfaces.Repositories;
using DataSync.Common.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Repositories
{
    internal class ChangeTrackerRepository : IChangeTrackerRepository
    {
        private IDataContext DataContext { get; }
        public DbSet<ChangeTracker> ChangeTrackers => this.DataContext.DbContext.Set<ChangeTracker>();
        public ChangeTrackerRepository(IDataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IReadOnlyCollection<ChangeTracker> GetTrackedTables()
        {
            return ChangeTrackers.AsNoTracking().ToList();
        }

        public long GetTableChangeVersion(string tableName)
        {
            ChangeTracker? record = ChangeTrackers.Where(x => x.TableName.Equals(tableName)).AsNoTracking()
                .FirstOrDefault();
            if (record == null) {
                throw new InvalidOperationException($"Table {tableName} Not found");
            }
            return record.ChangeVersion;
        }

        public void UpdateTableChangeVersion(string tableName, long lastChangeVersion)
        {
            ChangeTrackers.Where(x => x.TableName.Equals(tableName))
                .ExecuteUpdate(setters => setters
                .SetProperty(x => x.ChangeVersion, lastChangeVersion));
            Console.WriteLine($"Updated Change Tracking Version of Table: {tableName}");
        }

        public long GetDbChangeTrackingCurrentVersion()
        {
            long CurrentVersion = 0;
            var sqlParam = new SqlParameter("@ChangeVersion", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };
            DataContext.DbContext.Database.ExecuteSqlRaw("SET @ChangeVersion = CHANGE_TRACKING_CURRENT_VERSION();",
                sqlParam);
            CurrentVersion = sqlParam.Value is not null ? (long)sqlParam.Value : 0;
            return CurrentVersion;
        }

        public async Task<long> GetDbChangeTrackingCurrentVersionAsync()
        {
            long CurrentVersion = 0;

            var sqlParam = new SqlParameter("@ChangeVersion", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };

            await DataContext.DbContext.Database.ExecuteSqlRawAsync("SET @ChangeVersion = CHANGE_TRACKING_CURRENT_VERSION();",
                sqlParam);
            CurrentVersion = sqlParam.Value is not null ? (long)sqlParam.Value : 0;
            return CurrentVersion;
        }

        private IEnumerable<string> GetPrimaryKeys(string tableName)
        {
            List<string> keys = new List<string>();
            var dbConnections = DataContext.DbContext.Database.GetDbConnection();
            try
            {
                dbConnections.Open();
                using (var cmd = dbConnections.CreateCommand())
                {
                    cmd.CommandText = $@"
                        SELECT 
                            c.name AS ColumnName
                        FROM 
                            sys.indexes AS i
                            INNER JOIN sys.index_columns AS ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                            INNER JOIN sys.columns AS c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        WHERE 
                            i.is_primary_key = 1
                            AND i.object_id = OBJECT_ID('{tableName}');";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            keys.Add(reader.GetString(0));
                        }
                    }
                    dbConnections.Close();
                }
            }
            finally 
            {
                
                dbConnections.Close();
            }

            return keys;
        }

        public async Task<IReadOnlyCollection<TableRecord>> GetChangedTableRecordsAsync(ChangeTracker trackingTable)
        {
            string tableName = trackingTable.TableName;
            long lastChangeVersion = trackingTable.ChangeVersion;
            var primaryKeys = GetPrimaryKeys(tableName);
            string condition = string.Join(" AND ", primaryKeys.Select(x => $"T.{x} = CT.{x}"));

            string query = $@"
                SELECT 
                    (SELECT TOP 1 * FROM {tableName} T WHERE {condition} FOR JSON AUTO) AS Data,
                    CT.SYS_CHANGE_VERSION As ChangeVersion,
                    CT.SYS_CHANGE_OPERATION As Operation
                FROM CHANGETABLE(CHANGES {tableName}, {lastChangeVersion}) AS CT
                LEFT OUTER JOIN
                    {tableName} T ON {condition}";
            var changes = await GetChangesFromDatabaseAsync(query);
            return changes;
        }

        private async Task<List<TableRecord>> GetChangesFromDatabaseAsync(string sqlQuery)
        {
            List<TableRecord> changes = new List<TableRecord>();
            using (var command = DataContext.DbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlQuery;
                await DataContext.DbContext.Database.OpenConnectionAsync();
                try
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new TableRecord
                            {
                                Id = Guid.NewGuid(),
                                Data = reader["Data"]?.ToString(),
                                ChangeVersion = Convert.ToInt64(reader["ChangeVersion"]),
                                Operation = reader["Operation"]?.ToString(),
                            };
                            changes.Add(record);
                        }
                    }
                }
                finally
                {
                    await DataContext.DbContext.Database.CloseConnectionAsync();
                }
            }
            return changes;
        }

        
    }
}
