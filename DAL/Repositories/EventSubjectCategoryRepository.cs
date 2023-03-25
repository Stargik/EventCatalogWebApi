using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class EventSubjectCategoryRepository : Repository<EventSubjectCategory>, IEventSubjectCategoryRepository
    {
        public EventSubjectCategoryRepository(EventCatalogDbContext context) : base(context)
        {
        }
    }
}
