using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepository { get; }
        IEventFormatRepository EventFormatRepository { get; }
        IEventSubjectCategoryRepository EventSubjectCategoryRepository { get; }
        ISpeakerRepository SpeakerRepository { get; }
        IParticipantRepository ParticipantRepository { get; }
        Task SaveAsync();
    }
}
