using Invoyz.Invoices.Domain.Dtos.Invoices;

namespace Invoyz.Invoices.Domain.Interfaces.Services
{
    public interface IInvoiceService : IBaseService<InvoiceReadDto, InvoiceWriteDto>
    {
    }
}
