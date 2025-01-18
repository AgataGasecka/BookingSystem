using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Persistence;
public class EventBookingDbContext : DbContext
{
    public EventBookingDbContext(DbContextOptions<EventBookingDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .Property(v => v.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Event>()
            .Property(p => p.AvailableTickets)
            .HasColumnType("int");
    }
}


