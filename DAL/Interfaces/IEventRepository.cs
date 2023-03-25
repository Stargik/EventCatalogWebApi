using DAL.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Event>> GetAllWithDetailsAsync();
    }
}
