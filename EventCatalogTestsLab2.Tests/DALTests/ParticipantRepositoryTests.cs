using System;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab2.Tests.DALTests
{
    [Collection("DALTests")]
    public class ParticipantRepositoryTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private ParticipantRepository participantRepository;
        private IEnumerable<Participant> expectedParticipants;

        public ParticipantRepositoryTests(EventCatalogFixture fixture)
        {
            expectedParticipants = fixture.ExpectedParticipants;

            context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
                UnitTestHelper.SeedData(context);
            }

            participantRepository = new ParticipantRepository(context);
        }

        public void Dispose()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task ParticipantRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedParticipants.FirstOrDefault(x => x.Id == id);

            var actual = await participantRepository.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ParticipantRepository_GetAllAsync_ReturnsAllParticipants()
        {
            var expected = expectedParticipants;

            var actual = await participantRepository.GetAllAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(NewParticipants))]
        public async Task ParticipantRepository_AddAsync_AddsValueToDatabase(Participant participant)
        {
            await participantRepository.AddAsync(participant);
            await context.SaveChangesAsync();

            var participants = await participantRepository.GetAllAsync();

            participants.Should().ContainEquivalentOf(participant);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task ParticipantRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var participant = await participantRepository.GetByIdAsync(id);

            await participantRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var participants = await participantRepository.GetAllAsync();

            participants.Should().NotContainEquivalentOf(participant);
        }

        [Fact]
        public async Task ParticipantRepository_UpdateAsync_UpdatesEntity()
        {
            var participant = await participantRepository.GetByIdWithDetailsAsync(1);
            participant.Email = "newEmail@example.com";

            await participantRepository.UpdateAsync(participant);
            await context.SaveChangesAsync();

            var participants = await participantRepository.GetAllAsync();
            var newParticipant = new Participant { Id = 1, Name = "ParticipantName1", Email = "newEmail@example.com" };

            participants.Should().ContainEquivalentOf(newParticipant);
        }

        public static IEnumerable<object[]> NewParticipants => new List<object[]>
        {
            new object[] { new Participant { Id = 5, Name = "ParticipantName5", Email = "participant5@example.com" } },
            new object[] { new Participant { Id = 6, Name = "ParticipantName6", Email = "participant6@example.com" } }
        };
    }
}
