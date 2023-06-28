using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Repositories.Implementations
{
    public class UpdateEntityRepository<TEntity> : IUpdateEntityRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> DbSet;
        private readonly DbContext DatabaseContext;

        public UpdateEntityRepository(DbContext databaseContext)
        {
            DbSet = databaseContext.Set<TEntity>();
            DatabaseContext = databaseContext;
        }

        public async Task<TEntity> ExecuteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            DbSet.Update(entity);
            await DatabaseContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}
