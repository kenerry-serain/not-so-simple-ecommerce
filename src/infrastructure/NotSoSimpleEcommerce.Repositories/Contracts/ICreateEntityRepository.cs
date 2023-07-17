namespace NotSoSimpleEcommerce.Repositories.Contracts
{
    public interface ICreateEntityRepository<TEntity>
    {
        Task<TEntity> ExecuteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
