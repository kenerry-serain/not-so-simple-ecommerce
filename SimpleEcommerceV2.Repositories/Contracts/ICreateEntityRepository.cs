namespace SimpleEcommerceV2.Repositories.Contracts
{
    public interface ICreateEntityRepository<TEntity>
    {
        Task<TEntity> ExecuteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
