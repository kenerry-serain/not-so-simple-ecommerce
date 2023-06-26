using SimpleEcommerce.Main.Models;
using SimpleEcommerce.Main.Repositories.Contexts;
using SimpleEcommerce.Main.Repositories.Contracts;

namespace SimpleEcommerce.Main.Repositories.Implementations
{
    internal sealed class StockWriteRepository : IStockWriteRepository
    {
        private readonly ProductContext _context;
        public StockWriteRepository(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<StockEntity> AddAsync(StockEntity stock)
        {
            await _context.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<StockEntity> UpdateAsync(StockEntity stock)
        {
            _context.Update(stock);
            await _context.SaveChangesAsync();
            return stock;
        }
    }
}
