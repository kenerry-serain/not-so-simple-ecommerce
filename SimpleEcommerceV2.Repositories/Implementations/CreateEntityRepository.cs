using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Repositories.Implementations
{
    public class CreateEntityRepository<TEntity> : ICreateEntityRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> DbSet;
        private readonly DbContext DatabaseContext;

        public CreateEntityRepository(DbContext databaseContext)
        {
            DbSet = databaseContext.Set<TEntity>();
            DatabaseContext = databaseContext;
        }

        public async Task<TEntity> ExecuteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            await DatabaseContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}
