using Invoyz.Invoices.Domain.Dtos.Products;

namespace Invoyz.Invoices.Domain.Entities
{
    public class Product : BaseEntity<ProductReadDto, ProductWriteDto>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TaxRate { get; set; }

        public override ProductReadDto ToReadDto()
        {
            return new ProductReadDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                UnitPrice = UnitPrice,
                TaxRate = TaxRate,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }

        public override void UpdateFromWriteDto(ProductWriteDto dto)
        {
            Name = dto.Name;
            Description = dto.Description;
            UnitPrice = dto.UnitPrice;
            TaxRate = dto.TaxRate;
        }

        public static Product FromWriteDto(ProductWriteDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                UnitPrice = dto.UnitPrice,
                TaxRate = dto.TaxRate
            };
        }
    }
}
