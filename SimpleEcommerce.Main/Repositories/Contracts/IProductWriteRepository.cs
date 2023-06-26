using SimpleEcommerce.Main.Models;

namespace SimpleEcommerce.Main.Repositories.Contracts
{
    internal interface IProductWriteRepository
    {
        Task<ProductEntity> AddAsync(ProductEntity product);
        Task<ProductEntity> UpdateAsync(ProductEntity product);
        Task DeleteAsync(int primaryKey);
    }
}
