using System;
using System.Reflection;
using System.Text.RegularExpressions;
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
    public class SpeakerServiceTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private UnitOfWork unitOfWork;
        private SpeakerService speakerService;
        private IEnumerable<SpeakerModel> expectedSpeakerModels;

        public SpeakerServiceTests(EventCatalogFixture fixture)
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
        public async Task SpeakerService_GetById_ReturnsSpeakerModel(int id)
        {
            var expected = expectedSpeakerModels.FirstOrDefault(x => x.Id == id);

            var actual = await speakerService.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task SpeakerService_GetSpeakerByEventIdAsync_ReturnsSpeakerModel(int eventId)
        {
            var expected = expectedSpeakerModels.FirstOrDefault(x => x.EventsIds.Contains(eventId));

            var actual = await speakerService.GetSpeakerByEventIdAsync(eventId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SpeakerService_GetAll_ReturnsAllSpeakers()
        {
            var expected = expectedSpeakerModels;

            var actual = await speakerService.GetAllAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SpeakerService_AddAsync_AddsModel()
        {

            var speakerModel = new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4@example.com", EventsIds = new List<int>() };

            await speakerService.AddAsync(speakerModel);

            var speakerModels = await speakerService.GetAllAsync();

            speakerModels.Should().ContainEquivalentOf(speakerModel);
        }

        [Theory]
        [MemberData(nameof(NewSpeakerModels))]
        public async Task SpeakerService_AddAsync_AllParticipantsHaveValidEmails(SpeakerModel speakerModel)
        {
            await speakerService.AddAsync(speakerModel);

            var speakerModels = await speakerService.GetAllAsync();

            speakerModels.Should().AllSatisfy(x => x.Email.Should().MatchRegex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$"));
        }

        [Fact]
        public async Task SpeakerService_AddAsync_EventCatalogExceptionWithInvalidEmail()
        {

            var speakerModel = new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4example.com", EventsIds = new List<int>() };

            Func<Task> act = async () => await speakerService.AddAsync(speakerModel);

            await act.Should().ThrowAsync<EventCatalogException>();
        }

        [Fact]
        public async Task SpeakerService_AddAsync_EventCatalogExceptionWithEmptyInfoField()
        {
            var speakerModel = new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = String.Empty, EventsIds = new List<int>() };

            Func<Task> act = async () => await speakerService.AddAsync(speakerModel);

            await act.Should().ThrowAsync<EventCatalogException>();
        }

        [Fact]
        public async Task SpeakerService_AddAsync_EventCatalogExceptionWithNullObject()
        {
            Func<Task> act = async () => await speakerService.AddAsync(null);

            await act.Should().ThrowAsync<EventCatalogException>();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(100)]
        public async Task SpeakerService_DeleteAsync_DeletesSpeaker(int id)
        {
            var speakerModel = await speakerService.GetByIdAsync(id);

            await speakerService.DeleteAsync(id);

            var speakerModels = await speakerService.GetAllAsync();

            speakerModels.Should().NotContainEquivalentOf(speakerModel);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task SpeakerService_DeleteAsync_EventCatalogExceptionWithSpeakerWithEvents(int id)
        {
            Func<Task> act = async () => await speakerService.DeleteAsync(id);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task SpeakerService_UpdateAsync_EventCatalogExceptionWithEmptyInfoField()
        {
            var speakerModel = await speakerService.GetByIdAsync(1);

            speakerModel.Email = string.Empty;

            Func<Task> act = async () => await speakerService.UpdateAsync(speakerModel);

            await act.Should().ThrowAsync<EventCatalogException>();
        }

        public static IEnumerable<object[]> NewSpeakerModels => new List<object[]>
        {
            new object[] { new SpeakerModel { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4@example.com", EventsIds = new List<int>() } },
            new object[] { new SpeakerModel { Id = 5, FirstName = "FirstName5", LastName = "LastName5", Email = "speaker5@example.com", EventsIds = new List<int>() } }
        };
    }

}

