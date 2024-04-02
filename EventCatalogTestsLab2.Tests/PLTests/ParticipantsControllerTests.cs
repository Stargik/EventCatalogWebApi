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
    [Collection("PLTests")]
    public class ParticipantsControllerTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private ParticipantService participantService;
        private EventService eventService;
        private ParticipantsController participantController;
        private IEnumerable<ParticipantModel> expectedParticipantModels;

        public ParticipantsControllerTests(EventCatalogFixture fixture)
        {
            expectedParticipantModels = fixture.ExpectedParticipantModels;

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
            Thread.Sleep(1000);
        }

        public void Dispose()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task ParticipantsController_Get_ReturnOkObjectResult()
        {
            var actual = await participantController.Get();

            Assert.IsType<OkObjectResult>(actual.Result);
        }

        [Fact]
        public async Task ParticipantsController_Get_ReturnCorrectModelType()
        {
            var actual = await participantController.Get();

            Assert.IsAssignableFrom<IEnumerable<ParticipantModel>>(((OkObjectResult)actual.Result).Value);
        }

        [Fact]
        public async Task ParticipantsController_Get_ReturnCorrectModelContent()
        {
            var expected = expectedParticipantModels;

            var actual = ((OkObjectResult)(await participantController.Get()).Result).Value;

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ParticipantsController_GetWithIdParam_ReturnOkObjectResult(int id)
        {
            var actual = await participantController.Get(id);

            Assert.IsType<OkObjectResult>(actual.Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ParticipantsController_GetWithIdParam_ReturnCorrectModelType(int id)
        {
            var actual = await participantController.Get(id);

            Assert.IsAssignableFrom<ParticipantModel>(((OkObjectResult)actual.Result).Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ParticipantsController_GetWithIdParam_ReturnCorrectModelContent(int id)
        {
            var expected = expectedParticipantModels.FirstOrDefault(x => x.Id == id);

            var actual = ((OkObjectResult)(await participantController.Get(id)).Result).Value;

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(101)]
        [InlineData(102)]
        public async Task ParticipantsController_GetWithIdParam_ReturnNotFoundResult(int id)
        {
            var actual = await participantController.Get(id);

            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Theory]
        [MemberData(nameof(NewParticipantModels))]
        public async Task ParticipantsController_Add_ReturnOkResult(ParticipantModel participantModel)
        {
            var actual = await participantController.Add(participantModel);

            Assert.IsType<OkResult>(actual);
        }

        [Theory]
        [MemberData(nameof(NewParticipantModels))]
        public async Task ParticipantsController_Add_AddsModel(ParticipantModel participantModel)
        {
            await participantController.Add(participantModel);

            var participantModels = (List<ParticipantModel>)(((OkObjectResult)(await participantController.Get()).Result).Value);

            participantModels.Should().ContainEquivalentOf(participantModel);
        }

        public static IEnumerable<object[]> NewParticipantModels => new List<object[]>
        {
            new object[] { new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = "participant5@example.com", EventsIds = new List<int>() } },
            new object[] { new ParticipantModel { Id = 6, Name = "ParticipantName6", Email = "participant6@example.com", EventsIds = new List<int>() } }
        };
    }
}

