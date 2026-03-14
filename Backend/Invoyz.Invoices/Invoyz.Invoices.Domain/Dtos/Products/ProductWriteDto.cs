namespace Invoyz.Invoices.Domain.Dtos.Products
{
    public class ProductWriteDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
    }
}
