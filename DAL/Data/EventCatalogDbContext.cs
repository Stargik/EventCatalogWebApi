
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class EventCatalogDbContext : DbContext
    {
        public EventCatalogDbContext(DbContextOptions<EventCatalogDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<EventSubjectCategory> EventSubjectCategories { get; set; }
        public DbSet<EventFormat> EventFormats { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
