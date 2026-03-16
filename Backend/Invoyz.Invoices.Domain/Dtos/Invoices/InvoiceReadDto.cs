using Invoyz.Invoices.Domain.Dtos.Customers;
using Invoyz.Invoices.Domain.Dtos.InvoiceLines;
using Invoyz.Invoices.Domain.Enums;

namespace Invoyz.Invoices.Domain.Dtos.Invoices
{
    public class InvoiceReadDto
    {
        public Guid Id { get; set; }
        public required string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalTax { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CustomerReadDto? Customer { get; set; }
        public ICollection<InvoiceLineReadDto>? Lines { get; set; }
    }
}
