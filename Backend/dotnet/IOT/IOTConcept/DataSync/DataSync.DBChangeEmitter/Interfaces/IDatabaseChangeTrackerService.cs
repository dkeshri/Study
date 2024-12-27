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
    }
}
