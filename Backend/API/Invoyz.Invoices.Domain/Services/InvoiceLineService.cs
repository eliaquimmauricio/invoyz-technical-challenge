using Invoyz.Invoices.Domain.Dtos.InvoiceLines;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Interfaces.Services;

namespace Invoyz.Invoices.Domain.Services
{
    public class InvoiceLineService(IRepository<InvoiceLine, InvoiceLineReadDto, InvoiceLineWriteDto> repository) : BaseService<InvoiceLine, InvoiceLineReadDto, InvoiceLineWriteDto>(repository), IInvoiceLineService
    {
        protected override InvoiceLine CreateEntityFromWriteDto(InvoiceLineWriteDto dto)
        {
            return InvoiceLine.FromWriteDto(dto, Guid.Empty);
        }

        public async Task<IEnumerable<InvoiceLineReadDto>> GetByInvoiceIdAsync(Guid invoiceId)
        {
            var allLines = await repository.GetAllAsync();
            var invoiceLines = allLines.Where(l => l.InvoiceId == invoiceId);
            return invoiceLines.Select(l => l.ToReadDto());
        }
    }
}
