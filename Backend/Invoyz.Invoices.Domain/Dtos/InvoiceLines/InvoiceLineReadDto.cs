using Invoyz.Invoices.Domain.Dtos.Products;

namespace Invoyz.Invoices.Domain.Dtos.InvoiceLines
{
    public class InvoiceLineReadDto
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineTax { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ProductReadDto? Product { get; set; }
    }
}
