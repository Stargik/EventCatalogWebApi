using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ParticipantRepository : Repository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(EventCatalogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Participant>> GetAllWithDetailsAsync()
        {
            return await context.Participants.AsNoTracking().Include(participant => participant.Events).ToListAsync();
        }

        public async Task<Participant> GetByIdWithDetailsAsync(int id)
        {
            return await context.Participants.Include(participant => participant.Events).SingleOrDefaultAsync(participant => participant.Id == id);
        }
    }
}
