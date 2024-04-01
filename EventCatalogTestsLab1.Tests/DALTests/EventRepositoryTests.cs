using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab1.Tests.DALTests
{
    [TestFixture]
    public class EventRepositoryTests
    {
        private EventCatalogDbContext context;
        private EventRepository eventRepository;
        private IEnumerable<Event> expectedEvents;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectedEvents = new List<Event>
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
                    EventFormatId = 1,
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
                    EventFormatId = 2,
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
                    EventFormatId = 1,
                }
            };
        }

        [SetUp]
        public void Setup()
        {
            context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
                UnitTestHelper.SeedData(context);
            }

            eventRepository = new EventRepository(context);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestCase(2)]
        [TestCase(3)]
        public async Task EventFormatRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedEvents.FirstOrDefault(x => x.Id == id);

            var actual = await eventRepository.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new EventEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task EventFormatRepository_GetAllAsync_ReturnsAllEvents()
        {
            var expected = expectedEvents;

            var actual = await eventRepository.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected).Using(new EventEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCaseSource(nameof(newEvents))]
        public async Task EventFormatRepository_AddAsync_AddsValueToDatabase(Event _event)
        {
            await eventRepository.AddAsync(_event);
            await context.SaveChangesAsync();

            var events = await eventRepository.GetAllAsync();

            Assert.That(events, Has.Member(_event).Using(new EventEqualityComparer()), message: "AddAsync method works incorrect");
        }

        [TestCase(2)]
        [TestCase(3)]
        public async Task EventFormatRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var _event = await eventRepository.GetByIdAsync(id);

            await eventRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var events = await eventRepository.GetAllAsync();

            Assert.That(events, Has.No.Member(_event).Using(new EventEqualityComparer()), message: "DeleteByIdAsync works incorrect");
        }

        [Test]
        public async Task EventFormatRepository_UpdateAsync_UpdatesEntity()
        {
            var _event = await eventRepository.GetByIdWithDetailsAsync(1);
            _event.Title = "newTitle";

            await eventRepository.UpdateAsync(_event);
            await context.SaveChangesAsync();

            var events = await eventRepository.GetAllAsync();
            var newEvent = new Event
            {
                Id = 1,
                Title = "newTitle",
                Description = "Description1",
                Address = "Addrees1",
                StartDateTime = new DateTime(2025, 10, 11, 7, 0, 0),
                EndDateTime = new DateTime(2025, 10, 11, 9, 0, 0),
                SpeakerId = 1,
                EventSubjectCategoryId = 1,
                EventFormatId = 1,
            };

            Assert.That(events, Has.Member(newEvent).Using(new EventEqualityComparer()), message: "UpdateAsync method works incorrect");
        }

        public static object[] newEvents =
        {
            new Event
            {
                Id = 4,
                Title = "Title4",
                Description = "Description4",
                Address = "Addrees4",
                StartDateTime = new DateTime(2025, 10, 11, 7, 0, 1),
                EndDateTime = new DateTime(2025, 10, 11, 9, 0, 1),
                SpeakerId = 2,
                EventSubjectCategoryId = 2,
                EventFormatId = 2,
            },
            new Event
            {
                Id = 5,
                Title = "Title5",
                Description = "Description5",
                Address = "Addrees5",
                StartDateTime = new DateTime(2025, 10, 11, 7, 0, 1),
                EndDateTime = new DateTime(2025, 10, 11, 9, 0, 1),
                SpeakerId = 1,
                EventSubjectCategoryId = 1,
                EventFormatId = 1,
            }
        };

    }
}

