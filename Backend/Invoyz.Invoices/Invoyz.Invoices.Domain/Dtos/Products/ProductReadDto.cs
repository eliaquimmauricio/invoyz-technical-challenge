namespace Invoyz.Invoices.Domain.Dtos.Products
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
