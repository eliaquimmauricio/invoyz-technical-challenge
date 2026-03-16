using Invoyz.Invoices.Domain.Entities;

namespace Invoyz.Invoices.Domain.Interfaces.Data
{
    public interface IRepository<TEntity, TReadDto, TWriteDto> 
        where TEntity : BaseEntity<TReadDto, TWriteDto> 
        where TReadDto : class 
        where TWriteDto : class
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
