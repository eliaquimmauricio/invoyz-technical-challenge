using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Invoyz.Invoices.Data
{
    public class Repository<TEntity, TReadDto, TWriteDto>(DbContext context) : IRepository<TEntity, TReadDto, TWriteDto> 
        where TEntity : BaseEntity<TReadDto, TWriteDto> 
        where TReadDto : class 
        where TWriteDto : class
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();

            query = include(query);

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();

            query = include(query);

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task AddAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            await context.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;

            context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            context.Set<TEntity>().UpdateRange(entities);
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
