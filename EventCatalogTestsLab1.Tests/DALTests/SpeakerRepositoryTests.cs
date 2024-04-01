using System;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab1.Tests.DALTests
{
    [TestFixture]
    public class SpeakerRepositoryTests
    {
        private EventCatalogDbContext context;
        private SpeakerRepository speakerRepository;
        private IEnumerable<Speaker> expectedSpeakers;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectedSpeakers = new List<Speaker>
            {
                new Speaker { Id = 1, FirstName = "FirstName1", LastName = "LastName1", Email = "speaker1@example.com" },
                new Speaker { Id = 2, FirstName = "FirstName2", LastName = "LastName2", Email = "speaker2@example.com" },
                new Speaker { Id = 3, FirstName = "FirstName3", LastName = "LastName3", Email = "speaker3@example.com" }
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

            speakerRepository = new SpeakerRepository(context);
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
        public async Task SpeakerRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedSpeakers.FirstOrDefault(x => x.Id == id);

            var actual = await speakerRepository.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new SpeakerEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task SpeakerRepository_GetAllAsync_ReturnsAllSpeakers()
        {
            var expected = expectedSpeakers;

            var actual = await speakerRepository.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected).Using(new SpeakerEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCaseSource(nameof(newSpeakers))]
        public async Task SpeakerRepository_AddAsync_AddsValueToDatabase(Speaker speaker)
        {
            await speakerRepository.AddAsync(speaker);
            await context.SaveChangesAsync();

            var speakers = await speakerRepository.GetAllAsync();

            Assert.That(speakers, Has.Member(speaker).Using(new SpeakerEqualityComparer()), message: "AddAsync method works incorrect");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task SpeakerRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var speaker = await speakerRepository.GetByIdAsync(id);

            await speakerRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var speakers = await speakerRepository.GetAllAsync();

            Assert.That(speakers, Has.No.Member(speaker).Using(new SpeakerEqualityComparer()), message: "DeleteByIdAsync works incorrect");
        }

        [Test]
        public async Task SpeakerRepository_UpdateAsync_UpdatesEntity()
        {
            var speaker = await speakerRepository.GetByIdWithDetailsAsync(1);
            speaker.Email = "newEmail@example.com";

            await speakerRepository.UpdateAsync(speaker);
            await context.SaveChangesAsync();

            var speakers = await speakerRepository.GetAllAsync();
            var newSpeaker = new Speaker { Id = 1, FirstName = "FirstName1", LastName = "LastName1", Email = "newEmail@example.com" };

            Assert.That(speakers, Has.Member(newSpeaker).Using(new SpeakerEqualityComparer()), message: "UpdateAsync method works incorrect");
        }

        public static object[] newSpeakers =
        {
            new Speaker { Id = 4, FirstName = "FirstName4", LastName = "LastName4", Email = "speaker4@example.com" },
            new Speaker { Id = 5, FirstName = "FirstName5", LastName = "LastName5", Email = "speaker5@example.com" },
        };
    }
}

