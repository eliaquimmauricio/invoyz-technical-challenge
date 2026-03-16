using Invoyz.Invoices.Domain.Entities;

namespace Invoyz.Invoices.Domain.Interfaces.Services
{
    public interface IPdfGeneratorService
    {
        Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice);
    }
}
