using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Main.Domain.Repositories.Configurations;
using NotSoSimpleEcommerce.Shared.Models;
using NotSoSimpleEcommerce.Shared.Repositories.Configurations;

namespace NotSoSimpleEcommerce.Main.Domain.Repositories.Contexts
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options): base(options) { }

        public DbSet<StockEntity> Stock { get; set; } = null!;
        public DbSet<ProductEntity> Product { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockEntityTypeConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
