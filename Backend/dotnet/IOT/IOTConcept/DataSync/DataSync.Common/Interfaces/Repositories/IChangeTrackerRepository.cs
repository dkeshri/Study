using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Interfaces.Repositories
{
    public interface IChangeTrackerRepository
    {
        public long GetTableChangeVersion(string tableName);
    }
}
