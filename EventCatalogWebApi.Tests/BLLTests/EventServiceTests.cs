using BLL.Models;
using BLL.Services;
using Castle.Core.Resource;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCatalogWebApi.Tests.BLLTests
{
    internal class EventServiceTests
    {
        [Test]
        public async Task EventService_GetAllAsync_ReturnsAllEvents()
        {
            var expected = GetTestEventModels;
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.EventRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(GetTestEvents.AsEnumerable());

            var customerService = new EventService(mockUnitOfWork.Object, UnitTestHelper.GetAutoMapperProfile());

            var actual = await customerService.GetAllAsync();

            Assert.That(actual, Is.EqualTo(expected).Using(new EventModelEqualityComparer()));
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(0)]
        public async Task EventService_GetAByIdAsync_ReturnCorrectEvent(int id)
        {
            var expected = GetTestEventModels.FirstOrDefault(e => e.Id == id);
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.EventRepository.GetByIdWithDetailsAsync(id))
                .ReturnsAsync(GetTestEvents.FirstOrDefault(e => e.Id == id));

            var customerService = new EventService(mockUnitOfWork.Object, UnitTestHelper.GetAutoMapperProfile());

            var actual = await customerService.GetByIdAsync(id);

            Assert.That(actual, Is.EqualTo(expected).Using(new EventModelEqualityComparer()));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task EventService_DeleteAsync_DeletesEvent(int id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.EventRepository.DeleteByIdAsync(It.IsAny<int>()));
            var customerService = new EventService(mockUnitOfWork.Object, UnitTestHelper.GetAutoMapperProfile());

            await customerService.DeleteAsync(id);

            mockUnitOfWork.Verify(x => x.EventRepository.DeleteByIdAsync(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
        }

        [Test]
        public async Task EventService_AddAsync_AddsEventModel()
        {
            using var context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.EventRepository.AddAsync(It.IsAny<Event>()));
            mockUnitOfWork.Setup(m => m.EventSubjectCategoryRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(context.EventSubjectCategories.AsEnumerable().FirstOrDefault(It.IsAny<EventSubjectCategory>()));
            mockUnitOfWork.Setup(m => m.SpeakerRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(context.Speakers.AsEnumerable().FirstOrDefault(It.IsAny<Speaker>()));
            mockUnitOfWork.Setup(m => m.EventFormatRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(context.EventFormats.AsEnumerable().FirstOrDefault(It.IsAny<EventFormat>()));

            var customerService = new EventService(mockUnitOfWork.Object, UnitTestHelper.GetAutoMapperProfile());
            var eventModel = GetTestEventModels.First();

            await customerService.AddAsync(eventModel);

            mockUnitOfWork.Verify(x => x.EventRepository.AddAsync(It.Is<Event>(x => x.Id == eventModel.Id)), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
        }
        [Test]
        public async Task EventService_UpdateAsync_AddsEventModel()
        {
            using var context = new EventCatalogDbContext(UnitTestHelper.GetUnitTestDbContextOptions());
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.EventRepository.UpdateAsync(It.IsAny<Event>()));
            mockUnitOfWork.Setup(m => m.EventSubjectCategoryRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(context.EventSubjectCategories.AsEnumerable().FirstOrDefault(It.IsAny<EventSubjectCategory>()));
            mockUnitOfWork.Setup(m => m.SpeakerRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(context.Speakers.AsEnumerable().FirstOrDefault(It.IsAny<Speaker>()));
            mockUnitOfWork.Setup(m => m.EventFormatRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(context.EventFormats.AsEnumerable().FirstOrDefault(It.IsAny<EventFormat>()));
            mockUnitOfWork.Setup(m => m.EventRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(context.Events.AsEnumerable().FirstOrDefault(It.IsAny<Event>()));

            var customerService = new EventService(mockUnitOfWork.Object, UnitTestHelper.GetAutoMapperProfile());
            var eventModel = GetTestEventModels.First();

            await customerService.UpdateAsync(eventModel);

            mockUnitOfWork.Verify(x => x.EventRepository.UpdateAsync(It.Is<Event>(x => x.Id == eventModel.Id)), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
        }





        public List<EventModel> GetTestEventModels = new List<EventModel>()
        {
                new EventModel
                {
                    Id = 1,
                    Title = "Title1",
                    Description = "Description1",
                    Address = "Addrees1",
                    StartDateTime = new DateTime(2025, 10, 11, 7, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 11, 9, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 1,
                    EventFormatId = 1,
                    ParticipantsIds = new List<int>()
                },
                new EventModel
                {
                    Id = 2,
                    Title = "Title2",
                    Description = "Description2",
                    Address = "Addrees2",
                    StartDateTime = new DateTime(2025, 10, 12, 8, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 12, 11, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 2,
                    ParticipantsIds = new List<int>()
                },
                new EventModel
                {
                    Id = 3,
                    Title = "Title3",
                    Description = "Description3",
                    Address = "Addrees3",
                    StartDateTime = new DateTime(2025, 10, 3, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 3, 13, 0, 0),
                    SpeakerId = 2,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 1,
                    ParticipantsIds = new List<int>()
                }
        };
        public List<Event> GetTestEvents = new List<Event>()
        {
                new Event
                {
                    Id = 1,
                    Title = "Title1",
                    Description = "Description1",
                    Address = "Addrees1",
                    StartDateTime = new DateTime(2025, 10, 11, 7, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 11, 9, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 1,
                    EventFormatId = 1,
                },
                new Event
                {
                    Id = 2,
                    Title = "Title2",
                    Description = "Description2",
                    Address = "Addrees2",
                    StartDateTime = new DateTime(2025, 10, 12, 8, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 12, 11, 0, 0),
                    SpeakerId = 1,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 2,
                },
                new Event
                {
                    Id = 3,
                    Title = "Title3",
                    Description = "Description3",
                    Address = "Addrees3",
                    StartDateTime = new DateTime(2025, 10, 3, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 3, 13, 0, 0),
                    SpeakerId = 2,
                    EventSubjectCategoryId = 2,
                    EventFormatId = 1,
                }
        };
    }
}
