using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestrator.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestrator.Data
{
    internal class AppDbContext : SagaDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override IEnumerable<ISagaClassMap> Configurations => new List<ISagaClassMap>
        {
            new OrderStateMap()
        };

    }

    public class OrderStateMap : SagaClassMap<OrderState>
    {
        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
        {

            // Primary Key
            entity.HasKey(x => x.CorrelationId);

            // Map CurrentState with a max length constraint
            entity.Property(x => x.CurrentState)
                  .HasMaxLength(50)
                  .IsRequired();

            // Map OrderId as a Guid
            entity.Property(x => x.OrderId)
                  .IsRequired();

            // Map Amount as a decimal with precision
            entity.Property(x => x.Amount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();
        }
    }
}
