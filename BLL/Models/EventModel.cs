using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        public int SpeakerId { get; set; }
        public int EventSubjectCategoryId { get; set; }
        public int EventFormatId { get; set; }

        public ICollection<int> ParticipantsIds { get; set; }
    }
}
