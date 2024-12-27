using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Interfaces
{
    internal interface IDatabaseChangeTrackerService
    {
        public long GetTableChangeVersion(string tableName);
        public long GetDbChangeTrackingCurrentVersion();
        public Task<long> GetDbChangeTrackingCurrentVersionAsync();
        public List<string> GetPrimaryKeys(string tableName);

    }
}
