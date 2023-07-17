using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Order.Domain.Models;

namespace NotSoSimpleEcommerce.Order.Domain.Repositories.Contexts
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
        : base(options) { }

        public DbSet<OrderEntity> Order { get; set; }
    }
}
