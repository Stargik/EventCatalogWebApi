using System;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab1.Tests.DALTests
{
	public class EventSubjectCategoryRepositoryTests
    {
        private EventCatalogDbContext context;
        private EventSubjectCategoryRepository eventSubjectCategoryRepository;
        private IEnumerable<EventSubjectCategory> expectedEventSubjectCategories;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectedEventSubjectCategories = new List<EventSubjectCategory>
            {                
                new EventSubjectCategory { Id = 1, Title = "Category1"},
                new EventSubjectCategory { Id = 2, Title = "Category2" },
                new EventSubjectCategory { Id = 3, Title = "Category3" }
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

            eventSubjectCategoryRepository = new EventSubjectCategoryRepository(context);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestCase(1)]
        [TestCase(3)]
        public async Task EventSubjectCategoryRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedEventSubjectCategories.FirstOrDefault(x => x.Id == id);

            var actual = await eventSubjectCategoryRepository.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new EventSubjectCategoryEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task EventSubjectCategoryRepository_GetAllAsync_ReturnsAllEventSubjectCategories()
        {
            var expected = expectedEventSubjectCategories;

            var actual = await eventSubjectCategoryRepository.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected).Using(new EventSubjectCategoryEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCaseSource(nameof(newCatagories))]
        public async Task EventSubjectCategoryRepository_AddAsync_AddsValueToDatabase(EventSubjectCategory category)
        {
            await eventSubjectCategoryRepository.AddAsync(category);
            await context.SaveChangesAsync();

            var categories = await eventSubjectCategoryRepository.GetAllAsync();

            Assert.That(categories, Has.Member(category).Using(new EventSubjectCategoryEqualityComparer()), message: "AddAsync method works incorrect");
        }

        [TestCase(1)]
        [TestCase(3)]
        public async Task EventSubjectCategoryRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var category = await eventSubjectCategoryRepository.GetByIdAsync(id);

            await eventSubjectCategoryRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var categories = await eventSubjectCategoryRepository.GetAllAsync();

            Assert.That(categories, Has.No.Member(category).Using(new EventSubjectCategoryEqualityComparer()), message: "DeleteByIdAsync works incorrect");
        }

        [Test]
        public async Task EventSubjectCategoryRepository_UpdateAsync_UpdatesEntity()
        {
            var category = await eventSubjectCategoryRepository.GetByIdWithDetailsAsync(1);
            category.Title = "newTitle";

            await eventSubjectCategoryRepository.UpdateAsync(category);
            await context.SaveChangesAsync();

            var categories = await eventSubjectCategoryRepository.GetAllAsync();
            var newCategory = new EventSubjectCategory { Id = 1, Title = "newTitle" };

            Assert.That(categories, Has.Member(newCategory).Using(new EventSubjectCategoryEqualityComparer()), message: "UpdateAsync method works incorrect");
        }

        public static object[] newCatagories =
        {
            new EventSubjectCategory { Id = 4, Title = "Category4" },
            new EventSubjectCategory { Id = 5, Title = "Category5" }
        };
    }
}

