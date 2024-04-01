using System;
using BLL.Models;
using DAL.Entities;

namespace EventCatalogTestsLab2.Tests
{
	public class EventCatalogFixture
	{
        public IEnumerable<Speaker> ExpectedSpeakers { get; private set; }
        public IEnumerable<Participant> ExpectedParticipants { get; private set; }
        public IEnumerable<EventSubjectCategory> ExpectedEventSubjectCategories { get; private set; }
        public IEnumerable<Event> ExpectedEvents { get; private set; }
        public IEnumerable<EventFormat> ExpectedEventFormats { get; private set; }
        public IEnumerable<SpeakerModel> ExpectedSpeakerModels { get; private set; }
        public IEnumerable<ParticipantModel> ExpectedParticipantModels { get; private set; }
        public IEnumerable<EventModel> ExpectedEventModels { get; private set; }

        public EventCatalogFixture()
		{
            ExpectedSpeakers = new List<Speaker>
            {
                new Speaker { Id = 1, FirstName = "FirstName1", LastName = "LastName1", Email = "speaker1@example.com" },
                new Speaker { Id = 2, FirstName = "FirstName2", LastName = "LastName2", Email = "speaker2@example.com" },
                new Speaker { Id = 3, FirstName = "FirstName3", LastName = "LastName3", Email = "speaker3@example.com" }
            };

            ExpectedParticipants = new List<Participant>
            {
                new Participant { Id = 1, Name="ParticipantName1", Email = "participant1@example.com" },
                new Participant { Id = 2, Name = "ParticipantName2", Email = "participant2@example.com" },
                new Participant { Id = 3, Name = "ParticipantName3", Email = "participant3@example.com" },
                new Participant { Id = 4, Name = "ParticipantName4", Email = "participant4@example.com" }
            };

            ExpectedEventSubjectCategories = new List<EventSubjectCategory>
            {
                new EventSubjectCategory { Id = 1, Title = "Category1"},
                new EventSubjectCategory { Id = 2, Title = "Category2" },
                new EventSubjectCategory { Id = 3, Title = "Category3" }
            };

            ExpectedEvents = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Title1",
                    Description = "Description1",
                    Address = "Addrees1",
                    StartDateTime = new DateTime(2025, 10, 11, 7, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 11, 9, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 1,
                    EventFormatId = 1
                },
                new Event
                {
                    Id = 2,
                    Title = "Title2",
                    Description = "Description2",
                    Address = "Addrees2",
                    StartDateTime = new DateTime(2025, 10, 12, 8, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 12, 11, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 2
                },
                new Event
                {
                    Id = 3,
                    Title = "Title3",
                    Description = "Description3",
                    Address = "Addrees3",
                    StartDateTime = new DateTime(2025, 10, 3, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 3, 13, 0, 0),
                    SpeakerId = 2,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 1
                }
            };

            ExpectedEventFormats = new List<EventFormat>
            {
                new EventFormat { Id = 1, Format = "Online"},
                new EventFormat { Id = 2, Format = "Ofline"}
            };

            ExpectedSpeakerModels = new List<SpeakerModel>
            {
                new SpeakerModel { Id = 1, FirstName = "FirstName1", LastName = "LastName1", Email = "speaker1@example.com", EventsIds = new List<int>() { 1, 2 } },
                new SpeakerModel { Id = 2, FirstName = "FirstName2", LastName = "LastName2", Email = "speaker2@example.com", EventsIds = new List<int>() { 3 } },
                new SpeakerModel { Id = 3, FirstName = "FirstName3", LastName = "LastName3", Email = "speaker3@example.com", EventsIds = new List<int>() }
            };

            ExpectedParticipantModels = new List<ParticipantModel>
            {
                new ParticipantModel { Id = 1, Name = "ParticipantName1", Email = "participant1@example.com", EventsIds = new List<int>() },
                new ParticipantModel { Id = 2, Name = "ParticipantName2", Email = "participant2@example.com", EventsIds = new List<int>() },
                new ParticipantModel { Id = 3, Name = "ParticipantName3", Email = "participant3@example.com", EventsIds = new List<int>() },
                new ParticipantModel { Id = 4, Name = "ParticipantName4", Email = "participant4@example.com", EventsIds = new List<int>() }
            };

            ExpectedEventModels = new List<EventModel>()
            {
                new EventModel
                {
                    Id = 1,
                    Title = "Title1",
                    Description = "Description1",
                    Address = "Addrees1",
                    StartDateTime = new DateTime(2025, 10, 11, 7, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 11, 9, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 1,
                    EventFormatId = 1,
                    ParticipantsIds = new List<int>()
                },
                new EventModel
                {
                    Id = 2,
                    Title = "Title2",
                    Description = "Description2",
                    Address = "Addrees2",
                    StartDateTime = new DateTime(2025, 10, 12, 8, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 12, 11, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 2,
                    ParticipantsIds = new List<int>()
                },
                new EventModel
                {
                    Id = 3,
                    Title = "Title3",
                    Description = "Description3",
                    Address = "Addrees3",
                    StartDateTime = new DateTime(2025, 10, 3, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 3, 13, 0, 0),
                    SpeakerId = 2,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 1,
                    ParticipantsIds = new List<int>()
                }
            };

        }
	}
}

