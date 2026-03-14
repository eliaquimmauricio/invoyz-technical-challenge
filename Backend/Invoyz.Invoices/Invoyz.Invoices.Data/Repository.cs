using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Invoyz.Invoices.Data
{
    public class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : EntityBase
    {
        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await context.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
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
