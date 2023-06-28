using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.Main.Domain.Models;
using SimpleEcommerceV2.Main.Domain.Repositories.Contexts;
using SimpleEcommerceV2.Main.Domain.Repositories.Contracts;
using SimpleEcommerceV2.Repositories.Implementations;

namespace SimpleEcommerceV2.Main.Domain.Repositories.Implementations;

public sealed class StockReadRepository: ReadEntityRepository<StockEntity>, IStockReadRepository
{
    private readonly ProductContext _context;
    public StockReadRepository(ProductContext context):
        base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<StockEntity?> GetByProductIdAsync(int productId)
    {
        return await _context.Stock
            .AsNoTracking()
            .FirstOrDefaultAsync(stock => stock.ProductId == productId);
    }
}
