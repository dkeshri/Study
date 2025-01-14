using Dkeshri.DataSync.Common.Interfaces.Repositories;
using Dkeshri.DataSync.Common.Models;
using Dkeshri.DataSync.DbChangeReceiver.Interfaces;
using System.Text.Json;
namespace Dkeshri.DataSync.DbChangeReceiver.Services
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
                        if (tableRecord.Data == null)
                        {
                            Console.WriteLine($"Data is Null skipping insertOrUpdate Operation {tableRecord.Operation}");
                        }
                        var record = JsonSerializer.Deserialize<Dictionary<string, object>>(tableRecord.Data!);
                        ApplyDbChangeRepository.InsertUpdate(tableName, record!);
                        break;
                    case "D":
                        if (tableRecord.PkKeysWithValues == null)
                        {
                            Console.WriteLine($"PrimaryKeys is Null skipping Delete Operation {tableRecord.Operation}");
                        }
                        var peimaryKeysWithValues = JsonSerializer.Deserialize<Dictionary<string, object>>(tableRecord.PkKeysWithValues!);
                        ApplyDbChangeRepository.Delete(tableName, peimaryKeysWithValues!);
                        break;
                }
            
            }

        }
    }
}
