using Contract.Data.Context;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Entities;
namespace OrderService.Data
{
    public class OrderDbContext : DbContextBase
    {
        public OrderDbContext(IConfiguration configuration):base(configuration)
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

        public virtual DbSet<Order> Orders { get; set; }
    }
}
