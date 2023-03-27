using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IParticipantRepository participantRepository;
        private readonly IMapper mapper;

        public ParticipantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            participantRepository = unitOfWork.ParticipantRepository;
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
            if (String.IsNullOrEmpty(participantModel.Name))
            {
                throw new EventCatalogException("Incorrect ParticipantModel info", "Name");
            }
        }
    }
}
