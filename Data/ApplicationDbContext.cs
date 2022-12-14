
using CabManagementSystems.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystems.Data
{
    public class ApplicationDbContext : IdentityDbContext

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Booking>()
                .HasOne(m => m.Driver)
                .WithMany(m => m.Bookings)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Cab> Cabs { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<DriverDetails> Drivers { get; set; }

        public DbSet<Place> Places { get; set; }
    }
}