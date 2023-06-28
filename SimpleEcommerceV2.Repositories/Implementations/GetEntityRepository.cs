using Microsoft.EntityFrameworkCore;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Repositories.Implementations
{
    public class ReadEntityRepository<TEntity> : IReadEntityRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> DbSet;

        public ReadEntityRepository(DbContext databaseContext)
        {
            DbSet = databaseContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await DbSet.ToListAsync(cancellationToken);
            return entities;
        }

        public async Task<TEntity?> GetByIdAsync(int primaryKey, CancellationToken cancellationToken)
        {
            var entity = await DbSet.FindAsync(primaryKey, cancellationToken);
            return entity;
        }
    }
}
