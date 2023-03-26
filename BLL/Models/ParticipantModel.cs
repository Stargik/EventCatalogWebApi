
namespace BLL.Models
{
    public class ParticipantModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<int> EventsIds { get; set; }
    }
}
