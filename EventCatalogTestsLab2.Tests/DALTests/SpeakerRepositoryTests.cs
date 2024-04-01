using System;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab2.Tests.DALTests
{
    public class SpeakerRepositoryTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private SpeakerRepository speakerRepository;
        private IEnumerable<Speaker> expectedSpeakers;

        public SpeakerRepositoryTests(EventCatalogFixture fixture)
        {
            expectedSpeakers = fixture.ExpectedSpeakers;

            context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
                UnitTestHelper.SeedData(context);
            }

            speakerRepository = new SpeakerRepository(context);
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
        public async Task SpeakerRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedSpeakers.FirstOrDefault(x => x.Id == id);

            var actual = await speakerRepository.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SpeakerRepository_GetAllAsync_ReturnsAllSpeakers()
        {
            var expected = expectedSpeakers;

            var actual = await speakerRepository.GetAllAsync();

            actual.Should().BeEquivalentTo(expected);
        }
        
        [Theory]
        [MemberData(nameof(NewSpeakers))]
        public async Task SpeakerRepository_AddAsync_AddsValueToDatabase(Speaker speaker)
        {
            await speakerRepository.AddAsync(speaker);
            await context.SaveChangesAsync();

            var speakers = await speakerRepository.GetAllAsync();

            speakers.Should().ContainEquivalentOf(speaker);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task SpeakerRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var speaker = await speakerRepository.GetByIdAsync(id);

            await speakerRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var speakers = await speakerRepository.GetAllAsync();

            speakers.Should().NotContainEquivalentOf(speaker);
        }
        
        [Fact]
        public async Task SpeakerRepository_UpdateAsync_UpdatesEntity()
        {
            var speaker = await speakerRepository.GetByIdWithDetailsAsync(1);
            speaker.Email = "newEmail@example.com";

            await speakerRepository.UpdateAsync(speaker);
            await context.SaveChangesAsync();

            var speakers = await speakerRepository.GetAllAsync();
            var newSpeaker = new Speaker { Id = 1, FirstName = "FirstName1", LastName = "LastName1", Email = "newEmail@example.com" };

            speakers.Should().ContainEquivalentOf(newSpeaker);
        }

        public static IEnumerable<object[]> NewSpeakers => new List<object[]>
        {
            new object[] { new Speaker { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4@example.com" } },
            new object[] { new Speaker { Id = 5, FirstName = "FirstName5", LastName = "LastName5", Email = "speaker5@example.com" } }
        };
    }
}

