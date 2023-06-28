namespace SimpleEcommerceV2.Repositories.Contracts
{
    public interface IUpdateEntityRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> ExecuteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
