using Invoyz.Invoices.Domain.Dtos.Customers;

namespace Invoyz.Invoices.Domain.Interfaces.Services
{
    public interface ICustomerService : IBaseService<CustomerReadDto, CustomerWriteDto>
    {
    }
}
