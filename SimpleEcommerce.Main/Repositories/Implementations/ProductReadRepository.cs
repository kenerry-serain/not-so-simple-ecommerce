using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Main.Models;
using SimpleEcommerce.Main.Repositories.Contexts;
using SimpleEcommerce.Main.Repositories.Contracts;

namespace SimpleEcommerce.Main.Repositories.Implementations
{
    internal sealed class ProductReadRepository: IProductReadRepository
    {
        private readonly ProductContext _context;
        public ProductReadRepository(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            return await _context.Product
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdAsync(int primaryKey)
        {
            return await _context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.Id == primaryKey);
        }
    }
}
