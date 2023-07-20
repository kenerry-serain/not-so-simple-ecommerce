using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Order.Domain.Repositories.Configurations;
using NotSoSimpleEcommerce.Shared.Enums;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Domain.Repositories.Contexts
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityTypeConfiguration).Assembly);
            modelBuilder.Entity<StatusEntity>().HasData(
                new StatusEntity
                (
                    OrderStatus.Created,
                    Enum.GetName(OrderStatus.Created)!
                ),
                new StatusEntity
                (
                    OrderStatus.Confirmed,
                    Enum.GetName(OrderStatus.Confirmed)!
                )
            );
            
            base.OnModelCreating(modelBuilder);

        }
    }
}
