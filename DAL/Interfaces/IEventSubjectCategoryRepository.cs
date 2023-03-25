using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IEventSubjectCategoryRepository : IRepository<EventSubjectCategory>
    {
        Task<EventSubjectCategory> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<EventSubjectCategory>> GetAllWithDetailsAsync();
    }
}
