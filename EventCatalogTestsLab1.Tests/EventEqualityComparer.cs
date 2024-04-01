using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCatalogTestsLab1.Tests
{
    internal class EventEqualityComparer : IEqualityComparer<Event>
    {
        public bool Equals(Event? x, Event? y)
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
                && x.StartDateTime == y.StartDateTime
                && x.EndDateTime == y.EndDateTime;
        }

        public int GetHashCode([DisallowNull] Event obj)
        {
            return obj.GetHashCode();
        }

    }
}
