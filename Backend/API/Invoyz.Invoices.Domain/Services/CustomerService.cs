using Invoyz.Invoices.Domain.Dtos.Customers;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Interfaces.Services;

namespace Invoyz.Invoices.Domain.Services
{
    public class CustomerService(IRepository<Customer, CustomerReadDto, CustomerWriteDto> repository) : BaseService<Customer, CustomerReadDto, CustomerWriteDto>(repository), ICustomerService
    {
        protected override Customer CreateEntityFromWriteDto(CustomerWriteDto dto)
        {
            return Customer.FromWriteDto(dto);
        }
    }
}
