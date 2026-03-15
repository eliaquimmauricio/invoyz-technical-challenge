using Invoyz.Invoices.Domain.Dtos.Invoices;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Interfaces.Services;

namespace Invoyz.Invoices.Domain.Services
{
    public class InvoiceService(IRepository<Invoice> repository) : BaseService<Invoice, InvoiceReadDto, InvoiceWriteDto>(repository), IInvoiceService
    {
        protected override Invoice CreateEntityFromWriteDto(InvoiceWriteDto dto)
        {
            return Invoice.FromWriteDto(dto);
        }

        public override async Task<InvoiceReadDto?> GetByIdAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id, i => i.Lines);

            return entity?.ToReadDto();
        }
    }
}
