using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class EventFormatRepository : Repository<EventFormat>, IEventFormatRepository
    {
        public EventFormatRepository(EventCatalogDbContext context) : base(context)
        {
        }
    }
}
