
namespace BLL.Models
{
    public class SpeakerModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<int> EventsIds { get; set; }
    }
}
