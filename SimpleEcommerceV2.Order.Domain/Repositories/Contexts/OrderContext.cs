using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.Order.Domain.Models;

namespace SimpleEcommerceV2.Order.Domain.Repositories.Contexts
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
        : base(options) { }

        public DbSet<OrderEntity> Order { get; set; }
    }
}
