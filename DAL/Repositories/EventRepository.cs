using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(EventCatalogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetAllWithDetailsAsync()
        {
            return await context.Events.Include(e => e.Speaker).Include(e => e.Participants).Include(e => e.EventFormat).Include(e => e.EventSubjectCategory).ToListAsync();
        }

        public async Task<Event> GetByIdWithDetailsAsync(int id)
        {
            return await context.Events.Include(e => e.Speaker).Include(e => e.Participants).Include(e => e.EventFormat).Include(e => e.EventSubjectCategory).SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}
