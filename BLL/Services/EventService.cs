using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEventRepository eventRepository;
        private readonly IEventSubjectCategoryRepository eventSubjectCategoryRepository;
        private readonly IMapper mapper;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            eventRepository = unitOfWork.EventRepository;
            eventSubjectCategoryRepository = unitOfWork.EventSubjectCategoryRepository;
        }
        public async Task AddAsync(EventModel model)
        {
            await EventModelValidate(model);
            var _event = mapper.Map<Event>(model);
            await eventRepository.AddAsync(_event);
            await unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(EventSubjectCategoryModel categoryModel)
        {
            await EventSubjectCategoryModelValidate(categoryModel);
            var category = mapper.Map<EventSubjectCategory>(categoryModel);
            await eventSubjectCategoryRepository.AddAsync(category);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await eventRepository.DeleteByIdAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await eventSubjectCategoryRepository.GetByIdWithDetailsAsync(categoryId);
            if (category is not null && category.Events.Any())
            {
                throw new InvalidOperationException("Category with existing events cannot be deleted.");
            }
            await eventSubjectCategoryRepository.DeleteByIdAsync(categoryId);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<EventModel>> GetAllAsync()
        {
            var events = await eventRepository.GetAllWithDetailsAsync();
            var eventModels = mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events);
            return eventModels;
        }

        public async Task<EventModel> GetByIdAsync(int id)
        {
            var _event = await eventRepository.GetByIdAsync(id);
            var eventModel = mapper.Map<EventModel>(_event);
            return eventModel;
        }

        public async Task<IEnumerable<EventModel>> GetEventsByCategoryIdAsync(int categoryId)
        {
            var events = (await eventRepository.GetAllWithDetailsAsync()).Where(e => e.EventSubjectCategoryId == categoryId);
            var eventModels = mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events);
            return eventModels;
        }

        public async Task<IEnumerable<EventModel>> GetEventsBySpeakerIdAsync(int speakerId)
        {
            var events = (await eventRepository.GetAllWithDetailsAsync()).Where(e => e.SpeakerId == speakerId);
            var eventModels = mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events);
            return eventModels;
        }

        public async Task UpdateAsync(EventModel model)
        {
            await EventModelValidate(model);
            var eventOld = await eventRepository.GetByIdAsync(model.Id);
            if (eventOld is null)
            {
                throw new EventCatalogException("Incorrect EventModel info (event with this id is not exist)", "Id");
            }
            var _event = mapper.Map<Event>(model);
            await eventRepository.UpdateAsync(_event);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(EventSubjectCategoryModel categoryModel)
        {
            await EventSubjectCategoryModelValidate(categoryModel);
            var categoryOld = await eventSubjectCategoryRepository.GetByIdAsync(categoryModel.Id);
            if (categoryOld is null)
            {
                throw new EventCatalogException("Incorrect EventSubjectCategoryModel info (category with this id is not exist)", "Id");
            }
            var category = mapper.Map<EventSubjectCategory>(categoryModel);
            await eventSubjectCategoryRepository.UpdateAsync(category);
            await unitOfWork.SaveAsync();
        }

        private async Task EventModelValidate(EventModel eventModel)
        {
            if (eventModel is null)
            {
                throw new EventCatalogException("Incorrect EventModel info", "EventModel");
            }
            if (String.IsNullOrEmpty(eventModel.Title))
            {
                throw new EventCatalogException("Incorrect EventModel info", "Title");
            }
            if (String.IsNullOrEmpty(eventModel.Address))
            {
                throw new EventCatalogException("Incorrect EventModel info", "Address");
            }
            if (eventModel.StartDateTime <= DateTime.Now)
            {
                throw new EventCatalogException("Incorrect EventModel info", "StartDateTime");
            }
            if (eventModel.EndDateTime <= eventModel.StartDateTime)
            {
                throw new EventCatalogException("Incorrect EventModel info", "EndDateTime");
            }
            var category = await eventSubjectCategoryRepository.GetByIdAsync(eventModel.EventSubjectCategoryId);
            if (category is null)
            {
                throw new EventCatalogException("Incorrect EventModel info", "EventSubjectCategoryId");
            }
            var speaker = await unitOfWork.SpeakerRepository.GetByIdAsync(eventModel.SpeakerId);
            if (speaker is null)
            {
                throw new EventCatalogException("Incorrect EventModel info", "SpeakerId");
            }
            var eventFormat = await unitOfWork.EventFormatRepository.GetByIdAsync(eventModel.EventFormatId);
            if (eventFormat is null)
            {
                throw new EventCatalogException("Incorrect EventModel info", "EventFormatId");
            }
        }
        private async Task EventSubjectCategoryModelValidate(EventSubjectCategoryModel categoryModel)
        {
            if (categoryModel is null)
            {
                throw new EventCatalogException("Incorrect EventSubjectCategoryModel info", "EventSubjectCategoryModel");
            }
            if (String.IsNullOrEmpty(categoryModel.Title))
            {
                throw new EventCatalogException("Incorrect EventSubjectCategoryModel info", "Title");
            }
        }
    }
}
