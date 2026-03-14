namespace Invoyz.Invoices.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TaxRate { get; set; }
    }
}
