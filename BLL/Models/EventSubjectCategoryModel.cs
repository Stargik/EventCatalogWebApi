
namespace BLL.Models
{
    public class EventSubjectCategoryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<int> EventsIds { get; set; }
    }
}
