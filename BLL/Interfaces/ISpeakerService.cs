using BLL.Models;

namespace BLL.Interfaces
{
    public interface ISpeakerService : ICrud<SpeakerModel>
    {
        Task<SpeakerModel> GetSpeakerByEventIdAsync(int eventId);
    }
}
