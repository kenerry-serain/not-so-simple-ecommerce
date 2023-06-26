using SimpleEcommerce.Main.Models;

namespace SimpleEcommerce.Main.Repositories.Contracts
{
    internal interface IStockWriteRepository
    {
        Task<StockEntity> AddAsync(StockEntity stock);
        Task<StockEntity> UpdateAsync(StockEntity stock);
    }
}
