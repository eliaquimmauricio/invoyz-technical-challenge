using Invoyz.Invoices.Domain.Dtos.Customers;

namespace Invoyz.Invoices.Domain.Entities
{
    public class Customer : BaseEntity<CustomerReadDto, CustomerWriteDto>
    {
        public required string Name { get; set; }
        public required string VatNumber { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }        
        public ICollection<Invoice>? Invoices { get; set; }

        public override CustomerReadDto ToReadDto()
        {
            return new CustomerReadDto
            {
                Id = Id,
                Name = Name,
                VatNumber = VatNumber,
                Email = Email,
                Address = Address,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }

        public override void UpdateFromWriteDto(CustomerWriteDto dto)
        {
            Name = dto.Name;
            VatNumber = dto.VatNumber;
            Email = dto.Email;
            Address = dto.Address;
        }

        public static Customer FromWriteDto(CustomerWriteDto dto)
        {
            return new Customer
            {
                Name = dto.Name,
                VatNumber = dto.VatNumber,
                Email = dto.Email,
                Address = dto.Address
            };
        }
    }
}