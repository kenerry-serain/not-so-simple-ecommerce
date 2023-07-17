using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Repositories.Contracts;

namespace NotSoSimpleEcommerce.Repositories.Implementations
{
    public class DeleteEntityRepository<TEntity> : IDeleteEntityRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly DbContext _databaseContext;

        public DeleteEntityRepository(DbContext databaseContext)
        {
            _dbSet = databaseContext.Set<TEntity>();
            _databaseContext = databaseContext;
        }

        public async Task ExecuteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
