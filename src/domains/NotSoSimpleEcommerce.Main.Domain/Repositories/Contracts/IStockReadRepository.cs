using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Main.Domain.Repositories.Contracts;

public interface IStockReadRepository: IReadEntityRepository<StockEntity>
{
    Task<StockEntity?> GetByProductIdAsync(int productId, CancellationToken cancellationToken);
}
