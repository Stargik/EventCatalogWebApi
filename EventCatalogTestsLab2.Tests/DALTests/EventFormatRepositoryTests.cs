using System;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace EventCatalogTestsLab2.Tests.DALTests
{
    [Collection("DALTests")]
    public class EventFormatRepositoryTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private EventFormatRepository eventFormatRepository;
        private IEnumerable<EventFormat> expectedEventFormats;

        public EventFormatRepositoryTests(EventCatalogFixture fixture)
        {
            expectedEventFormats = fixture.ExpectedEventFormats;

            context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
                UnitTestHelper.SeedData(context);
            }

            eventFormatRepository = new EventFormatRepository(context);
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
        public async Task EventFormatRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedEventFormats.FirstOrDefault(x => x.Id == id);

            var actual = await eventFormatRepository.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task EventFormatRepository_GetAllAsync_ReturnsAllEventFormats()
        {
            var expected = expectedEventFormats;

            var actual = await eventFormatRepository.GetAllAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(NewFormats))]
        public async Task EventFormatRepository_AddAsync_AddsValueToDatabase(EventFormat format)
        {
            await eventFormatRepository.AddAsync(format);
            await context.SaveChangesAsync();

            var formats = await eventFormatRepository.GetAllAsync();

            formats.Should().ContainEquivalentOf(format);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task EventFormatRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var format = await eventFormatRepository.GetByIdAsync(id);

            await eventFormatRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var formats = await eventFormatRepository.GetAllAsync();

            formats.Should().NotContainEquivalentOf(format);
        }

        [Fact]
        public async Task EventFormatRepository_UpdateAsync_UpdatesEntity()
        {
            var format = await eventFormatRepository.GetByIdWithDetailsAsync(1);
            format.Format = "Virtual";

            await eventFormatRepository.UpdateAsync(format);
            await context.SaveChangesAsync();

            var formats = await eventFormatRepository.GetAllAsync();
            var newFormat = new EventFormat { Id = 1, Format = "Virtual" };

            formats.Should().ContainEquivalentOf(newFormat);
        }

        public static IEnumerable<object[]> NewFormats => new List<object[]>
        {
            new object[] { new EventFormat { Id = 3, Format = "Virtual" } },
            new object[] { new EventFormat { Id = 4, Format = "Zoom meeting" } }
        };
    }
}

