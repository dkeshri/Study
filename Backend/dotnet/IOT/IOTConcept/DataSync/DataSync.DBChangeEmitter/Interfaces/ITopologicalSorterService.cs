using DataSync.Common.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Interfaces
{
    internal interface ITopologicalSorterService
    {
        public IReadOnlyCollection<ChangeTracker> TopologicalSort(IReadOnlyCollection<ChangeTracker> changeTrackers);
    }
}
