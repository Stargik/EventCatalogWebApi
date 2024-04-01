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
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab1.Tests.BLLTests
{
	public class EventServiceTests
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private EventService eventService;
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
        public async Task EventService_GetAllAsync_ReturnsAllEvents()
        {
            var expected = expectedEventModels;

            var actual = await eventService.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected).Using(new EventModelEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCase(2)]
        [TestCase(3)]
        public async Task EventService_GetByIdAsync_ReturnCorrectEvent(int id)
        {
            var expected = expectedEventModels.FirstOrDefault(e => e.Id == id);

            var actual = await eventService.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new EventModelEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task EventService_DeleteAsync_DeletesEvent(int id)
        {
            var eventModel = await eventService.GetByIdAsync(id);

            await eventService.DeleteAsync(id);

            var eventModels = await eventService.GetAllAsync();

            Assert.That(eventModels, Has.No.Member(eventModel).Using(new EventModelEqualityComparer()), message: "DeleteAsync method works incorrect");
        }

        [TestCaseSource(nameof(newEventModels))]
        public async Task EventService_AddAsync_AddsEventModel(EventModel eventModel)
        {
            Assume.That(eventModel, Has.Property("StartDateTime").GreaterThan(DateTime.Now), message: "StartDateTime property must be greater than now for AddAsync method works correct");

            await eventService.AddAsync(eventModel);

            var eventModels = await eventService.GetAllAsync();

            Assert.That(eventModels, Has.Member(eventModel).Using(new EventModelEqualityComparer()), message: "AddAsync method works incorrect");
        }

        [TestCaseSource(nameof(newEventModels))]
        public async Task EventService_AddAsync_AllEventModelsHaveValidDates(EventModel eventModel)
        {
            Assume.That(eventModel, Has.Property("StartDateTime").GreaterThan(DateTime.Now), message: "StartDateTime property must be greater than now for AddAsync method works correct");

            await eventService.AddAsync(eventModel);

            var eventModels = await eventService.GetAllAsync();

            Assert.That(eventModels, Has.All.Property("StartDateTime").GreaterThan(DateTime.Now), message: "AddAsync method works incorrect");
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

