using NotSoSimpleEcommerce.Main.Domain.Models;
using NotSoSimpleEcommerce.Repositories.Contracts;

namespace NotSoSimpleEcommerce.Main.Domain.Repositories.Contracts;

public interface IStockReadRepository: IReadEntityRepository<StockEntity>
{
    Task<StockEntity?> GetByProductIdAsync(int productId, CancellationToken cancellationToken);
}
