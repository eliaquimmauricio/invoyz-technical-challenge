namespace Invoyz.Invoices.Domain.Interfaces.Services
{
    public interface IBaseService
    {
        public Task<IEnumerable<T>> GetAllAsync<T>();
        public Task<T?> GetByIdAsync<T>(Guid id);
        public Task<T> CreateAsync<T>(T entity);
        public Task UpdateAsync<T>(T entity);
        public Task DeleteAsync(Guid id);
    }
}
