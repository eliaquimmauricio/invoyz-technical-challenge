using Invoyz.Invoices.Domain.Dtos.Products;
using Invoyz.Invoices.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Invoyz.Invoices.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ProductReadDto>> GetAll()
        {
            return await productService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ProductReadDto?> GetById(Guid id)
        {
            return await productService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ProductReadDto> Create([FromBody] ProductWriteDto dto)
        {
            return await productService.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] ProductWriteDto dto)
        {
            await productService.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await productService.DeleteAsync(id);
        }
    }
}
