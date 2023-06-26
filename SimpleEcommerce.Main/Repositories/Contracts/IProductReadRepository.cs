using SimpleEcommerce.Main.Models;

namespace SimpleEcommerce.Main.Repositories.Contracts
{
    public interface IProductReadRepository
    {
        Task<IEnumerable<ProductEntity>> GetAllAsync();
        Task<ProductEntity?> GetByIdAsync(int primaryKey);
    }
}
