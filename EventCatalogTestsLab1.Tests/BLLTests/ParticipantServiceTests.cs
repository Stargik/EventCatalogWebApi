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
	public class ParticipantServiceTests
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private ParticipantService participantService;
        private IEnumerable<ParticipantModel> expectedParticipantModels;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectedParticipantModels = new List<ParticipantModel>
            {
                new ParticipantModel { Id = 1, Name = "ParticipantName1", Email = "participant1@example.com", EventsIds = new List<int>() },
                new ParticipantModel { Id = 2, Name = "ParticipantName2", Email = "participant2@example.com", EventsIds = new List<int>() },
                new ParticipantModel { Id = 3, Name = "ParticipantName3", Email = "participant3@example.com", EventsIds = new List<int>() },
                new ParticipantModel { Id = 4, Name = "ParticipantName4", Email = "participant4@example.com", EventsIds = new List<int>() }
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
            participantService = new ParticipantService(unitOfWork, UnitTestHelper.GetAutoMapperProfile());
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
        public async Task ParticipantService_GetById_ReturnsParticipantModel(int id)
        {
            var expected = expectedParticipantModels.FirstOrDefault(x => x.Id == id);

            var actual = await participantService.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new ParticipantModelEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [TestCase("participant1@example.com")]
        [TestCase("participant2@example.com")]
        public async Task ParticipantService_GetByEmailAsync_ReturnsParticipantModel(string email)
        {
            Assume.That(email, Does.Match(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").IgnoreCase, message: "Email for GetByIdAsync is not correct");

            var expected = expectedParticipantModels.FirstOrDefault(x => x.Email == email);

            var actual = await participantService.GetByEmailAsync(email);

            Assert.That(actual, Is.EqualTo(expected).Using(new ParticipantModelEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task ParticipantService_GetAll_ReturnsAllParticipants()
        {
            var expected = expectedParticipantModels;

            var actual = await participantService.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected).Using(new ParticipantModelEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task ParticipantService_AddAsync_AddsModel()
        {

            var participantModel = new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = "participant5@example.com", EventsIds = new List<int>() };

            await participantService.AddAsync(participantModel);

            var participantModels = await participantService.GetAllAsync();

            Assert.That(participantModels, Has.Member(participantModel).Using(new ParticipantModelEqualityComparer()), message: "AddAsync method works incorrect");
        }

        [TestCaseSource(nameof(newParticipantModels))]
        public async Task ParticipantService_AddAsync_AllParticipantsHaveValidEmails(ParticipantModel participantModel)
        {
            await participantService.AddAsync(participantModel);

            var participantModels = await participantService.GetAllAsync();

            Assert.That(participantModels, Has.Some.Property("Email").Match(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").IgnoreCase, message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task SpeakerService_AddAsync_EventCatalogExceptionWithInvalidEmail()
        {

            var participantModel = new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = "participant5example.com", EventsIds = new List<int>() };

            Assert.ThrowsAsync<EventCatalogException>(async () => await participantService.AddAsync(participantModel), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task ParticipantService_AddAsync_EventCatalogExceptionWithEmptyInfoField()
        {
            var participantModel = new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = string.Empty, EventsIds = new List<int>() };

            Assert.ThrowsAsync<EventCatalogException>(async () => await participantService.AddAsync(participantModel), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task ParticipantService_AddAsync_EventCatalogExceptionWithNullObject()
        {
            Assert.ThrowsAsync<EventCatalogException>(async () => await participantService.AddAsync(null), message: "AddAsync method works incorrect");
        }


        [TestCase(3)]
        [TestCase(100)]
        public async Task ParticipantService_DeleteAsync_DeletesParticipant(int id)
        {
            var participantModel = await participantService.GetByIdAsync(id);

            await participantService.DeleteAsync(id);

            var participantModels = await participantService.GetAllAsync();

            Assert.That(participantModels, Has.No.Member(participantModel).Using(new ParticipantModelEqualityComparer()), message: "DeleteAsync method works incorrect");
        }
        

        [Test]
        public async Task ParticipantService_UpdateAsync_EventCatalogExceptionWithEmptyInfoField()
        {
            var participantModel = await participantService.GetByIdAsync(1);

            participantModel.Email = string.Empty;

            Assert.ThrowsAsync<EventCatalogException>(async () => await participantService.UpdateAsync(participantModel), message: "UpdateAsync method works incorrect");
        }

        public static object[] newParticipantModels =
        {
            new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = "participant5@example.com", EventsIds = new List<int>() },
            new ParticipantModel { Id = 6, Name = "ParticipantName6", Email = "participant6@example.com", EventsIds = new List<int>() }
        };
    }

}

