using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SREES.Common.Models;
using SREES.Common.Repositories.Interfaces;

namespace SREES.Common.Repositories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        protected readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<int> CountAsync(bool useIsDeleted = false)
        {
            if(useIsDeleted) 
            {
                return await _context.Set<TEntity>().CountAsync();
            }
            else
            {
                return await _context.Set<TEntity>().Where(e => !e.IsDeleted).CountAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool useIsDeleted = false, params Expression<Func<TEntity, object>>[] includes)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool useIsDeleted = false, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> entities = _context.Set<TEntity>();
            foreach(var include in includes)
            {
                entities = entities.Include(include);
            }
            if (!useIsDeleted)
            {
                entities = entities.Where(e => !e.IsDeleted);
            }
            
            return await entities.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, bool useIsDeleted = false, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> entities = _context.Set<TEntity>();
            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }
            if (!useIsDeleted)
            {
                entities = entities.Where(e => !e.IsDeleted);
            }
            return await entities.FirstOrDefaultAsync();
        }

        public void RemoveEntity(TEntity entity)
        {
            entity.IsDeleted = true;
        }

        public void RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
        }
    }
}
