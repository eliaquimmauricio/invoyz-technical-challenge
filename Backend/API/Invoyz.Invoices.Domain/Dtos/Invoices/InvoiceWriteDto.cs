using Invoyz.Invoices.Domain.Dtos.InvoiceLines;
using Invoyz.Invoices.Domain.Enums;

namespace Invoyz.Invoices.Domain.Dtos.Invoices
{
    public class InvoiceWriteDto
    {
        public required string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
        public ICollection<InvoiceLineWriteDto>? Lines { get; set; }
    }
}
