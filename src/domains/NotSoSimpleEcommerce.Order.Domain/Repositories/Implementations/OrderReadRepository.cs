using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Order.Domain.Repositories.Contexts;
using NotSoSimpleEcommerce.Order.Domain.Repositories.Contracts;
using NotSoSimpleEcommerce.Repositories.Implementations;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Domain.Repositories.Implementations;

public sealed class OrderReadRepository : ReadEntityRepository<OrderEntity>, IOrderReadRepository
{
    private readonly OrderContext _context;

    public OrderReadRepository(OrderContext context) :
        base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<OrderEntity?> GetByProductIdAsync(int productId, CancellationToken cancellationToken)
    {
        return await _context.Order
            .AsNoTracking()
            .Include(order => order.Product)
            .FirstOrDefaultAsync(order => order.ProductId ==productId, cancellationToken: cancellationToken);
    }
}
