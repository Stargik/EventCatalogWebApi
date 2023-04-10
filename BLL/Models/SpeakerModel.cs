
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class SpeakerModel
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public ICollection<int> EventsIds { get; set; }
    }
}
