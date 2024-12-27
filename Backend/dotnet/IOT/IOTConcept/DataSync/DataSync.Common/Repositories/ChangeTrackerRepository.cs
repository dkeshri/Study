using DataSync.Common.Data.Entities;
using DataSync.Common.Interfaces.DataContext;
using DataSync.Common.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public long GetTableChangeVersion(string tableName)
        {
            ChangeTracker? record = ChangeTrackers.Where(x => x.TableName.Equals(tableName)).AsNoTracking()
                .FirstOrDefault();
            if (record == null) {
                throw new InvalidOperationException($"Table {tableName} Not found");
            }
            return record.ChangeVersion;
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

        public List<string> GetPrimaryKeys(string tableName)
        {
            List<string> keys = new List<string>();
            var dbConnections = DataContext.DbContext.Database.GetDbConnection();
            using var cmd = dbConnections.CreateCommand();
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
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                keys.Add(reader.GetString(0));
            }
            return keys;
        }
    }
}
