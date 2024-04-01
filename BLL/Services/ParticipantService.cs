using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interfaces;
using System.Text.RegularExpressions;

namespace BLL.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IParticipantRepository participantRepository;
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;

        public ParticipantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            participantRepository = unitOfWork.ParticipantRepository;
            eventRepository = unitOfWork.EventRepository;
        }
        public async Task AddAsync(ParticipantModel model)
        {
            await ParticipantModelValidate(model);
            var participant = mapper.Map<Participant>(model);
            await participantRepository.AddAsync(participant);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await participantRepository.DeleteByIdAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ParticipantModel>> GetAllAsync()
        {
            var participants = await participantRepository.GetAllWithDetailsAsync();
            var participantModels = mapper.Map<IEnumerable<Participant>, IEnumerable<ParticipantModel>>(participants);
            return participantModels;
        }

        public async Task<ParticipantModel> GetByIdAsync(int id)
        {
            var participant = await participantRepository.GetByIdWithDetailsAsync(id);
            var participantModel = mapper.Map<ParticipantModel>(participant);
            return participantModel;
        }

        public async Task<ParticipantModel> GetByEmailAsync(string email)
        {
            var participant = (await participantRepository.GetAllWithDetailsAsync()).FirstOrDefault(participant => participant.Email == email);
            var participantModel = mapper.Map<ParticipantModel>(participant);
            return participantModel;
        }

        public async Task<IEnumerable<ParticipantModel>> GetParticipantsByEventIdAsync(int eventId)
        {
            var _event = await unitOfWork.EventRepository.GetByIdWithDetailsAsync(eventId);
            if (_event is null)
            {
                throw new EventCatalogException("Incorrect event info", "Id");
            }
            var participantModels = mapper.Map<IEnumerable<Participant>, IEnumerable<ParticipantModel>>(_event.Participants);
            return participantModels;
        }

        public async Task UpdateAsync(ParticipantModel model)
        {
            await ParticipantModelValidate(model);
            var participantOld = await participantRepository.GetByIdAsync(model.Id);
            if (participantOld is null)
            {
                throw new EventCatalogException("Incorrect ParticipantModel info (participant with this id is not exist)", "Id");
            }
            var participant = mapper.Map<Participant>(model);
            var events = (await eventRepository.GetAllAsync()).Where(_event => model.EventsIds.Contains(_event.Id)).ToList();
            participant.Events = events;
            await participantRepository.UpdateAsync(participant);
            await unitOfWork.SaveAsync();
        }

        private async Task ParticipantModelValidate(ParticipantModel participantModel)
        {
            if (participantModel is null)
            {
                throw new EventCatalogException("Incorrect ParticipantModel info", "SpeakerModel");
            }
            if (String.IsNullOrEmpty(participantModel.Email))
            {
                throw new EventCatalogException("Incorrect ParticipantModel info", "Email");
            }
            if (!Regex.IsMatch(participantModel.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                throw new EventCatalogException("Incorrect ParticipantModel info", "Email");
            }
            if (String.IsNullOrEmpty(participantModel.Name))
            {
                throw new EventCatalogException("Incorrect ParticipantModel info", "Name");
            }
        }
    }
}
