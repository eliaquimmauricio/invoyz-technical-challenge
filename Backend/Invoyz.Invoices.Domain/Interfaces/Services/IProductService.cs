using Invoyz.Invoices.Domain.Dtos.Products;

namespace Invoyz.Invoices.Domain.Interfaces.Services
{
    public interface IProductService : IBaseService<ProductReadDto, ProductWriteDto>
    {
    }
}
