using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IParticipantRepository : IRepository<Participant>
    {
        Task<Participant> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Participant>> GetAllWithDetailsAsync();
    }
}
