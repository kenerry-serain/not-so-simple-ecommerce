using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Main.Domain.Models;

namespace NotSoSimpleEcommerce.Main.Domain.Repositories.Contexts
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
        : base(options) { }

        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<StockEntity> Stock{ get; set; }
    }
}
