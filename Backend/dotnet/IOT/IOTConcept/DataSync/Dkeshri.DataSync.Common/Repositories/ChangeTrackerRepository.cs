﻿using Dkeshri.DataSync.Common.Data.Entities;
using Dkeshri.DataSync.Common.Interfaces.DataContext;
using Dkeshri.DataSync.Common.Interfaces.Repositories;
using Dkeshri.DataSync.Common.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Dkeshri.DataSync.Common.Repositories
{
    internal class ChangeTrackerRepository : Repository, IChangeTrackerRepository
    {
        
        public DbSet<ChangeTracker> ChangeTrackers => DataContext.DbContext.Set<ChangeTracker>();
        public ChangeTrackerRepository(IDataContext dataContext) : base(dataContext)
        {
            
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

        public void RemoveTableFromChangeTracker(string tableName) 
        {
            Console.WriteLine($"Removing {tableName} from ChangeTracker!");
            ChangeTrackers.Where(x => x.TableName.Equals(tableName))
                .ExecuteDelete();    
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
        private bool IsTableExist(string tableName)
        {
            bool tableExists = false;
            try
            {
                string query = $@"
                            SELECT 
                                CASE WHEN OBJECT_ID('[{tableName}]', 'U') IS NOT NULL 
                                THEN CAST(1 AS BIT) 
                                ELSE CAST(0 AS BIT) 
                                END AS [Value]";
                tableExists = DataContext.DbContext.Database.SqlQueryRaw<bool>(query).Single();
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            return tableExists;
        }

        public IReadOnlyCollection<TableRecord>? GetChangedTableRecords(ChangeTracker trackingTable)
        {
            string tableName = trackingTable.TableName;
            long lastChangeVersion = trackingTable.ChangeVersion;

            if (!IsTableExist(tableName))
            {
                Console.WriteLine($"Table :{tableName} does not exist!");
                RemoveTableFromChangeTracker(tableName);
                return null;
            }


            var primaryKeys = GetPrimaryKeys(tableName);
            string condition = string.Join(" AND ", primaryKeys.Select(x => $"T.{x} = CT.{x}"));

            var primaryKeysValueQuery = string.Join(",", primaryKeys.Select(x => $"CT.{x} As {x}"));

            if (string.IsNullOrEmpty(condition))
                return null;

            string query = $@"
                SELECT 
                    (SELECT TOP 1 * FROM [{tableName}] T WHERE {condition} FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS Data,
                    (SELECT {primaryKeysValueQuery} FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS PkKeysWithValues,
                    CT.SYS_CHANGE_VERSION As ChangeVersion,
                    CT.SYS_CHANGE_OPERATION As Operation
                FROM CHANGETABLE(CHANGES [{tableName}], {lastChangeVersion}) AS CT
                LEFT OUTER JOIN
                    [{tableName}] T ON {condition}";
            var changes = GetChangesFromDatabase(query);
            return changes;
        }

        private List<TableRecord>? GetChangesFromDatabase(string sqlQuery)
        {
            List<TableRecord> changes = new List<TableRecord>();
            try
            {
                changes = DataContext.DbContext.Database.SqlQueryRaw<TableRecord>(sqlQuery).ToList();
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error : While fetching table changes!");
                Console.WriteLine(ex.Message);
            }
            return changes;
        }

        public ICollection<ForeignKeyRelationship>? GetForeignRelationships()
        {
            try
            {
                var query = @"
                        SELECT 
                            FK.TABLE_NAME As FkTable,
                            PK.TABLE_NAME As PkTable
                        FROM 
                            INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
                            INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK
                                ON RC.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                            INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK
                                ON RC.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
                        WHERE FK.CONSTRAINT_TYPE = 'FOREIGN KEY';"
                        ;
                var data = DataContext.DbContext.Database.SqlQueryRaw<ForeignKeyRelationship>(query).ToList();
                return data;
            }
            catch (Exception ex) {
                Console.WriteLine($"Can not retrive the ForigenKey relationship, Error: {ex.Message}");
            }
            return null;
        }

        public void EnableChangeTrackingOnTable(string tableName)
        {
            try
            {
                bool isChangeTrackingEnabled = IsChangeTrackingEnabled(tableName);
                if (!isChangeTrackingEnabled) {
                    string query = $@"
                        ALTER TABLE [{tableName}]
                        ENABLE CHANGE_TRACKING;";
                    DataContext.DbContext.Database.ExecuteSqlRaw(query);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private bool IsChangeTrackingEnabled(string tableName)
        {
            bool isChangeTrackingEnabled = false;
                string query = $@"
                        SELECT 
                            CASE WHEN ct.object_id IS NOT NULL 
                                    THEN CAST(1 AS BIT) 
                                    ELSE CAST(0 AS BIT) 
                                    END AS [Value]
                                FROM 
                                    sys.tables t
                                LEFT JOIN 
                                    sys.change_tracking_tables ct ON t.object_id = ct.object_id
                                WHERE 
                                    t.name = '{tableName}' ";
            isChangeTrackingEnabled = DataContext.DbContext.Database.SqlQueryRaw<bool>(query).Single();
            return isChangeTrackingEnabled;
        }

        public bool IsDatabaseChangeTrackingEnabled()
        {
            bool isDbChangeTrackingEnabled = false;
            try
            {
                string databaseName = DataContext.DbContext.Database.GetDbConnection().Database;
                string query = $@"
                            SELECT CASE WHEN COUNT(*) > 0 
                                THEN CAST(1 AS BIT) 
                                ELSE CAST(0 AS BIT) 
                            END AS [Value]
                            FROM sys.change_tracking_databases
                            WHERE database_id = DB_ID('{databaseName}')";
                isDbChangeTrackingEnabled = DataContext.DbContext.Database.SqlQueryRaw<bool>(query).Single();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return isDbChangeTrackingEnabled;
        }

        public bool IsDatabaseExist()
        {
            bool isDatabaseExist = false;
            string databaseName = "";
            try
            {
                databaseName = DataContext.DbContext.Database.GetDbConnection().Database;
                string query = $@"
                            SELECT 
                                CASE  
                                    WHEN EXISTS (SELECT 1 FROM sys.databases WHERE name = '{databaseName}') 
                                    THEN CAST(1 AS BIT) 
                                    ELSE CAST(0 AS BIT) 
                                END AS [Value]";
                isDatabaseExist = DataContext.DbContext.Database.SqlQueryRaw<bool>(query).Single();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Database => {databaseName} does not exist!\nPlease check connection string.");
                Console.WriteLine($"Error: {ex.Message}");
            }
            return isDatabaseExist;
        }

        public void ApplyMigration()
        {
            try
            {
                if (DataContext.DbContext.Database.GetPendingMigrations().Any())
                {
                    Console.WriteLine("DB migration started....");
                    DataContext.DbContext.Database.Migrate();

                    Console.WriteLine("DB migration successfully completed.");
                }
                else
                {
                    Console.WriteLine("DB migration: Not needed. Already up to date...");
                }
            }
            catch (Exception ex) 
            {
            
            }
        }
    }
}
