using Invoyz.Invoices.Domain.Enums;

namespace Invoyz.Invoices.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public required string InvoiceNumber { get; set; }
        public required Guid CustomerId { get; set; }
        public required DateTime IssueDate { get; set; }
        public required DateTime DueDate { get; set; }
        public required InvoiceStatus Status { get; set; }
        public required decimal SubTotal { get; set; }
        public required decimal TotalTax { get; set; }
        public required decimal GrandTotal { get; set; }
        public required Customer Customer { get; set; }
        public required ICollection<InvoiceLine> Lines { get; set; }
    }
}
