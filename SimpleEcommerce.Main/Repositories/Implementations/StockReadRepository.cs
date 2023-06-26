using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Main.Models;
using SimpleEcommerce.Main.Repositories.Contexts;
using SimpleEcommerce.Main.Repositories.Contracts;

namespace SimpleEcommerce.Main.Repositories.Implementations;

internal sealed class StockReadRepository: IStockReadRepository
{
    private readonly ProductContext _context;
    public StockReadRepository(ProductContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<StockEntity>> GetAllAsync()
    {
        return await _context.Stock
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<StockEntity?> GetByProductIdAsync(int productId)
    {
        return await _context.Stock
            .AsNoTracking()
            .FirstOrDefaultAsync(stock => stock.ProductId == productId);
    }
}
