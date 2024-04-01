using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCatalogTestsLab1.Tests
{
    internal class ParticipantEqualityComparer : IEqualityComparer<Participant>
    {
        public bool Equals(Participant? x, Participant? y)
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
                && x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] Participant obj)
        {
            return obj.GetHashCode();
        }

    }
}
