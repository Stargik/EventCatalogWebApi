using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IEventFormatRepository : IRepository<EventFormat>
    {
        Task<EventFormat> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<EventFormat>> GetAllWithDetailsAsync();
    }
}
