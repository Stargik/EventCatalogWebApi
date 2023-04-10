using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        protected readonly EventCatalogDbContext context;

        public Repository(EventCatalogDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity is not null)
            {
                context.Set<T>().Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            var oldEntity = await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (oldEntity is not null)
            {
                context.Set<T>().Update(entity);
            }
        }
    }
}
