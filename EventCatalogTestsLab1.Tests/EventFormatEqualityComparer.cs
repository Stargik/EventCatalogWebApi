using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCatalogTestsLab1.Tests
{
    internal class EventFormatEqualityComparer : IEqualityComparer<EventFormat>
    {
        public bool Equals(EventFormat? x, EventFormat? y)
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
                && x.Format == y.Format;
        }

        public int GetHashCode([DisallowNull] EventFormat obj)
        {
            return obj.GetHashCode();
        }

    }
}
