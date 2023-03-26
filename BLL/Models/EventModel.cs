using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int SpeakerId { get; set; }
        public int EventSubjectCategoryId { get; set; }
        public int EventFormatId { get; set; }

        public ICollection<int> ParticipantsIds { get; set; }
    }
}
