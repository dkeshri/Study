using DataSync.Common.Interfaces.Repositories;
using DataSync.Common.Models;
using DataSync.DbChangeReceiver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Services
{
    internal class DbChangeApplyService : IDbChangeApplyService
    {
        private IChangeTrackerRepository ChangeTrackerRepository { get; set; }
        public DbChangeApplyService(IChangeTrackerRepository changeTrackerRepository)
        {
            ChangeTrackerRepository = changeTrackerRepository;
        }

        public void ApplyTableChanges(TableChanges tableChanges)
        {
            string tableName = tableChanges.TableName;
            IReadOnlyCollection<TableRecord> tableRecords = tableChanges.Records;

        }
    }
}
