using Invoyz.Invoices.Domain.Dtos.InvoiceLines;

namespace Invoyz.Invoices.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceLinesController(IInvoiceLineService invoiceLineService) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<InvoiceLineReadDto>> GetAll()
        {
            return await invoiceLineService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<InvoiceLineReadDto?> GetById(Guid id)
        {
            return await invoiceLineService.GetByIdAsync(id);
        }

        [HttpGet("invoice/{invoiceId}")]
        public async Task<IEnumerable<InvoiceLineReadDto>> GetByInvoiceId(Guid invoiceId)
        {
            return await invoiceLineService.GetByInvoiceIdAsync(invoiceId);
        }

        [HttpPost]
        public async Task<InvoiceLineReadDto> Create([FromBody] InvoiceLineWriteDto dto)
        {
            return await invoiceLineService.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] InvoiceLineWriteDto dto)
        {
            await invoiceLineService.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await invoiceLineService.DeleteAsync(id);
        }
    }
}
