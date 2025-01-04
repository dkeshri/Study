﻿using DataSync.Common.Interfaces.DataContext;
using DataSync.Common.Interfaces.Repositories;
using DataSync.Common.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Repositories
{
    internal class ApplyDbChangeRepository : IApplyDbChangeRepository
    {
        private IDataContext DataContext { get; }
        public ApplyDbChangeRepository(IDataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void InsertUpdate(string tableName, Dictionary<string, object> record)
        {
            try
            {
                var primaryKeys = GetPrimaryKeys(tableName);
                string condition = string.Join(" AND ", primaryKeys.Select(x => $"T.{x} = {record[x]}"));
                string checkQuery = $"SELECT COUNT(1) As [Value] FROM [{tableName}] T WHERE {condition}";
                bool isRecordExist = IsRecordExist(checkQuery);
                if (isRecordExist) {
                    // update Operstion
                    string updateQuery = BuildUpdateQuery(record, tableName, "Id");
                    DataContext.DbContext.Database.ExecuteSqlRaw(updateQuery, GetSqlParameters(record, primaryKeys.ToArray()));
                }
                else
                {
                    string insertQuery = BuildInsertQuery(record, tableName);
                    DataContext.DbContext.Database.ExecuteSqlRaw(insertQuery, GetSqlParameters(record));
                }
            }
            catch (Exception ex)
            { }

        }

        // Build Insert Query
        private string BuildInsertQuery(Dictionary<string, object> record, string tableName)
        {
            var columns = string.Join(", ", record.Keys);

            var values = string.Join(", ", record.Keys.Select(key => "@" + key));
            return $"INSERT INTO [{tableName}] ({columns}) VALUES ({values})";
        }

        // Build Update Query
        private string BuildUpdateQuery(Dictionary<string, object> record, string tableName, string primaryKey)
        {
            var setClauses = record
            .Where(kvp => kvp.Key != primaryKey)
            .Select(kvp => $"{kvp.Key} = @{kvp.Key}");
            var setClause = string.Join(", ", setClauses);
            return $"UPDATE [{tableName}] SET {setClause} WHERE {primaryKey} = @{primaryKey}";
        }

        private object[] GetSqlParameters(Dictionary<string, object> record, string[] includeKeys = null)
        {
            return record
                .Where(kvp => includeKeys == null || includeKeys.Contains(kvp.Key))
                .Select(kvp => new Microsoft.Data.SqlClient.SqlParameter($"@{kvp.Key}", kvp.Value ?? DBNull.Value))
                .ToArray();
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
                            AND i.object_id = OBJECT_ID('[{tableName}]');";
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
        private bool IsRecordExist(string query)
        {
            int recordExists = DataContext.DbContext.Database.SqlQueryRaw<int>(query).Single();
            if (recordExists > 0)
            {
                return true;
            }
            return false;
        }
    }
}
