
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class EventFormat : BaseEntity
    {
        [Required]
        public string Format { get; set; }
    }
}
