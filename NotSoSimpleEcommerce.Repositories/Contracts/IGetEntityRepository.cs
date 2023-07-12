namespace NotSoSimpleEcommerce.Repositories.Contracts
{
    public interface IReadEntityRepository<TEntity> 
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity?> GetByIdAsync(int primaryKey, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    }
}
