using Dkeshri.DataSync.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dkeshri.DataSync.Common.Interfaces.DataContext
{
    public abstract class DataContextBase : DbContext, IDataContext
    {
        private bool _disposed = false;
        protected DbConfig Configuration { get; }
        public DataContextBase(DbConfig configuration)
        {
            this.Configuration = configuration;
        }
        public DbContext DbContext => this;
        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        public abstract string GetConnectionString();
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                base.Dispose();
            }
            _disposed = true;
        }
    }
}
