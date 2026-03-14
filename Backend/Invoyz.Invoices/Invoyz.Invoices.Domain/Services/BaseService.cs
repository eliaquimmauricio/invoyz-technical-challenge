using Invoyz.Invoices.Domain.Interfaces.Services;

namespace Invoyz.Invoices.Domain.Services
{
    public abstract class BaseService : IBaseService
    {
        public abstract Task<T> CreateAsync<T>(T entity);
        public abstract Task DeleteAsync(Guid id);
        public abstract Task<IEnumerable<T>> GetAllAsync<T>();
        public abstract Task<T?> GetByIdAsync<T>(Guid id);
        public abstract Task UpdateAsync<T>(T entity);
    }
}
