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
	public class SpeakersControllerTests
	{
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private SpeakerService speakerService;
        private SpeakersController speakersController;
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
            speakersController = new SpeakersController(speakerService);
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
        public async Task SpeakersController_Get_ReturnOkObjectResult()
        {
            var actual = await speakersController.Get();

            Assert.IsInstanceOf<OkObjectResult>(actual.Result, message: "Get request works incorrect");
        }

        [Test]
        public async Task SpeakersController_Get_ReturnCorrectModelType()
        {
            var actual = await speakersController.Get();

            Assert.IsInstanceOf<IEnumerable<SpeakerModel>>(((OkObjectResult)actual.Result).Value, message: "Get request works incorrect");
        }

        [Test]
        public async Task SpeakersController_Get_ReturnCorrectModelContent()
        {
            var expected = expectedSpeakerModels;

            var actual = ((OkObjectResult)(await speakersController.Get()).Result).Value;

            Assert.That(actual, Is.EquivalentTo(expected).Using(new SpeakerModelEqualityComparer()), message: "Get request works incorrect");
        }

        [Test]
        public async Task SpeakersController_GetWithIdParam_ReturnOkObjectResult([Range(1, 3, 1)] int id)
        {
            var actual = await speakersController.Get(id);

            Assert.IsInstanceOf<OkObjectResult>(actual.Result, message: "Get request works incorrect");
        }

        [Test]
        public async Task SpeakersController_GetWithIdParam_ReturnCorrectModelType([Range(1, 3, 1)] int id)
        {
            var actual = await speakersController.Get(id);

            Assert.IsInstanceOf<SpeakerModel>(((OkObjectResult)actual.Result).Value, message: "Get request works incorrect");
        }

        [Test]
        public async Task SpeakersController_GetWithIdParam_ReturnCorrectModelContent([Range(1, 3, 1)] int id)
        {
            var expected = expectedSpeakerModels.FirstOrDefault(x => x.Id == id);

            var actual = ((OkObjectResult)(await speakersController.Get(id)).Result).Value;

            Assert.That(actual, Is.EqualTo(expected).Using(new SpeakerModelEqualityComparer()), message: "Get request works incorrect");
        }

        [Test]
        public async Task SpeakersController_GetWithIdParam_ReturnNotFoundResult([Range(100, 106, 3)] int id)
        {
            var actual = await speakersController.Get(id);

            Assert.IsInstanceOf<NotFoundResult>(actual.Result, message: "Get method works incorrect");
        }

        [TestCaseSource(nameof(newSpeakerModels))]
        public async Task SpeakersController_Add_ReturnOkResult(SpeakerModel speakerModel)
        {
            var actual = await speakersController.Add(speakerModel);

            Assert.IsInstanceOf<OkResult>(actual, message: "Add request works incorrect");
        }

        [TestCaseSource(nameof(newSpeakerModels))]
        public async Task SpeakersController_Add_AddsModel(SpeakerModel speakerModel)
        {
            await speakersController.Add(speakerModel);

            var speakerModels = ((OkObjectResult)(await speakersController.Get()).Result).Value;

            Assert.That(speakerModels, Has.Member(speakerModel).Using(new SpeakerModelEqualityComparer()), message: "Add request works incorrect");
        }

        public static object[] newSpeakerModels =
        {
            new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4@example.com", EventsIds = new List<int>() },
            new SpeakerModel { Id = 5, FirstName = "FirstName5", LastName = "LastName5", Email = "speaker5@example.com", EventsIds = new List<int>() }
        };
    }
}

