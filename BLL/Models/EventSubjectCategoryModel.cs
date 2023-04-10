
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class EventSubjectCategoryModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public ICollection<int> EventsIds { get; set; }
    }
}
