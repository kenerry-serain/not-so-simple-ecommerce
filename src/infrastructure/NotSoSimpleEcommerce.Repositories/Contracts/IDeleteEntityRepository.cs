namespace NotSoSimpleEcommerce.Repositories.Contracts
{
    public interface IDeleteEntityRepository<in TEntity>
        where TEntity : class
    {
        Task ExecuteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
