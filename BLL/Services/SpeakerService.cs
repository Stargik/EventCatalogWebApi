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
        public Task AddAsync(SpeakerModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpeakerModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SpeakerModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SpeakerModel> GetSpeakerByEventIdAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SpeakerModel model)
        {
            throw new NotImplementedException();
        }
    }
}
