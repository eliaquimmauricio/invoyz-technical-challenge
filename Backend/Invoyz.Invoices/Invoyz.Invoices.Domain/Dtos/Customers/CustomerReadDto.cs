namespace Invoyz.Invoices.Domain.Dtos.Customers
{
    public class CustomerReadDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string VatNumber { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
