using Invoyz.Invoices.Domain.Entities;

namespace Invoyz.Invoices.Domain.Interfaces.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
    }
}
