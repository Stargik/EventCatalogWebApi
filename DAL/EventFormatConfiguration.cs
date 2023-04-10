using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class EventFormatConfiguration : IEntityTypeConfiguration<EventFormat>
    {
        public void Configure(EntityTypeBuilder<EventFormat> builder)
        {
            builder.HasData(
                new EventFormat
                {
                    Id = 1,
                    Format = "Online"
                    
                },
                new EventFormat
                {
                    Id = 2,
                    Format = "Ofline"
                }
            );
        }
    }
}
