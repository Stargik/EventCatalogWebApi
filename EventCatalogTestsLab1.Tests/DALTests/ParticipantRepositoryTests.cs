using System;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventCatalogTestsLab1.Tests.DALTests
{
	public class ParticipantRepositoryTests
	{
        private EventCatalogDbContext context;
        private ParticipantRepository participantRepository;
        private IEnumerable<Participant> expectedParticipants;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            expectedParticipants = new List<Participant>
            {
                new Participant { Id = 1, Name="ParticipantName1", Email = "participant1@example.com" },
                new Participant { Id = 2, Name = "ParticipantName2", Email = "participant2@example.com" },
                new Participant { Id = 3, Name = "ParticipantName3", Email = "participant3@example.com" },
                new Participant { Id = 4, Name = "ParticipantName4", Email = "participant4@example.com" }
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

            participantRepository = new ParticipantRepository(context);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (context.Database.IsInMemory())
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestCase(3)]
        [TestCase(4)]
        public async Task ParticipantRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            var expected = expectedParticipants.FirstOrDefault(x => x.Id == id);

            var actual = await participantRepository.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new ParticipantEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task ParticipantRepository_GetAllAsync_ReturnsAllParticipants()
        {
            var expected = expectedParticipants;

            var actual = await participantRepository.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected).Using(new ParticipantEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCaseSource(nameof(newParticipants))]
        public async Task ParticipantRepository_AddAsync_AddsValueToDatabase(Participant participant)
        {
            await participantRepository.AddAsync(participant);
            await context.SaveChangesAsync();

            var participants = await participantRepository.GetAllAsync();

            Assert.That(participants, Has.Member(participant).Using(new ParticipantEqualityComparer()), message: "AddAsync method works incorrect");
        }

        [TestCase(3)]
        [TestCase(4)]
        public async Task ParticipantRepository_DeleteByIdAsync_DeletesEntity(int id)
        {
            var participant = await participantRepository.GetByIdAsync(id);

            await participantRepository.DeleteByIdAsync(id);
            await context.SaveChangesAsync();

            var participants = await participantRepository.GetAllAsync();

            Assert.That(participants, Has.No.Member(participant).Using(new ParticipantEqualityComparer()), message: "DeleteByIdAsync works incorrect");
        }

        [Test]
        public async Task ParticipantRepository_UpdateAsync_UpdatesEntity()
        {
            var participant = await participantRepository.GetByIdWithDetailsAsync(1);
            participant.Email = "newEmail@example.com";

            await participantRepository.UpdateAsync(participant);
            await context.SaveChangesAsync();

            var participants = await participantRepository.GetAllAsync();
            var newParticipant = new Participant { Id = 1, Name = "ParticipantName1", Email = "newEmail@example.com" };

            Assert.That(participants, Has.Member(newParticipant).Using(new ParticipantEqualityComparer()), message: "UpdateAsync method works incorrect");
        }

        public static object[] newParticipants =
        {
            new Participant { Id = 5, Name = "ParticipantName5", Email = "participant5@example.com" },
            new Participant { Id = 6, Name = "ParticipantName6", Email = "participant6@example.com" }
        };
    }
}

