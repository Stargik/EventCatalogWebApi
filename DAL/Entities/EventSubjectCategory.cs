using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class EventSubjectCategory : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
