using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Order.Domain.Repositories.Configurations;
using NotSoSimpleEcommerce.Shared.Enums;
using NotSoSimpleEcommerce.Shared.Models;
using NotSoSimpleEcommerce.Shared.Repositories.Configurations;

namespace NotSoSimpleEcommerce.Order.Domain.Repositories.Contexts
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }
        public DbSet<OrderEntity> Order { get; set; } = null!;
        public DbSet<ProductEntity> Product { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityTypeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);

            modelBuilder.Entity<StatusEntity>().HasData(
                new StatusEntity
                (
                    OrderStatus.Pendente,
                    Enum.GetName(OrderStatus.Pendente)!
                ),
                new StatusEntity
                (
                    OrderStatus.Confirmada,
                    Enum.GetName(OrderStatus.Confirmada)!
                )
            );
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
