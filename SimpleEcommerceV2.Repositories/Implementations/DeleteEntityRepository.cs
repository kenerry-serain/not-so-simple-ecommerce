using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Repositories.Implementations
{
    public class DeleteEntityRepository<TEntity> : IDeleteEntityRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> DbSet;
        private readonly DbContext DatabaseContext;

        public DeleteEntityRepository(DbContext databaseContext)
        {
            DbSet = databaseContext.Set<TEntity>();
            DatabaseContext = databaseContext;
        }

        public async Task ExecuteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            DbSet.Remove(entity);
            await DatabaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
