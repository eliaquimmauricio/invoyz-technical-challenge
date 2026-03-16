using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Interfaces.Services;

namespace Invoyz.Invoices.Domain.Services
{
    public abstract class BaseService<TEntity, TReadDto, TWriteDto>(IRepository<TEntity, TReadDto, TWriteDto> repository) : IBaseService<TReadDto, TWriteDto> 
        where TEntity : BaseEntity<TReadDto, TWriteDto>
        where TReadDto : class
        where TWriteDto : class
    {
        protected readonly IRepository<TEntity, TReadDto, TWriteDto> repository = repository;

        protected abstract TEntity CreateEntityFromWriteDto(TWriteDto dto);

        public virtual async Task<IEnumerable<TReadDto>> GetAllAsync()
        {
            var entities = await repository.GetAllAsync();

            return entities.Select(e => e.ToReadDto());
        }

        public virtual async Task<TReadDto?> GetByIdAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id);

            return entity?.ToReadDto();
        }

        public virtual async Task<TReadDto> CreateAsync(TWriteDto dto)
        {
            var entity = CreateEntityFromWriteDto(dto);

            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();
            return entity.ToReadDto();
        }

        public virtual async Task UpdateAsync(Guid id, TWriteDto dto)
        {
            var entity = await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");

            entity.UpdateFromWriteDto(dto);
            repository.Update(entity);
            await repository.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");

            repository.Delete(entity);
            await repository.SaveChangesAsync();
        }
    }
}
