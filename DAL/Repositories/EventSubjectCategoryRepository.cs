using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EventSubjectCategoryRepository : Repository<EventSubjectCategory>, IEventSubjectCategoryRepository
    {
        public EventSubjectCategoryRepository(EventCatalogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EventSubjectCategory>> GetAllWithDetailsAsync()
        {
            return await context.EventSubjectCategories.Include(category => category.Events).ToListAsync();
        }

        public async Task<EventSubjectCategory> GetByIdWithDetailsAsync(int id)
        {
            return await context.EventSubjectCategories.Include(category => category.Events).SingleOrDefaultAsync(category => category.Id == id);
        }
    }
}
