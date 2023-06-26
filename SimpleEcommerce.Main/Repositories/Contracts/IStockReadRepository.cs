using SimpleEcommerce.Main.Models;

namespace SimpleEcommerce.Main.Repositories.Contracts;

public interface IStockReadRepository
{
    Task<IEnumerable<StockEntity>> GetAllAsync();
    Task<StockEntity?> GetByProductIdAsync(int productId);
}
