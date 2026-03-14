using Invoyz.Invoices.Domain.Dtos.Invoices;
using Invoyz.Invoices.Domain.Enums;

namespace Invoyz.Invoices.Domain.Entities
{
    public class Invoice : BaseEntity<InvoiceReadDto, InvoiceWriteDto>
    {
        public required string InvoiceNumber { get; set; }
        public required Guid CustomerId { get; set; }
        public required DateTime IssueDate { get; set; }
        public required DateTime DueDate { get; set; }
        public required InvoiceStatus Status { get; set; }
        public decimal SubTotal => Lines?.Sum(l => l.LineTotal) ?? 0;
        public decimal TotalTax => Lines?.Sum(l => l.LineTax) ?? 0;
        public decimal GrandTotal => SubTotal + TotalTax;
        public required Customer Customer { get; set; }
        public required ICollection<InvoiceLine> Lines { get; set; }

        public override InvoiceReadDto ToReadDto()
        {
            return new InvoiceReadDto
            {
                Id = Id,
                InvoiceNumber = InvoiceNumber,
                CustomerId = CustomerId,
                IssueDate = IssueDate,
                DueDate = DueDate,
                Status = Status,
                SubTotal = SubTotal,
                TotalTax = TotalTax,
                GrandTotal = GrandTotal,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Customer = Customer?.ToReadDto(),
                Lines = Lines?.Select(l => l.ToReadDto()).ToList()
            };
        }

        public override void UpdateFromWriteDto(InvoiceWriteDto dto)
        {
            InvoiceNumber = dto.InvoiceNumber;
            CustomerId = dto.CustomerId;
            IssueDate = dto.IssueDate;
            DueDate = dto.DueDate;
            Status = dto.Status;
        }

        public static Invoice FromWriteDto(InvoiceWriteDto dto)
        {
            var lines = dto.Lines?.Select(l => InvoiceLine.FromWriteDto(l, Guid.Empty)).ToList() ?? [];
            
            return new Invoice
            {
                InvoiceNumber = dto.InvoiceNumber,
                CustomerId = dto.CustomerId,
                IssueDate = dto.IssueDate,
                DueDate = dto.DueDate,
                Status = dto.Status,
                Customer = null!,
                Lines = lines
            };
        }
    }
}
