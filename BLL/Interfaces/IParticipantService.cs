using BLL.Models;

namespace BLL.Interfaces
{
    public interface IParticipantService : ICrud<ParticipantModel>
    {
        Task<IEnumerable<ParticipantModel>> GetParticipantsByEventIdAsync(int eventId);
        Task<ParticipantModel> GetByEmailAsync(string email);
    }
}
