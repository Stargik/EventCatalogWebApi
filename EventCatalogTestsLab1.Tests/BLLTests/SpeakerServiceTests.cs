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
    [TestFixture]
    public class SpeakerServiceTests
	{
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private SpeakerService speakerService;
        private IEnumerable<SpeakerModel> expectedSpeakerModels;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectedSpeakerModels = new List<SpeakerModel>
            {
                new SpeakerModel { Id = 1, FirstName = "FirstName1", LastName = "LastName1", Email = "speaker1@example.com", EventsIds = new List<int>() { 1, 2 } },
                new SpeakerModel { Id = 2, FirstName = "FirstName2", LastName = "LastName2", Email = "speaker2@example.com", EventsIds = new List<int>() { 3 } },
                new SpeakerModel { Id = 3, FirstName = "FirstName3", LastName = "LastName3", Email = "speaker3@example.com", EventsIds = new List<int>() }
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
            speakerService = new SpeakerService(unitOfWork, UnitTestHelper.GetAutoMapperProfile());
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task SpeakerService_GetById_ReturnsSpeakerModel(int id)
        {
            var expected = expectedSpeakerModels.FirstOrDefault(x => x.Id == id);

            var actual = await speakerService.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new SpeakerModelEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task SpeakerService_GetSpeakerByEventIdAsync_ReturnsSpeakerModel(int eventId)
        {
            var expected = expectedSpeakerModels.FirstOrDefault(x => x.EventsIds.Contains(eventId));

            var actual = await speakerService.GetSpeakerByEventIdAsync(eventId);

            Assert.That(actual, Is.EqualTo(expected).Using(new SpeakerModelEqualityComparer()), message: "GetSpeakerByEventIdAsync method works incorrect");
        }

        [Test]
        public async Task SpeakerService_GetAll_ReturnsAllSpeakers()
        {
            var expected = expectedSpeakerModels;

            var actual = await speakerService.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected).Using(new SpeakerModelEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task SpeakerService_AddAsync_AddsModel()
        {

            var speakerModel = new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4@example.com", EventsIds = new List<int>() };

            await speakerService.AddAsync(speakerModel);

            var speakerModels = await speakerService.GetAllAsync();

            Assert.That(speakerModels, Has.Member(speakerModel).Using(new SpeakerModelEqualityComparer()), message: "AddAsync method works incorrect");
        }

        [TestCaseSource(nameof(newSpeakerModels))]
        public async Task SpeakerService_AddAsync_AllParticipantsHaveValidEmails(SpeakerModel speakerModel)
        {
            await speakerService.AddAsync(speakerModel);

            var speakerModels = await speakerService.GetAllAsync();

            Assert.That(speakerModels, Has.All.Property("Email").Match(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").IgnoreCase, message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task SpeakerService_AddAsync_EventCatalogExceptionWithInvalidEmail()
        {

            var speakerModel = new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4example.com", EventsIds = new List<int>() };

            Assert.ThrowsAsync<EventCatalogException>(async () => await speakerService.AddAsync(speakerModel), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task SpeakerService_AddAsync_EventCatalogExceptionWithEmptyInfoField()
        {
            var speakerModel = new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = String.Empty, EventsIds = new List<int>() };

            Assert.ThrowsAsync<EventCatalogException>(async () => await speakerService.AddAsync(speakerModel), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task SpeakerService_AddAsync_EventCatalogExceptionWithNullObject()
        {
            Assert.ThrowsAsync<EventCatalogException>(async () => await speakerService.AddAsync(null), message: "AddAsync method works incorrect");
        }


        [TestCase(3)]
        [TestCase(100)]
        public async Task SpeakerService_DeleteAsync_DeletesSpeaker(int id)
        {
            var speakerModel = await speakerService.GetByIdAsync(id);

            await speakerService.DeleteAsync(id);

            var speakerModels = await speakerService.GetAllAsync();

            Assert.That(speakerModels, Has.No.Member(speakerModel).Using(new SpeakerModelEqualityComparer()), message: "DeleteAsync method works incorrect");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task SpeakerService_DeleteAsync_EventCatalogExceptionWithSpeakerWithEvents(int id)
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await speakerService.DeleteAsync(id), message: "DeleteAsync method works incorrect");
        }

        [Test]
        public async Task SpeakerService_UpdateAsync_EventCatalogExceptionWithEmptyInfoField()
        {
            var speakerModel = await speakerService.GetByIdAsync(1);

            speakerModel.Email = string.Empty;

            Assert.ThrowsAsync<EventCatalogException>(async () => await speakerService.UpdateAsync(speakerModel), message: "UpdateAsync method works incorrect");
        }

        public static object[] newSpeakerModels =
        {
            new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4@example.com", EventsIds = new List<int>() },
            new SpeakerModel { Id = 5, FirstName = "FirstName5", LastName = "LastName5", Email = "speaker5@example.com", EventsIds = new List<int>() }
        };
    }

}

