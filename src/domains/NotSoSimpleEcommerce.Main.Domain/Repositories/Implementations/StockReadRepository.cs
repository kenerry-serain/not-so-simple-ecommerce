using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Main.Domain.Repositories.Contexts;
using NotSoSimpleEcommerce.Main.Domain.Repositories.Contracts;
using NotSoSimpleEcommerce.Repositories.Implementations;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Main.Domain.Repositories.Implementations;

public sealed class StockReadRepository: ReadEntityRepository<StockEntity>, IStockReadRepository
{
    private readonly ProductContext _context;
    public StockReadRepository(ProductContext context):
        base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<StockEntity?> GetByProductIdAsync(int productId, CancellationToken cancellationToken)
    {
        return await _context.Stock
            .AsNoTracking()
            .Include(stock => stock.Product)
            .FirstOrDefaultAsync(stock => stock.ProductId == productId, cancellationToken);
    }
}
