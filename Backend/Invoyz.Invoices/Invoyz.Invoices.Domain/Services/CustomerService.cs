using Invoyz.Invoices.Domain.Interfaces.Services;

namespace Invoyz.Invoices.Domain.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        public override Task<T> CreateAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public override Task<T?> GetByIdAsync<T>(Guid id) where T : default
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
