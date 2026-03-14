using Invoyz.Invoices.Domain.Dtos.Products;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Interfaces.Services;

namespace Invoyz.Invoices.Domain.Services
{
    public class ProductService(IRepository<Product> repository) : BaseService<Product, ProductReadDto, ProductWriteDto>(repository), IProductService
    {
        protected override Product CreateEntityFromWriteDto(ProductWriteDto dto)
        {
            return Product.FromWriteDto(dto);
        }
    }
}
