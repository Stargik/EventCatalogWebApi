using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Task AddAsync(EventModel model)
        {
            throw new NotImplementedException();
        }

        public Task AddCategoryAsync(EventSubjectCategoryModel categoryModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<EventModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventModel>> GetEventsByCategoryIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventModel>> GetEventsBySpeakerIdAsync(int speakerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(EventModel model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(EventSubjectCategoryModel categoryModel)
        {
            throw new NotImplementedException();
        }
    }
}
