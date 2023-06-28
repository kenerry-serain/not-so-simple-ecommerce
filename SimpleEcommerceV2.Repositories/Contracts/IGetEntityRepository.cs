namespace SimpleEcommerceV2.Repositories.Contracts
{
    public interface IReadEntityRepository<TEntity> 
        where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int primaryKey, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    }
}
