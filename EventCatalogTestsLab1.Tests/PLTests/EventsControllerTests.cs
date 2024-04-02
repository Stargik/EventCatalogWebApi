using System;
using System.Collections.Generic;
using BLL.Models;
using BLL.Services;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

