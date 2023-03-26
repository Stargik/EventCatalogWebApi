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
        public Task AddAsync(ParticipantModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ParticipantModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ParticipantModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ParticipantModel>> GetParticipantsByEventIdAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ParticipantModel model)
        {
            throw new NotImplementedException();
        }
    }
}
