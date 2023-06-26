using SimpleEcommerce.Order.Models;

namespace SimpleEcommerce.Order.Repositories.Contracts
{
    internal interface IOrderWriteRepository
    {
        Task<OrderEntity> AddAsync(OrderEntity order);
    }
}
