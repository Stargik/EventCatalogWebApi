using System;
using System.Reflection;
using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using BLL.Validation;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab2.Tests.BLLTests
{
    [Collection("BLLTests")]
    [Trait("TestCategory", "EventsTestCategory"), Trait("LayerCategory", "BLLTests")]
    public class EventServiceTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private EventService eventService;
        private IEnumerable<EventModel> expectedEventModels;

        public EventServiceTests(EventCatalogFixture fixture)
        {
            expectedEventModels = fixture.ExpectedEventModels;

            context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
                UnitTestHelper.SeedData(context);
            }
            unitOfWork = new UnitOfWork(context);
            eventService = new EventService(unitOfWork, UnitTestHelper.GetAutoMapperProfile());
        }

        public void Dispose()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task EventService_GetAllAsync_ReturnsAllEvents()
        {
            var expected = expectedEventModels;

            var actual = await eventService.GetAllAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public async Task EventService_GetByIdAsync_ReturnCorrectEvent(int id)
        {
            var expected = expectedEventModels.FirstOrDefault(e => e.Id == id);

            var actual = await eventService.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task EventService_DeleteAsync_DeletesEvent(int id)
        {
            var eventModel = await eventService.GetByIdAsync(id);

            await eventService.DeleteAsync(id);

            var eventModels = await eventService.GetAllAsync();

            eventModels.Should().NotContainEquivalentOf(eventModel);
        }

        [Theory]
        [MemberData(nameof(NewEventModels))]
        public async Task EventService_AddAsync_AddsEventModel(EventModel eventModel)
        {
            await eventService.AddAsync(eventModel);

            var eventModels = await eventService.GetAllAsync();

            eventModels.Should().ContainEquivalentOf(eventModel);
        }

        [Theory]
        [MemberData(nameof(NewEventModels))]
        public async Task EventService_AddAsync_AllEventModelsHaveValidDates(EventModel eventModel)
        {
            await eventService.AddAsync(eventModel);

            var eventModels = await eventService.GetAllAsync();

            eventModels.Select(x => x.StartDateTime).Should().OnlyContain(x => x > DateTime.Now);
        }

        public static IEnumerable<object[]> NewEventModels => new List<object[]>
        {
            new object[] { new EventModel
            {
                Id = 4,
                Title = "Title4",
                Description = "Description4",
                Address = "Addrees4",
                StartDateTime = new DateTime(2025, 10, 3, 9, 0, 1),
                EndDateTime = new DateTime(2025, 10, 3, 13, 0, 1),
                SpeakerId = 3,
                EventSubjectCategoryId = 1,
                EventFormatId = 2,
                ParticipantsIds = new List<int>()
            } },
            new object[] { new EventModel
            {
                Id = 5,
                Title = "Title5",
                Description = "Description5",
                Address = "Addrees5",
                StartDateTime = new DateTime(2025, 10, 3, 9, 0, 1),
                EndDateTime = new DateTime(2025, 10, 3, 13, 0, 1),
                SpeakerId = 2,
                EventSubjectCategoryId = 2,
                EventFormatId = 1,
                ParticipantsIds = new List<int>()
            } }
        };

    }

}

