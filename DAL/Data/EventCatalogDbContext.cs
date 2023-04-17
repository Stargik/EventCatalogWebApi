
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DAL.Data
{
    public class EventCatalogDbContext : IdentityDbContext<ApiUser>
    {
        public EventCatalogDbContext(DbContextOptions<EventCatalogDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new EventFormatConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            
        }

        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<EventSubjectCategory> EventSubjectCategories { get; set; }
        public DbSet<EventFormat> EventFormats { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
