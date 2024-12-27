using DataSync.Common.Data.Entities;
using DataSync.Common.Interfaces.DataContext;
using DataSync.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Repositories
{
    internal class ChangeTrackerRepository : IChangeTrackerRepository
    {
        private IDataContext DataContext { get; }
        public DbSet<ChangeTracker> ChangeTrackers => this.DataContext.DbContext.Set<ChangeTracker>();
        public ChangeTrackerRepository(IDataContext dataContext)
        {
            DataContext = dataContext;
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
    }
}
