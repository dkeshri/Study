using DataSync.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Interfaces.DataContext
{
    public abstract class DataContextBase : DbContext, IDataContext
    {
        private bool _disposed = false;
        protected IConfiguration Configuration { get; }
        public DataContextBase(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.DefaultSchema = Configuration.GetDatabaseSchema();
        }
        public DbContext DbContext => this;
        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        public abstract string GetConnectionString();
        protected string DefaultSchema { get; }
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
