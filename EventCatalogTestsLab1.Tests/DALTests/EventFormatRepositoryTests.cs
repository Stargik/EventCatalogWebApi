using System;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace EventCatalogTestsLab1.Tests.DALTests
{
    [TestFixture]
    public class EventFormatRepositoryTests
    {
        private EventCatalogDbContext context;
        private EventFormatRepository eventFormatRepository;
        private IEnumerable<EventFormat> expectedEventFormats;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectedEventFormats = new List<EventFormat>
            {
                new EventFormat { Id = 1, Format = "Online"},
                new EventFormat { Id = 2, Format = "Ofline"}
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

            eventFormatRepository = new EventFormatRepository(context);
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
        [TestCase(2)]
        public async Task EventFormatRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedEventFormats.FirstOrDefault(x => x.Id == id);

            var actual = await eventFormatRepository.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new EventFormatEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task EventFormatRepository_GetAllAsync_ReturnsAllEventFormats()
        {
            var expected = expectedEventFormats;

            var actual = await eventFormatRepository.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected).Using(new EventFormatEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCaseSource(nameof(newFormats))]
        public async Task EventFormatRepository_AddAsync_AddsValueToDatabase(EventFormat format)
        {
            await eventFormatRepository.AddAsync(format);
            await context.SaveChangesAsync();

            var formats = await eventFormatRepository.GetAllAsync();

            Assert.That(formats, Has.Member(format).Using(new EventFormatEqualityComparer()), message: "AddAsync method works incorrect");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task EventFormatRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var format = await eventFormatRepository.GetByIdAsync(id);

            await eventFormatRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var formats = await eventFormatRepository.GetAllAsync();

            Assert.That(formats, Has.No.Member(format).Using(new EventFormatEqualityComparer()), message: "DeleteByIdAsync works incorrect");
        }

        [Test]
        public async Task EventFormatRepository_UpdateAsync_UpdatesEntity()
        {
            var format = await eventFormatRepository.GetByIdWithDetailsAsync(1);
            format.Format = "Virtual";

            await eventFormatRepository.UpdateAsync(format);
            await context.SaveChangesAsync();

            var formats = await eventFormatRepository.GetAllAsync();
            var newFormat = new EventFormat { Id = 1, Format = "Virtual" };

            Assert.That(formats, Has.Member(newFormat).Using(new EventFormatEqualityComparer()), message: "UpdateAsync method works incorrect");
        }

        public static object[] newFormats =
        {
            new EventFormat { Id = 3, Format = "Virtual" },
            new EventFormat { Id = 4, Format = "Zoom meeting" }
        };
    }
}

