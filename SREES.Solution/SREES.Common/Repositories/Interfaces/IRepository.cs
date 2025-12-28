using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SREES.Common.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id, bool useIsDeleted = false, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllAsync(bool useIsDeleted = false, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool useIsDeleted = false, params Expression<Func<TEntity, object>>[] includes);
        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void RemoveEntity(TEntity entity);  
        void RemoveRangeAsync(IEnumerable<TEntity> entities);
        Task<int> CountAsync(bool useIsDeleted = false);
    }
}
