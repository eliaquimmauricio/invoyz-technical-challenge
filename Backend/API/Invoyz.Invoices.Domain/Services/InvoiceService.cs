using Invoyz.Invoices.Domain.Dtos.Invoices;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Interfaces.Services;
using Invoyz.Invoices.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Invoyz.Invoices.Domain.Services
{
    public class InvoiceService(
        IRepository<Invoice, InvoiceReadDto, InvoiceWriteDto> repository,
        IPdfGeneratorService pdfGeneratorService,
        IFileUtility fileUtility) : BaseService<Invoice, InvoiceReadDto, InvoiceWriteDto>(repository), IInvoiceService
    {
        protected override Invoice CreateEntityFromWriteDto(InvoiceWriteDto dto)
        {
            return Invoice.FromWriteDto(dto);
        }

        public override async Task<InvoiceReadDto?> GetByIdAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id, query => query.Include(i => i.Lines).ThenInclude(l => l.Product));

            return entity?.ToReadDto();
        }

        public override async Task<IEnumerable<InvoiceReadDto>> GetAllAsync()
        {
            var entities = await repository.GetAllAsync(query => query.Include(i => i.Lines).ThenInclude(l => l.Product));

            return entities.Select(e => e.ToReadDto());
        }

        public async Task GeneratePdfAndSendByEmail(Guid id)
        {
            var entity = await repository.GetByIdAsync(id, query => query.Include(i => i.Lines).ThenInclude(l => l.Product)) 
                ?? throw new KeyNotFoundException($"Invoice with id {id} not found.");

            byte[] pdf = await pdfGeneratorService.GenerateInvoicePdfAsync(entity);

            //TODO: Implement email sending logic here, using an IEmailService to send the PDF as an attachment to the customer associated with the invoice.

            fileUtility.SaveFile(pdf, $"Invoice_{id}.pdf"); // Saving the PDF just for demonstration purposes.
        }
    }
}
