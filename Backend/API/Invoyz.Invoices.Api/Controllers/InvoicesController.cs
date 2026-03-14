using Invoyz.Invoices.Domain.Dtos.Invoices;

namespace Invoyz.Invoices.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController(IInvoiceService invoiceService) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<InvoiceReadDto>> GetAll()
        {
            return await invoiceService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<InvoiceReadDto?> GetById(Guid id)
        {
            return await invoiceService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<InvoiceReadDto> Create([FromBody] InvoiceWriteDto dto)
        {
            return await invoiceService.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] InvoiceWriteDto dto)
        {
            await invoiceService.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await invoiceService.DeleteAsync(id);
        }
    }
}
