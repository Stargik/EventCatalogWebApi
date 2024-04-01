using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace EventCatalogTestsLab2.Tests.DALTests
{
    public class EventRepositoryTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private EventRepository eventRepository;
        private IEnumerable<Event> expectedEvents;

        public EventRepositoryTests(EventCatalogFixture fixture)
        {
            expectedEvents = fixture.ExpectedEvents;

            context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
                UnitTestHelper.SeedData(context);
            }

            eventRepository = new EventRepository(context);
        }

        public void Dispose()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public async Task EventFormatRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedEvents.FirstOrDefault(x => x.Id == id);

            var actual = await eventRepository.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task EventFormatRepository_GetAllAsync_ReturnsAllEvents()
        {
            var expected = expectedEvents;

            var actual = await eventRepository.GetAllAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(NewEvents))]
        public async Task EventFormatRepository_AddAsync_AddsValueToDatabase(Event _event)
        {
            await eventRepository.AddAsync(_event);
            await context.SaveChangesAsync();

            var events = await eventRepository.GetAllWithDetailsAsync();

            events.Should().ContainEquivalentOf(_event);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public async Task EventFormatRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var _event = await eventRepository.GetByIdAsync(id);

            await eventRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var events = await eventRepository.GetAllAsync();

            events.Should().NotContainEquivalentOf(_event);
        }

        [Fact]
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
                EventFormatId = 1
            };

            events.Should().ContainEquivalentOf(newEvent);
        }

        public static IEnumerable<object[]> NewEvents => new List<object[]>
        {
            new object[] { new Event
            {
                Id = 4,
                Title = "Title4",
                Description = "Description4",
                Address = "Addrees4",
                StartDateTime = new DateTime(2025, 10, 11, 7, 0, 1),
                EndDateTime = new DateTime(2025, 10, 11, 9, 0, 1),
                SpeakerId = 2,
                EventSubjectCategoryId = 2,
                EventFormatId = 2
            } },
            new object[] { new Event
            {
                Id = 5,
                Title = "Title5",
                Description = "Description5",
                Address = "Addrees5",
                StartDateTime = new DateTime(2025, 10, 11, 7, 0, 1),
                EndDateTime = new DateTime(2025, 10, 11, 9, 0, 1),
                SpeakerId = 1,
                EventSubjectCategoryId = 1,
                EventFormatId = 1
            } }
        };

    }
}

