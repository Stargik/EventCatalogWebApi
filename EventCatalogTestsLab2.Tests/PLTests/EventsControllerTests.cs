using System;
using System.Collections.Generic;
using BLL.Models;
using BLL.Services;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.Controllers;

namespace EventCatalogTestsLab2.Tests.PLTests
{
	public class EventsControllerTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private EventService eventService;
        private EventsController eventsController;
        private IEnumerable<EventModel> expectedEventModels;

        public EventsControllerTests(EventCatalogFixture fixture)
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
            eventsController = new EventsController(eventService);
        }

        public void Dispose()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task EventsController_Get_ReturnOkObjectResult()
        {
            var actual = await eventsController.Get();

            Assert.IsType<OkObjectResult>(actual.Result);
        }

        [Fact]
        public async Task EventsController_Get_ReturnCorrectModelType()
        {
            var actual = await eventsController.Get();

            Assert.IsAssignableFrom<IEnumerable<EventModel>>(((OkObjectResult)actual.Result).Value);
        }

        [Fact]
        public async Task EventsController_Get_ReturnCorrectModelContent()
        {
            var expected = expectedEventModels;

            var actual = ((OkObjectResult)(await eventsController.Get()).Result).Value;

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task EventsController_GetWithIdParam_ReturnOkObjectResult(int id)
        {
            var actual = await eventsController.Get(id);

            Assert.IsType<OkObjectResult>(actual.Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task EventsController_GetWithIdParam_ReturnCorrectModelType(int id)
        {
            var actual = await eventsController.Get(id);

            Assert.IsAssignableFrom<EventModel>(((OkObjectResult)actual.Result).Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task EventsController_GetWithIdParam_ReturnCorrectModelContent(int id)
        {
            var expected = expectedEventModels.FirstOrDefault(x => x.Id == id);

            var actual = ((OkObjectResult)(await eventsController.Get(id)).Result).Value;

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(101)]
        [InlineData(102)]
        public async Task EventsController_GetWithIdParam_ReturnNotFoundResult(int id)
        {
            var actual = await eventsController.Get(id);

            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Theory]
        [MemberData(nameof(NewEventModels))]
        public async Task EventsController_Add_ReturnOkResult(EventModel eventModel)
        {
            var actual = await eventsController.Add(eventModel);

            Assert.IsType<OkResult>(actual);
        }

        [Theory]
        [MemberData(nameof(NewEventModels))]
        public async Task EventsController_Add_AddsModel(EventModel eventModel)
        {
            await eventsController.Add(eventModel);

            var eventModels = (List<EventModel>)((OkObjectResult)(await eventsController.Get()).Result).Value;

            eventModels.Should().ContainEquivalentOf(eventModel);
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

