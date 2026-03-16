using Invoyz.Invoices.Domain.Dtos.Invoices;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Invoyz.Invoices.Domain.Services
{
    public class InvoiceService(IRepository<Invoice, InvoiceReadDto, InvoiceWriteDto> repository) : BaseService<Invoice, InvoiceReadDto, InvoiceWriteDto>(repository), IInvoiceService
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
    }
}
