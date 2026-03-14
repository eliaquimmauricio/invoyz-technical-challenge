namespace Invoyz.Invoices.Domain.Dtos.InvoiceLines
{
    public class InvoiceLineWriteDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
    }
}
