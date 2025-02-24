using Contract.Data.Context;
using InventeryService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventeryService.Data
{
    public class InventoryDbContext : DbContextBase
    {
        public InventoryDbContext(IConfiguration configuration): base(configuration)
        {
            
        }

        public override string GetConnectionString()
        {
            return Configuration.GetConnectionString("DefaultConnection")!;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = GetConnectionString();

                optionsBuilder.UseSqlServer(connectionString,
                    opt => opt.CommandTimeout(60));

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Inventory> Inventories { get; set; }
    }
}
