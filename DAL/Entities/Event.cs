using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Event : BaseEntity
    {
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

        [Required]
        public Speaker Speaker { get; set; }
        [Required]
        public EventSubjectCategory EventSubjectCategory { get; set; }
        [Required]
        public EventFormat EventFormat { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}
