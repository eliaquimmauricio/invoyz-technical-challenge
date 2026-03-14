namespace Invoyz.Invoices.Domain.Interfaces.Services
{
    public interface IBaseService<TReadDto, TWriteDto>
    {
        Task<IEnumerable<TReadDto>> GetAllAsync();
        Task<TReadDto?> GetByIdAsync(Guid id);
        Task<TReadDto> CreateAsync(TWriteDto dto);
        Task UpdateAsync(Guid id, TWriteDto dto);
        Task DeleteAsync(Guid id);
    }
}
