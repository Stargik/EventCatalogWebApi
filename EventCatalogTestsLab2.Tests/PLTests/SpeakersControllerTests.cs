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
	public class SpeakersControllerTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private SpeakerService speakerService;
        private SpeakersController speakersController;
        private IEnumerable<SpeakerModel> expectedSpeakerModels;

        public SpeakersControllerTests(EventCatalogFixture fixture)
        {
            expectedSpeakerModels = fixture.ExpectedSpeakerModels;

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

        public void Dispose()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task SpeakersController_Get_ReturnOkObjectResult()
        {
            var actual = await speakersController.Get();

            Assert.IsType<OkObjectResult>(actual.Result);
        }

        [Fact]
        public async Task SpeakersController_Get_ReturnCorrectModelType()
        {
            var actual = await speakersController.Get();

            Assert.IsAssignableFrom<IEnumerable<SpeakerModel>>(((OkObjectResult)actual.Result).Value);
        }

        [Fact]
        public async Task SpeakersController_Get_ReturnCorrectModelContent()
        {
            var expected = expectedSpeakerModels;

            var actual = ((OkObjectResult)(await speakersController.Get()).Result).Value;

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task SpeakersController_GetWithIdParam_ReturnOkObjectResult(int id)
        {
            var actual = await speakersController.Get(id);

            Assert.IsType<OkObjectResult>(actual.Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task SpeakersController_GetWithIdParam_ReturnCorrectModelType(int id)
        {
            var actual = await speakersController.Get(id);

            Assert.IsAssignableFrom<SpeakerModel>(((OkObjectResult)actual.Result).Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task SpeakersController_GetWithIdParam_ReturnCorrectModelContent(int id)
        {
            var expected = expectedSpeakerModels.FirstOrDefault(x => x.Id == id);

            var actual = ((OkObjectResult)(await speakersController.Get(id)).Result).Value;

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(101)]
        [InlineData(102)]
        public async Task SpeakersController_GetWithIdParam_ReturnNotFoundResult(int id)
        {
            var actual = await speakersController.Get(id);

            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Theory]
        [MemberData(nameof(NewSpeakerModels))]
        public async Task SpeakersController_Add_ReturnOkResult(SpeakerModel speakerModel)
        {
            var actual = await speakersController.Add(speakerModel);

            Assert.IsType<OkResult>(actual);
        }

        [Theory]
        [MemberData(nameof(NewSpeakerModels))]
        public async Task SpeakersController_Add_AddsModel(SpeakerModel speakerModel)
        {
            await speakersController.Add(speakerModel);

            var speakerModels = (List<SpeakerModel>)(((OkObjectResult)(await speakersController.Get()).Result).Value);

            speakerModels.Should().ContainEquivalentOf(speakerModel);
        }

        public static IEnumerable<object[]> NewSpeakerModels => new List<object[]>
        {
            new object[] { new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4@example.com", EventsIds = new List<int>() } },
            new object[] { new SpeakerModel { Id = 5, FirstName = "FirstName5", LastName = "LastName5", Email = "speaker5@example.com", EventsIds = new List<int>() } }
        };
    }
}

