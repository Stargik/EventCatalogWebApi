using AutoMapper;
using BLL;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogWebApi.Tests
{
    public static class UnitTestHelper
    {
        public static DbContextOptions<EventCatalogDbContext> GetUnitTestDbContextOptions()
        {
            var options = new DbContextOptionsBuilder<EventCatalogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new EventCatalogDbContext(options))
            {
                SeedData(context);
            }

            return options;

        }

        public static IMapper GetAutoMapperProfile()
        {
            var profile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(c => c.AddProfile(profile));
            return new Mapper(configuration);
        }

        public static void SeedData(EventCatalogDbContext context)
        {
            context.Speakers.AddRange(
                new Speaker { Id = 1, FirstName = "FirstName1", LastName = "LastName1", Email = "speaker1@example.com" },
                new Speaker { Id = 2, FirstName = "FirstName2", LastName = "LastName2", Email = "speaker2@example.com" },
                new Speaker { Id = 3, FirstName = "FirstName3", LastName = "LastName3", Email = "speaker3@example.com" }
            );
            context.Participants.AddRange(
                new Participant { Id = 1, Name="ParticipantName1", Email = "participant1@example.com" },
                new Participant { Id = 2, Name = "ParticipantName2", Email = "participant2@example.com" },
                new Participant { Id = 3, Name = "ParticipantName3", Email = "participant3@example.com" },
                new Participant { Id = 4, Name = "ParticipantName4", Email = "participant4@example.com" }
            );
            context.EventSubjectCategories.AddRange(
                new EventSubjectCategory { Id = 1, Title = "Category1"},
                new EventSubjectCategory { Id = 2, Title = "Category2" },
                new EventSubjectCategory { Id = 3, Title = "Category3" }
            );
            context.EventFormats.AddRange(
                new EventFormat { Id = 1, Format = "Online"},
                new EventFormat { Id = 2, Format = "Ofline"}
            );
            context.Events.AddRange(
                new Event
                {
                    Id = 1,
                    Title = "Title1",
                    Description = "Description1",
                    Address = "Addrees1",
                    StartDateTime = new DateTime(2023, 10, 11, 7, 0, 0),
                    EndDateTime = new DateTime(2023, 10, 11, 9, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 1,
                    EventFormatId = 1,
                },
                new Event
                {
                    Id = 2,
                    Title = "Title2",
                    Description = "Description2",
                    Address = "Addrees2",
                    StartDateTime = new DateTime(2023, 10, 12, 8, 0, 0),
                    EndDateTime = new DateTime(2023, 10, 12, 11, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 2,
                },
                new Event
                {
                    Id = 3,
                    Title = "Title3",
                    Description = "Description3",
                    Address = "Addrees3",
                    StartDateTime = new DateTime(2023, 10, 3, 9, 0, 0),
                    EndDateTime = new DateTime(2023, 10, 3, 13, 0, 0),
                    SpeakerId = 2,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 1,
                }
            );
            
            context.SaveChanges();
        }
    }
}
