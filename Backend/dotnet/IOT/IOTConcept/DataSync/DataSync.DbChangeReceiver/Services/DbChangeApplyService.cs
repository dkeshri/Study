using DataSync.Common.Interfaces.Repositories;
using DataSync.Common.Models;
using DataSync.DbChangeReceiver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Services
{
    internal class DbChangeApplyService : IDbChangeApplyService
    {
        private IApplyDbChangeRepository ApplyDbChangeRepository { get; set; }
        public DbChangeApplyService(IApplyDbChangeRepository applyDbChangeRepository)
        {
            ApplyDbChangeRepository = applyDbChangeRepository;
        }

        public void ApplyTableChanges(TableChanges tableChanges)
        {
            string tableName = tableChanges.TableName;
            IReadOnlyCollection<TableRecord> tableRecords = tableChanges.Records;
            foreach (TableRecord tableRecord in tableRecords) {
                
                switch (tableRecord.Operation)
                {
                    
                    case "I":
                    case "U":
                        var record = JsonSerializer.Deserialize<Dictionary<string, object>>(tableRecord.Data?.ToString()!);
                        ApplyDbChangeRepository.InsertUpdate(tableName, record!);
                        break;
                    case "D":
                        ApplyDbChangeRepository.Delete(tableName, record!);
                        break;
                }
            
            }

        }
    }
}
