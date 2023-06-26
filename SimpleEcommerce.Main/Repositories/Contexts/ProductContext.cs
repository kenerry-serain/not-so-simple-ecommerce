using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Main.Models;

namespace SimpleEcommerce.Main.Repositories.Contexts
{
    internal class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
        : base(options) { }

        internal DbSet<ProductEntity> Product { get; set; }
        internal DbSet<StockEntity> Stock{ get; set; }
    }
}
