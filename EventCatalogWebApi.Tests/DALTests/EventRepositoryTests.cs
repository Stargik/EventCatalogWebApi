using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace EventCatalogWebApi.Tests.DALTests
{
    public class EventRepositoryTests
    {

        [Test]
        public async Task EventRepository_GetAllAsync_ReturnsAllEvents()
        {
            var context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            var expected = GetTestEvents;

            var eventRepository = new EventRepository(context);
            var actual = await eventRepository.GetAllAsync();

            Assert.That(actual, Is.EqualTo(expected).Using(new EventEqualityComparer()));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        public async Task EventRepository_GetByIdAsync_ReturnCorrectEvent(int id)
        {
            var context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            var expected = GetTestEvents.FirstOrDefault(x => x.Id == id);

            var eventRepository = new EventRepository(context);
            var actual = await eventRepository.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new EventEqualityComparer()), "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task EventRepository_AddAsync_AddEvent()
        {
            var context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            var expected = context.Events.Count() + 1;

            var newEvent = new Event
            {
                Id = 100,
                Title = "Title1",
                Description = "Description1",
                Address = "Addrees1",
                StartDateTime = new DateTime(2023, 10, 11, 7, 0, 0),
                EndDateTime = new DateTime(2023, 10, 11, 9, 0, 0),
                SpeakerId = 1,
                EventSubjectCategoryId = 1,
                EventFormatId = 1,
            };
            var eventRepository = new EventRepository(context);
            await eventRepository.AddAsync(newEvent);
            await context.SaveChangesAsync();
            var actual = context.Events.Count();

            Assert.That(actual, Is.EqualTo(expected), "AddAsync method works incorrect");
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task EventRepository_DeleteByIdAsync_DeleteEvent(int id)
        {
            var context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            var expected = context.Events.Count() - 1;

            var eventRepository = new EventRepository(context);
            await eventRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();
            var actual = context.Events.Count();

            Assert.That(actual, Is.EqualTo(expected), "DeleteByIdAsync method works incorrect");
        }

        [Test]
        public async Task EventRepository_UpdateAsync_AddEvent()
        {
            var context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            var expected = GetUpdatedTestEvents;

            var newEvent = GetTestEvents.First();
            newEvent.Title = "newTitle1";
            var eventRepository = new EventRepository(context);
            await eventRepository.UpdateAsync(newEvent);
            await context.SaveChangesAsync();
            var actual = context.Events;

            Assert.That(actual, Is.EqualTo(expected).Using(new EventEqualityComparer()), "UpdateAsync method works incorrect");
        }

        public List<Event> GetTestEvents = new List<Event>()
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
        public List<Event> GetUpdatedTestEvents = new List<Event>()
        {
                new Event
                {
                    Id = 1,
                    Title = "newTitle1",
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
}
