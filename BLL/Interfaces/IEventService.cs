using BLL.Models;

namespace BLL.Interfaces
{
    public interface IEventService : ICrud<EventModel>
    {
        Task<IEnumerable<EventModel>> GetEventsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<EventModel>> GetEventsBySpeakerIdAsync(int speakerId);
        Task AddCategoryAsync(EventSubjectCategoryModel categoryModel);

        Task<IEnumerable<EventSubjectCategoryModel>> GetAllCategoriesAsync();
        Task DeleteCategoryAsync(int categoryId);
        Task UpdateCategoryAsync(EventSubjectCategoryModel categoryModel);

    }
}
