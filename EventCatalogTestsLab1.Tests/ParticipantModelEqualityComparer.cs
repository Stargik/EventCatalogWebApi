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
    internal class ParticipantModelEqualityComparer : IEqualityComparer<ParticipantModel>
    {
        public bool Equals(ParticipantModel? x, ParticipantModel? y)
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

        public int GetHashCode([DisallowNull] ParticipantModel obj)
        {
            return obj.GetHashCode();
        }

    }
}
