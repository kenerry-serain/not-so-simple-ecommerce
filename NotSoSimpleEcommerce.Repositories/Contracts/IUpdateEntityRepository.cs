namespace NotSoSimpleEcommerce.Repositories.Contracts
{
    public interface IUpdateEntityRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> ExecuteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
