using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Domain.Repositories.Contracts;

public interface IOrderReadRepository: IReadEntityRepository<OrderEntity>
{
    Task<OrderEntity?> GetByProductIdAsync(int productId, CancellationToken cancellationToken);
}
