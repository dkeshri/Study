using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Contract.Data.Context
{
    public abstract class DbContextBase : DbContext, IDataContext
    {
        private bool _disposed = false;
        protected IConfiguration Configuration { get; }

        public DbContext DbContext => this;

        public DbContextBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public override void Dispose()
        {
            Dispose(true);
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
