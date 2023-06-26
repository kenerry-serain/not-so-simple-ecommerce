using SimpleEcommerce.Main.Models;
using SimpleEcommerce.Main.Repositories.Contexts;
using SimpleEcommerce.Main.Repositories.Contracts;

namespace SimpleEcommerce.Main.Repositories.Implementations
{
    internal sealed class ProductWriteRepository: IProductWriteRepository
    {
        private readonly ProductContext _context;
        private readonly IProductReadRepository _productReadRepository;
        public ProductWriteRepository(ProductContext context, IProductReadRepository productReadRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _productReadRepository = productReadRepository ?? throw new ArgumentNullException(nameof(productReadRepository)); 
        }

        public async Task<ProductEntity> AddAsync(ProductEntity product)
        {
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<ProductEntity> UpdateAsync(ProductEntity product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int primaryKey)
        {
            var product = await _productReadRepository.GetByIdAsync(primaryKey);
            if (product is not null)
            {
                _context.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
