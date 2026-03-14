namespace Invoyz.Invoices.Domain.Entities
{
    public class InvoiceLine : BaseEntity
    {
        public required Guid InvoiceId { get; set; }
        public required Guid ProductId { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TaxRate { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
        public decimal LineTax => LineTotal * (TaxRate / 100);
        public required Invoice Invoice { get; set; }
        public required Product Product { get; set; }
    }
}
