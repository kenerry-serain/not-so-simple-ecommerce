using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Repositories.Contracts;

namespace NotSoSimpleEcommerce.Repositories.Implementations
{
    public class ReadEntityRepository<TEntity> : IReadEntityRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        public ReadEntityRepository(DbContext databaseContext)
        {
            databaseContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dbSet = databaseContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _dbSet.ToListAsync(cancellationToken);
            return entities;
        }

        public async Task<TEntity?> GetByIdAsync(int primaryKey, CancellationToken cancellationToken)
        {
            var entity = await _dbSet.FindAsync(primaryKey, cancellationToken);
            return entity;
        }
    }
}
