using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ISpeakerRepository : IRepository<Speaker>
    {
        Task<Speaker> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Speaker>> GetAllWithDetailsAsync();
    }
}
