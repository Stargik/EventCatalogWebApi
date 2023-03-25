using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class SpeakerRepository : Repository<Speaker>, ISpeakerRepository
    {
        public SpeakerRepository(EventCatalogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Speaker>> GetAllWithDetailsAsync()
        {
            return await context.Speakers.Include(speaker => speaker.Events).ToListAsync();
        }

        public async Task<Speaker> GetByIdWithDetailsAsync(int id)
        {
            return await context.Speakers.Include(speaker => speaker.Events).SingleOrDefaultAsync(speaker => speaker.Id == id);
        }
    }
}
