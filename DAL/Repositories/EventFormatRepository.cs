using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class EventFormatRepository : Repository<EventFormat>, IEventFormatRepository
    {
        public EventFormatRepository(EventCatalogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EventFormat>> GetAllWithDetailsAsync()
        {
            return await context.EventFormats.ToListAsync();
        }

        public async Task<EventFormat> GetByIdWithDetailsAsync(int id)
        {
            return await context.EventFormats.SingleOrDefaultAsync(category => category.Id == id);
        }
    }
}
