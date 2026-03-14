using Invoyz.Invoices.Domain.Dtos.Customers;
using Invoyz.Invoices.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Invoyz.Invoices.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<CustomerReadDto>> GetAll()
        {
            return await customerService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<CustomerReadDto?> GetById(Guid id)
        {
            return await customerService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<CustomerReadDto> Create([FromBody] CustomerWriteDto dto)
        {
            return await customerService.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] CustomerWriteDto dto)
        {
            await customerService.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await customerService.DeleteAsync(id);
        }
    }
}
