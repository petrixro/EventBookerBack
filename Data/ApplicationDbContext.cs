using Microsoft.EntityFrameworkCore;
using EventBookerBack.Models;

namespace EventBookerBack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Location>()
                        .ToTable("Location")
                        .HasMany(e => e.Events);
            modelBuilder.Entity<User>().ToTable("User").HasMany(e => e.Events);
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
        }
    }
}
