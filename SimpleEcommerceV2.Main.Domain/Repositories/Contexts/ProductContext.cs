using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.Main.Domain.Models;

namespace SimpleEcommerceV2.Main.Domain.Repositories.Contexts
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
        : base(options) { }

        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<StockEntity> Stock{ get; set; }
    }
}
