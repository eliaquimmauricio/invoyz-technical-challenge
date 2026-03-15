using System.Linq.Expressions;

namespace Invoyz.Invoices.Domain.Interfaces.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<TEntity?> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
        Task AddAsync(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        Task SaveChangesAsync();        
    }
}
