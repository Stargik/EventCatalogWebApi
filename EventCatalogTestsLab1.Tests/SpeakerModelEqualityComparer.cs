using BLL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCatalogTestsLab1.Tests
{
    internal class SpeakerModelEqualityComparer : IEqualityComparer<SpeakerModel>
    {
        public bool Equals(SpeakerModel? x, SpeakerModel? y)
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
                && x.LastName == y.LastName
                && x.EventsIds.Count == y.EventsIds.Count;
        }

        public int GetHashCode([DisallowNull] SpeakerModel obj)
        {
            return obj.GetHashCode();
        }

    }
}
