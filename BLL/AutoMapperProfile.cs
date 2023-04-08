using AutoMapper;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Speaker, SpeakerModel>()
                .ForMember(speakerModel => speakerModel.EventsIds, x => x.MapFrom(speaker => speaker.Events.Select(e => e.Id)))
                .ReverseMap();

            CreateMap<EventSubjectCategory, EventSubjectCategoryModel>()
                .ForMember(eventSubjectCategoryModel => eventSubjectCategoryModel.EventsIds, x => x.MapFrom(eventSubjectCategory => eventSubjectCategory.Events.Select(e => e.Id)))
                .ReverseMap();

            CreateMap<Participant, ParticipantModel>()
                .ForMember(participantModel => participantModel.EventsIds, x => x.MapFrom(participant => participant.Events.Select(e => e.Id)))
                .ReverseMap();

            CreateMap<Event, EventModel>()
                .ForMember(eventModel => eventModel.ParticipantsIds, x => x.MapFrom(e => e.Participants.Select(participant => participant.Id)))
                .ReverseMap();

            CreateMap<EventFormat, EventFormatModel>()
                .ReverseMap();

            CreateMap<ApiUser, UserModel>();

            CreateMap<UserModel, ApiUser>()
                .ForMember(apiUser => apiUser.UserName, x => x.MapFrom(userModel => userModel.Email));

            CreateMap<UserModel, ParticipantModel>();
        }
    }
}
