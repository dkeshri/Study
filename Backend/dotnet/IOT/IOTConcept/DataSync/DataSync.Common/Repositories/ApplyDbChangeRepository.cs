using DataSync.Common.Interfaces.DataContext;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace DataSync.Common.Repositories
{
    internal class ApplyDbChangeRepository : Repository, IApplyDbChangeRepository
    {
        public ApplyDbChangeRepository(IDataContext dataContext) : base(dataContext) {}

        public void InsertUpdate(string tableName, Dictionary<string, object> record)
        {
            try
            {
                var primaryKeys = GetPrimaryKeys(tableName);
                string condition = string.Join(" AND ", primaryKeys.Select(x => $"T.{x} = {record[x]}"));
                string checkQuery = $"SELECT COUNT(1) As [Value] FROM [{tableName}] T WHERE {condition}";
                bool isRecordExist = IsRecordExist(checkQuery);
                if (isRecordExist) {
                    string updateQuery = BuildUpdateQuery(record, tableName, "Id");
                    DataContext.DbContext.Database.ExecuteSqlRaw(updateQuery, GetSqlParameters(record));
                }
                else
                {
                    string insertQuery = BuildInsertQuery(record, tableName);
                    DataContext.DbContext.Database.ExecuteSqlRaw(insertQuery, GetSqlParameters(record));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
        public void Delete(string tableName, Dictionary<string, object> pkKeysWithValues)
        {
            string condition = string.Join(" AND ", pkKeysWithValues.Select(x => $"{x.Key} = {x.Value}"));
            try
            {
                
                string query = $@"
                    DELETE from [{tableName}] where {condition}";
                int row = DataContext.DbContext.Database.ExecuteSqlRaw(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : While deleting records! in {tableName} where {condition} ");
            }
        }
        private string BuildInsertQuery(Dictionary<string, object> record, string tableName)
        {
            var columns = string.Join(", ", record.Keys);

            var values = string.Join(", ", record.Keys.Select(key => "@" + key));
            return $@"IF EXISTS (
                        SELECT 1
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = '{tableName}'
                          AND COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1
                    )
                    BEGIN
                        SET IDENTITY_INSERT [{tableName}] ON;
                        INSERT INTO [{tableName}] ({columns}) VALUES ({values});
                        SET IDENTITY_INSERT [{tableName}] OFF;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO [{tableName}] ({columns}) VALUES ({values});
                    END";

        }
        private string BuildUpdateQuery(Dictionary<string, object> record, string tableName, string primaryKey)
        {
            var setClauses = record
            .Where(kvp => kvp.Key != primaryKey)
            .Select(kvp => $"{kvp.Key} = @{kvp.Key}");
            var setClause = string.Join(", ", setClauses);
            return $"UPDATE [{tableName}] SET {setClause} WHERE {primaryKey} = @{primaryKey}";
        }

        private object[] GetSqlParameters(Dictionary<string, object> record)
        {
             var test  = record
                .Select(kvp => new SqlParameter($"@{kvp.Key}", ConvertJsonElement(kvp.Value) ?? DBNull.Value))
                .ToArray();
            return test;
        }
        private object ConvertJsonElement(object value)
        {
            if (value is JsonElement jsonElement)
            {
                // Handle different JSON element types
                return jsonElement.ValueKind switch
                {
                    JsonValueKind.String => jsonElement.GetString(),
                    JsonValueKind.Number => jsonElement.TryGetInt64(out long l) ? l : jsonElement.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    JsonValueKind.Null => DBNull.Value,
                    _ => jsonElement.GetRawText() // Fallback: return raw JSON text
                };
            }

            return value ?? DBNull.Value; // Return the original value or DBNull for nulls
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
