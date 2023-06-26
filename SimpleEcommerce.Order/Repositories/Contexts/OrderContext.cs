using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Order.Models;

namespace SimpleEcommerce.Order.Repositories.Contexts
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
        : base(options) { }

        public DbSet<OrderEntity> Order { get; set; }
    }
}
