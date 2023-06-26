using SimpleEcommerce.Order.Models;
using SimpleEcommerce.Order.Repositories.Contexts;
using SimpleEcommerce.Order.Repositories.Contracts;

namespace SimpleEcommerce.Order.Repositories.Implementations
{
    internal sealed class OrderWriteRepository : IOrderWriteRepository
    {
        private readonly OrderContext _context;
        public OrderWriteRepository(OrderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<OrderEntity> AddAsync(OrderEntity order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
