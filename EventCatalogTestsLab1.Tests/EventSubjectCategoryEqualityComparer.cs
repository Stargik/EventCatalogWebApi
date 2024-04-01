using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCatalogTestsLab1.Tests
{
    internal class EventSubjectCategoryEqualityComparer : IEqualityComparer<EventSubjectCategory>
    {
        public bool Equals(EventSubjectCategory? x, EventSubjectCategory? y)
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
                && x.Title == y.Title;
        }

        public int GetHashCode([DisallowNull] EventSubjectCategory obj)
        {
            return obj.GetHashCode();
        }

    }
}
