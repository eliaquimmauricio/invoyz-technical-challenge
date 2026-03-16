using Invoyz.Invoices.Domain.Dtos.InvoiceLines;

namespace Invoyz.Invoices.Domain.Interfaces.Services
{
    public interface IInvoiceLineService : IBaseService<InvoiceLineReadDto, InvoiceLineWriteDto>
    {
        Task<IEnumerable<InvoiceLineReadDto>> GetByInvoiceIdAsync(Guid invoiceId);
    }
}
