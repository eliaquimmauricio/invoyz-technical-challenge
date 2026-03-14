namespace Invoyz.Invoices.Domain.Dtos.Customers
{
    public class CustomerWriteDto
    {
        public required string Name { get; set; }
        public required string VatNumber { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
    }
}
