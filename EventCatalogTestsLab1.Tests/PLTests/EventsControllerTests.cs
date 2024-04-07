using System;
using System.Collections.Generic;
using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using BLL.Validation;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Internal.Execution;
using PL.Controllers;

namespace EventCatalogTestsLab1.Tests.PLTests
{
	public class EventsControllerTests
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private EventService eventService;
        private EventsController eventsController;
        private IEnumerable<EventModel> expectedEventModels;
        private Mock<IEventService> eventServiceMock;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectedEventModels = new List<EventModel>()
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

        [SetUp]
        public void Setup()
        {
            context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
                UnitTestHelper.SeedData(context);
            }
            unitOfWork = new UnitOfWork(context);
            eventService = new EventService(unitOfWork, UnitTestHelper.GetAutoMapperProfile());
            eventsController = new EventsController(eventService);
            eventServiceMock = new Mock<IEventService>() { CallBase = true };
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Test]
        public async Task EventsController_Get_ReturnOkObjectResult()
        {
            var actual = await eventsController.Get();

            Assert.IsInstanceOf<OkObjectResult>(actual.Result, message: "Get request works incorrect");
        }

        [Test]
        public async Task EventsController_Get_ReturnCorrectModelType()
        {
            var actual = await eventsController.Get();

            Assert.IsInstanceOf<IEnumerable<EventModel>>(((OkObjectResult)actual.Result).Value, message: "Get request works incorrect");
        }

        [Test]
        public async Task EventsController_Get_ReturnCorrectModelContent()
        {
            var expected = expectedEventModels;

            var actual = ((OkObjectResult)(await eventsController.Get()).Result).Value;

            Assert.That(actual, Is.EquivalentTo(expected).Using(new EventModelEqualityComparer()), message: "Get request works incorrect");
        }

        [Test]
        public async Task EventsController_GetWithIdParam_ReturnOkObjectResult([Range(1, 3, 1)] int id)
        {
            var actual = await eventsController.Get(id);

            Assert.IsInstanceOf<OkObjectResult>(actual.Result, message: "Get request works incorrect");
        }

        [Test]
        public async Task EventsController_GetWithIdParam_ReturnCorrectModelType([Range(1, 3, 1)] int id)
        {
            var actual = await eventsController.Get(id);

            Assert.IsInstanceOf<EventModel>(((OkObjectResult)actual.Result).Value, message: "Get request works incorrect");
        }

        [Test]
        public async Task EventsController_GetWithIdParam_ReturnCorrectModelContent([Range(1, 3, 1)] int id)
        {
            var expected = expectedEventModels.FirstOrDefault(x => x.Id == id);

            var actual = ((OkObjectResult)(await eventsController.Get(id)).Result).Value;

            Assert.That(actual, Is.EqualTo(expected).Using(new EventModelEqualityComparer()), message: "Get request works incorrect");
        }

        [Test]
        public async Task EventsController_GetWithIdParam_ReturnNotFoundResult([Range(100, 106, 3)] int id)
        {
            var actual = await eventsController.Get(id);

            Assert.IsInstanceOf<NotFoundResult>(actual.Result, message: "Get method works incorrect");
        }

        [TestCaseSource(nameof(newEventModels))]
        public async Task EventsController_Add_ReturnOkResult(EventModel eventModel)
        {
            var actual = await eventsController.Add(eventModel);

            Assert.IsInstanceOf<OkResult>(actual, message: "Add request works incorrect");
        }

        [TestCaseSource(nameof(newEventModels))]
        public async Task EventsController_Add_AddsModel(EventModel eventModel)
        {
            await eventsController.Add(eventModel);

            var eventModels = ((OkObjectResult)(await eventsController.Get()).Result).Value;

            Assert.That(eventModels, Has.Member(eventModel).Using(new EventModelEqualityComparer()), message: "Add request works incorrect");
        }

        [Test]
        public async Task EventsController_Update_ReturnOkResult()
        {
            var updatedEventModel = new EventModel
            {
                Id = 1,
                Title = "NewTitle",
                Description = "Description1",
                Address = "Addrees1",
                StartDateTime = new DateTime(2025, 10, 11, 7, 0, 0),
                EndDateTime = new DateTime(2025, 10, 11, 9, 0, 0),
                SpeakerId = 1,
                EventSubjectCategoryId = 1,
                EventFormatId = 1,
                ParticipantsIds = new List<int>()
            };

            eventServiceMock.Setup(m => m.UpdateAsync(It.IsAny<EventModel>()));
            var eventsControllerMock = new EventsController(eventServiceMock.Object);

            var actual = await eventsControllerMock.Update(updatedEventModel.Id, updatedEventModel);

            eventServiceMock.Verify(x => x.UpdateAsync(It.Is<EventModel>(c => c.Id == updatedEventModel.Id && c.Title == updatedEventModel.Title)), Times.Once);
            Assert.IsInstanceOf<OkResult>(actual, message: "Update request works incorrect");
        }

        [Test]
        public async Task EventsController_Update_ReturnBadResult()
        {
            var updatedEventModel = new EventModel
            {
                Id = 1,
                Title = "",
                Description = "Description1",
                Address = "Addrees1",
                StartDateTime = new DateTime(2025, 10, 11, 7, 0, 0),
                EndDateTime = new DateTime(2025, 10, 11, 9, 0, 0),
                SpeakerId = 1,
                EventSubjectCategoryId = 1,
                EventFormatId = 1,
                ParticipantsIds = new List<int>()
            };

            eventServiceMock.Setup(m => m.UpdateAsync(It.IsAny<EventModel>())).ThrowsAsync(new Exception());
            var eventsControllerMock = new EventsController(eventServiceMock.Object);

            var actual = await eventsControllerMock.Update(updatedEventModel.Id, updatedEventModel);

            eventServiceMock.Verify(x => x.UpdateAsync(It.Is<EventModel>(c => c.Id == updatedEventModel.Id && c.Title == updatedEventModel.Title)), Times.Once);
            Assert.IsInstanceOf<BadRequestResult>(actual, message: "Update method works incorrect");

        }

        [Test]
        public async Task EventsController_GetUpcomingEventInfo_ReturnBaseInfo()
        {
            var eventsControllerMock = new EventsController(eventServiceMock.Object);

            var actual = ((OkObjectResult)(await eventsControllerMock.GetUpcomingEventInfo()).Result).Value;

            eventServiceMock.Verify(x => x.GetUpcomingEventInfo(), Times.Once);
            Assert.That(actual, Is.EqualTo($"There are no events."), message: "Info request works incorrect");
        }

        [Test]
        public async Task EventsController_GetByCategory_ReturnOkObjectResult([Range(1, 2, 1)] int categoryId)
        {
            eventServiceMock.Setup(m => m.GetEventsByCategoryIdAsync(categoryId)).ReturnsAsync(expectedEventModels.Where(x => x.EventSubjectCategoryId == categoryId));
            var eventsControllerMock = new EventsController(eventServiceMock.Object);

            var actual = await eventsControllerMock.GetByCategory(categoryId);
            eventServiceMock.Verify(x => x.GetEventsByCategoryIdAsync(categoryId), Times.Once);

            Assert.IsInstanceOf<OkObjectResult>(actual.Result, message: "GetByCategory request works incorrect");
        }

        [Test]
        public async Task EventsController_GetByCategory_ReturnCorrectModelType([Range(1, 2, 1)] int categoryId)
        {
            eventServiceMock.Setup(m => m.GetEventsByCategoryIdAsync(categoryId)).ReturnsAsync(expectedEventModels.Where(x => x.EventSubjectCategoryId == categoryId));
            var eventsControllerMock = new EventsController(eventServiceMock.Object);

            var actual = await eventsControllerMock.GetByCategory(categoryId);
            eventServiceMock.Verify(x => x.GetEventsByCategoryIdAsync(categoryId), Times.Once);

            Assert.IsInstanceOf<IEnumerable<EventModel>>(((OkObjectResult)actual.Result).Value, message: "GetByCategory request works incorrect");
        }

        [Test]
        public async Task EventsController_GetByCategory_ReturnCorrectModelContent([Range(1, 2, 1)] int categoryId)
        {
            var expected = expectedEventModels.Where(x => x.EventSubjectCategoryId == categoryId);

            eventServiceMock.Setup(m => m.GetEventsByCategoryIdAsync(categoryId)).ReturnsAsync(expectedEventModels.Where(x => x.EventSubjectCategoryId == categoryId));
            var eventsControllerMock = new EventsController(eventServiceMock.Object);

            var actual = ((OkObjectResult)(await eventsControllerMock.GetByCategory(categoryId)).Result).Value;
            eventServiceMock.Verify(x => x.GetEventsByCategoryIdAsync(categoryId), Times.Once);

            Assert.That(actual, Is.EquivalentTo(expected).Using(new EventModelEqualityComparer()), message: "GetByCategory request works incorrect");
        }

        [Test]
        public async Task EventsController_GetByCategory_ReturnNotFoundResult([Range(100, 106, 3)] int categoryId)
        {
            eventServiceMock.Setup(m => m.GetEventsByCategoryIdAsync(It.IsAny<int>())).ReturnsAsync(null as IEnumerable<EventModel>);
            var eventsControllerMock = new EventsController(eventServiceMock.Object);

            var actual = await eventsControllerMock.GetByCategory(categoryId);
            eventServiceMock.Verify(x => x.GetEventsByCategoryIdAsync(categoryId), Times.Once);

            Assert.IsInstanceOf<NotFoundResult>(actual.Result, message: "GetByCategory method works incorrect");
        }

        public static object[] newEventModels =
        {
            new EventModel
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
            },
            new EventModel
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
            }
        };
    }
}

