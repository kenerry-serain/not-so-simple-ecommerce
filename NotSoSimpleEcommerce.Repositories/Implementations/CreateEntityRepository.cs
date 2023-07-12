using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Repositories.Contracts;

namespace NotSoSimpleEcommerce.Repositories.Implementations
{
    public class CreateEntityRepository<TEntity> : ICreateEntityRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly DbContext _databaseContext;

        public CreateEntityRepository(DbContext databaseContext)
        {
            _dbSet = databaseContext.Set<TEntity>();
            _databaseContext = databaseContext;
        }

        public async Task<TEntity> ExecuteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}
