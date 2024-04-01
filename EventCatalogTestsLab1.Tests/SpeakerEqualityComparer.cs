using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCatalogTestsLab1.Tests
{
    internal class SpeakerEqualityComparer : IEqualityComparer<Speaker>
    {
        public bool Equals(Speaker? x, Speaker? y)
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
                && x.Email == y.Email
                && x.FirstName == y.FirstName
                && x.LastName == y.LastName;
        }

        public int GetHashCode([DisallowNull] Speaker obj)
        {
            return obj.GetHashCode();
        }

    }
}
