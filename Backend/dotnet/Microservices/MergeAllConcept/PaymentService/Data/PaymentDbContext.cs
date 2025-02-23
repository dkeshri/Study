using Contract.Data.Context;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data.Entities;

namespace PaymentService.Data
{
    public class PaymentDbContext : DbContextBase
    {
        public PaymentDbContext(IConfiguration configuration):base(configuration)
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

        public virtual DbSet<Payment> Payments { get; set; }
    }
}
