using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class SpeakerService : ISpeakerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISpeakerRepository speakerRepository;
        private readonly IMapper mapper;

        public SpeakerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            speakerRepository = unitOfWork.SpeakerRepository;
        }
        public async Task AddAsync(SpeakerModel model)
        {
            await SpeakerModelValidate(model);
            var speaker = mapper.Map<Speaker>(model);
            await speakerRepository.AddAsync(speaker);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var speaker = await speakerRepository.GetByIdWithDetailsAsync(id);
            if (speaker is not null && speaker.Events.Any())
            {
                throw new InvalidOperationException("Speaker with existing events cannot be deleted.");
            }
            await speakerRepository.DeleteByIdAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<SpeakerModel>> GetAllAsync()
        {
            var speakers = await speakerRepository.GetAllWithDetailsAsync();
            var speakerModels = mapper.Map<IEnumerable<Speaker>, IEnumerable<SpeakerModel>>(speakers);
            return speakerModels;
        }

        public async Task<SpeakerModel> GetByIdAsync(int id)
        {
            var speaker = await speakerRepository.GetByIdWithDetailsAsync(id);
            var speakerModel = mapper.Map<SpeakerModel>(speaker);
            return speakerModel;
        }

        public async Task<SpeakerModel> GetSpeakerByEventIdAsync(int eventId)
        {
            var _event = await unitOfWork.EventRepository.GetByIdAsync(eventId);
            if (_event is null)
            {
                throw new EventCatalogException("Incorrect event info", "Id");
            }
            var speaker = await speakerRepository.GetByIdWithDetailsAsync(_event.SpeakerId);
            var speakerModel = mapper.Map<SpeakerModel>(speaker);
            return speakerModel;
        }

        public async Task UpdateAsync(SpeakerModel model)
        {
            await SpeakerModelValidate(model);
            var speakerOld = await speakerRepository.GetByIdAsync(model.Id);
            if (speakerOld is null)
            {
                throw new EventCatalogException("Incorrect SpeakerModel info (speaker with this id is not exist)", "Id");
            }
            var speaker = mapper.Map<Speaker>(model);
            await speakerRepository.UpdateAsync(speaker);
            await unitOfWork.SaveAsync();
        }

        private async Task SpeakerModelValidate(SpeakerModel speakerModel)
        {
            if (speakerModel is null)
            {
                throw new EventCatalogException("Incorrect SpeakerModel info", "SpeakerModel");
            }
            if (String.IsNullOrEmpty(speakerModel.Email))
            {
                throw new EventCatalogException("Incorrect SpeakerModel info", "Email");
            }
            if (String.IsNullOrEmpty(speakerModel.FirstName))
            {
                throw new EventCatalogException("Incorrect SpeakerModel info", "FirstName");
            }
            if (String.IsNullOrEmpty(speakerModel.LastName))
            {
                throw new EventCatalogException("Incorrect SpeakerModel info", "LastName");
            }
            
        }
    }
}
