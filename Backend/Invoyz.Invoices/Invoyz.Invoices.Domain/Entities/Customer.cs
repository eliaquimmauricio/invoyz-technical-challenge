namespace Invoyz.Invoices.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public required string Name { get; set; }
        public required string VatNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }        
        public ICollection<Invoice>? Invoices { get; set; }
    }
}