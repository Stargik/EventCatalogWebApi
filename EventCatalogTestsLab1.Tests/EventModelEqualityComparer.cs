using BLL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCatalogTestsLab1.Tests
{
    public class EventModelEqualityComparer : IEqualityComparer<EventModel>
    {
        public bool Equals(EventModel? x, EventModel? y)
        {
            if (x is null && y is null)
            {
                return true;
            }
            if (x is null || y is null)
            {
                return false;
            }
            return x.Id == y.Id
                && x.Title == y.Title
                && x.Description == y.Description
                && x.Address == y.Address
                && x.EventSubjectCategoryId == y.EventSubjectCategoryId
                && x.EventFormatId == y.EventFormatId
                && x.StartDateTime == y.StartDateTime
                && x.EndDateTime == y.EndDateTime
                && x.ParticipantsIds.Count == y.ParticipantsIds.Count;
        }

        public int GetHashCode([DisallowNull] EventModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
