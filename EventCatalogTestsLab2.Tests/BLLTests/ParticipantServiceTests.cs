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
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab2.Tests.BLLTests
{
    [Collection("BLLTests")]
    public class ParticipantServiceTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private ParticipantService participantService;
        private IEnumerable<ParticipantModel> expectedParticipantModels;

        public ParticipantServiceTests(EventCatalogFixture fixture)
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
            Thread.Sleep(1000);
        }

        public void Dispose()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ParticipantService_GetById_ReturnsParticipantModel(int id)
        {
            var expected = expectedParticipantModels.FirstOrDefault(x => x.Id == id);

            var actual = await participantService.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("participant1@example.com")]
        [InlineData("participant2@example.com")]
        public async Task ParticipantService_GetByEmailAsync_ReturnsParticipantModel(string email)
        {
            var expected = expectedParticipantModels.FirstOrDefault(x => x.Email == email);

            var actual = await participantService.GetByEmailAsync(email);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ParticipantService_GetAll_ReturnsAllParticipants()
        {
            var expected = expectedParticipantModels;

            var actual = await participantService.GetAllAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ParticipantService_AddAsync_AddsModel()
        {

            var participantModel = new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = "participant5@example.com", EventsIds = new List<int>() };

            await participantService.AddAsync(participantModel);

            var participantModels = await participantService.GetAllAsync();

            participantModels.Should().ContainEquivalentOf(participantModel);
        }

        [Theory]
        [MemberData(nameof(NewParticipantModels))]
        public async Task ParticipantService_AddAsync_AllParticipantsHaveValidEmails(ParticipantModel participantModel)
        {
            await participantService.AddAsync(participantModel);

            var participantModels = await participantService.GetAllAsync();

            participantModels.Should().AllSatisfy(x => x.Email.Should().MatchRegex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$"));
        }

        [Fact]
        public async Task SpeakerService_AddAsync_EventCatalogExceptionWithInvalidEmail()
        {
            var participantModel = new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = "participant5example.com", EventsIds = new List<int>() };

            Func<Task> act = async () => await participantService.AddAsync(participantModel);

            await act.Should().ThrowAsync<EventCatalogException>();
        }

        [Fact]
        public async Task ParticipantService_AddAsync_EventCatalogExceptionWithEmptyInfoField()
        {
            var participantModel = new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = string.Empty, EventsIds = new List<int>() };

            Func<Task> act = async () => await participantService.AddAsync(participantModel);

            await act.Should().ThrowAsync<EventCatalogException>();
        }

        [Fact]
        public async Task ParticipantService_AddAsync_EventCatalogExceptionWithNullObject()
        {
            Func<Task> act = async () => await participantService.AddAsync(null);

            await act.Should().ThrowAsync<EventCatalogException>();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(100)]
        public async Task ParticipantService_DeleteAsync_DeletesParticipant(int id)
        {
            var participantModel = await participantService.GetByIdAsync(id);

            await participantService.DeleteAsync(id);

            var participantModels = await participantService.GetAllAsync();

            participantModels.Should().NotContainEquivalentOf(participantModel);
        }


        [Fact]
        public async Task ParticipantService_UpdateAsync_EventCatalogExceptionWithEmptyInfoField()
        {
            var participantModel = await participantService.GetByIdAsync(1);

            participantModel.Email = string.Empty;

            Func<Task> act = async () => await participantService.UpdateAsync(participantModel);

            await act.Should().ThrowAsync<EventCatalogException>();
        }

        public static IEnumerable<object[]> NewParticipantModels => new List<object[]>
        {
            new object[] { new ParticipantModel { Id = 5, Name = "ParticipantName5", Email = "participant5@example.com", EventsIds = new List<int>() } },
            new object[] { new ParticipantModel { Id = 6, Name = "ParticipantName6", Email = "participant6@example.com", EventsIds = new List<int>() } }
        };
    }

}

