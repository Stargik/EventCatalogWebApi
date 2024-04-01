using System;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab2.Tests.DALTests
{
	public class EventSubjectCategoryRepositoryTests : IDisposable, IClassFixture<EventCatalogFixture>
    {
        private EventCatalogDbContext context;
        private EventSubjectCategoryRepository eventSubjectCategoryRepository;
        private IEnumerable<EventSubjectCategory> expectedEventSubjectCategories;

        public EventSubjectCategoryRepositoryTests(EventCatalogFixture fixture)
        {
            expectedEventSubjectCategories = fixture.ExpectedEventSubjectCategories;

            context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());

            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
                UnitTestHelper.SeedData(context);
            }

            eventSubjectCategoryRepository = new EventSubjectCategoryRepository(context);
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
        [InlineData(3)]
        public async Task EventSubjectCategoryRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedEventSubjectCategories.FirstOrDefault(x => x.Id == id);

            var actual = await eventSubjectCategoryRepository.GetByIdAsync(id);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task EventSubjectCategoryRepository_GetAllAsync_ReturnsAllEventSubjectCategories()
        {
            var expected = expectedEventSubjectCategories;

            var actual = await eventSubjectCategoryRepository.GetAllAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(NewCatagories))]
        public async Task EventSubjectCategoryRepository_AddAsync_AddsValueToDatabase(EventSubjectCategory category)
        {
            await eventSubjectCategoryRepository.AddAsync(category);
            await context.SaveChangesAsync();

            var categories = await eventSubjectCategoryRepository.GetAllAsync();

            categories.Should().ContainEquivalentOf(category);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async Task EventSubjectCategoryRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var category = await eventSubjectCategoryRepository.GetByIdAsync(id);

            await eventSubjectCategoryRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var categories = await eventSubjectCategoryRepository.GetAllAsync();

            categories.Should().NotContainEquivalentOf(category);
        }

        [Fact]
        public async Task EventSubjectCategoryRepository_UpdateAsync_UpdatesEntity()
        {
            var category = await eventSubjectCategoryRepository.GetByIdWithDetailsAsync(1);
            category.Title = "newTitle";

            await eventSubjectCategoryRepository.UpdateAsync(category);
            await context.SaveChangesAsync();

            var categories = await eventSubjectCategoryRepository.GetAllAsync();
            var newCategory = new EventSubjectCategory { Id = 1, Title = "newTitle" };

            categories.Should().ContainEquivalentOf(newCategory);
        }

        public static IEnumerable<object[]> NewCatagories => new List<object[]>
        {
            new object[] { new EventSubjectCategory { Id = 4, Title = "Category4" } },
            new object[] { new EventSubjectCategory { Id = 5, Title = "Category5" } }
        };
    }
}
