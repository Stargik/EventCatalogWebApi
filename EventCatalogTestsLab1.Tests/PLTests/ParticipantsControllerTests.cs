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
	public class ParticipantsControllerTests
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private ParticipantService participantService;
        private EventService eventService;
        private ParticipantsController participantController;
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
            eventService = new EventService(unitOfWork, UnitTestHelper.GetAutoMapperProfile());
            participantController = new ParticipantsController(participantService, eventService);
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
        public async Task ParticipantsController_Get_ReturnOkObjectResult()
        {
            var actual = await participantController.Get();

            Assert.IsInstanceOf<OkObjectResult>(actual.Result, message: "Get request works incorrect");
        }

        [Test]
        public async Task ParticipantsController_Get_ReturnCorrectModelType()
        {
            var actual = await participantController.Get();

            Assert.IsInstanceOf<IEnumerable<ParticipantModel>>(((OkObjectResult)actual.Result).Value, message: "Get request works incorrect");
        }

        [Test]
        public async Task ParticipantsController_Get_ReturnCorrectModelContent()
        {
            var expected = expectedParticipantModels;

            var actual = ((OkObjectResult)(await participantController.Get()).Result).Value;

            Assert.That(actual, Is.EquivalentTo(expected).Using(new ParticipantModelEqualityComparer()), message: "Get request works incorrect");
        }

        [Test]
        public async Task ParticipantsController_GetWithIdParam_ReturnOkObjectResult([Range(1, 3, 1)] int id)
        {
            var actual = await participantController.Get(id);

            Assert.IsInstanceOf<OkObjectResult>(actual.Result, message: "Get request works incorrect");
        }

        [Test]
        public async Task ParticipantsController_GetWithIdParam_ReturnCorrectModelType([Range(1, 3, 1)] int id)
        {
            var actual = await participantController.Get(id);

            Assert.IsInstanceOf<ParticipantModel>(((OkObjectResult)actual.Result).Value, message: "Get request works incorrect");
        }

        [Test]
        public async Task ParticipantsController_GetWithIdParam_ReturnCorrectModelContent([Range(1, 3, 1)] int id)
        {
            var expected = expectedParticipantModels.FirstOrDefault(x => x.Id == id);

            var actual = ((OkObjectResult)(await participantController.Get(id)).Result).Value;

            Assert.That(actual, Is.EqualTo(expected).Using(new ParticipantModelEqualityComparer()), message: "Get request works incorrect");
        }

        [Test]
        public async Task ParticipantsController_GetWithIdParam_ReturnNotFoundResult([Range(100, 106, 3)] int id)
        {
            var actual = await participantController.Get(id);

            Assert.IsInstanceOf<NotFoundResult>(actual.Result, message: "Get method works incorrect");
        }

        [TestCaseSource(nameof(newParticipantModels))]
        public async Task ParticipantsController_Add_ReturnOkResult(ParticipantModel participantModel)
        {
            var actual = await participantController.Add(participantModel);

            Assert.IsInstanceOf<OkResult>(actual, message: "Add request works incorrect");
        }

        [TestCaseSource(nameof(newParticipantModels))]
        public async Task ParticipantsController_Add_AddsModel(ParticipantModel participantModel)
        {
            await participantController.Add(participantModel);

            var participantModels = ((OkObjectResult)(await participantController.Get()).Result).Value;

            Assert.That(participantModels, Has.Member(participantModel).Using(new ParticipantModelEqualityComparer()), message: "Add request works incorrect");
        }

        public static object[] newParticipantModels =
        {
            new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = "participant5@example.com", EventsIds = new List<int>() },
            new ParticipantModel { Id = 6, Name = "ParticipantName6", Email = "participant6@example.com", EventsIds = new List<int>() }
        };
    }
}

