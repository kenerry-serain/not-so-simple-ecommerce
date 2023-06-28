using SimpleEcommerceV2.Main.Domain.Models;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Main.Domain.Repositories.Contracts;

public interface IStockReadRepository: IReadEntityRepository<StockEntity>
{
    Task<StockEntity?> GetByProductIdAsync(int productId);
}
