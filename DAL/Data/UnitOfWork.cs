using DAL.Interfaces;
using DAL.Repositories;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventCatalogDbContext context;

        public UnitOfWork(EventCatalogDbContext context)
        {
            this.context = context;
        }

        private IEventRepository eventRepository;
        private IEventFormatRepository eventFormatRepository;
        private IEventSubjectCategoryRepository eventSubjectCategoryRepository;
        private ISpeakerRepository speakerRepository;
        private IParticipantRepository participantRepository;
        public IEventRepository EventRepository
        {
            get
            {
                if (eventRepository is null)
                {
                    eventRepository = new EventRepository(context);
                }
                return eventRepository;
            }
        }

        public IEventFormatRepository EventFormatRepository
        {
            get
            {
                if (eventFormatRepository is null)
                {
                    eventFormatRepository = new EventFormatRepository(context);
                }
                return eventFormatRepository;
            }
        }
        public IEventSubjectCategoryRepository EventSubjectCategoryRepository
        {
            get
            {
                if (eventSubjectCategoryRepository is null)
                {
                    eventSubjectCategoryRepository = new EventSubjectCategoryRepository(context);
                }
                return eventSubjectCategoryRepository;
            }
        }

        public ISpeakerRepository SpeakerRepository
        {
            get
            {
                if (speakerRepository is null)
                {
                    speakerRepository = new SpeakerRepository(context);
                }
                return speakerRepository;
            }
        }

        public IParticipantRepository ParticipantRepository
        {
            get
            {
                if (participantRepository is null)
                {
                    participantRepository = new ParticipantRepository(context);
                }
                return participantRepository;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
