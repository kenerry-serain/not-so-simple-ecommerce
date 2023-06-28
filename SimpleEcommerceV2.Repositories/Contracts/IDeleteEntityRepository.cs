namespace SimpleEcommerceV2.Repositories.Contracts
{
    public interface IDeleteEntityRepository<TEntity>
        where TEntity : class
    {
        Task ExecuteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
