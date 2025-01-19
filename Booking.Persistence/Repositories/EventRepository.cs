using Booking.Domain;
using Booking.Domain.Abstract;
using Booking.Domain.Entities;
using Booking.Domain.UsefulModels;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Booking.Persistence.Repositories;
public class EventRepository : IEventRepository
{
    private readonly EventBookingDbContext _dbContext;
    private readonly AsyncRetryPolicy _retryPolicy;

    public EventRepository(EventBookingDbContext dbContext)
    {
        _dbContext = dbContext;
        _retryPolicy = Policy
           .Handle<DbUpdateConcurrencyException>()
           .WaitAndRetryAsync(3, retryAttempt =>
           TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<Event> CreateEvent(Event eventItem)
    {
        var result = await _dbContext.Events.AddAsync(eventItem);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Event> GetEvent(int eventId)
    {
        await GetReservationsForEvent(eventId);
        return await _dbContext.Events.SingleAsync(e => e.Id == eventId);
    }

    public async Task UpdateEventReservations(Reservation reservation, int eventId, int availableTicketsNumber)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            await _dbContext.Reservations.AddAsync(reservation);
            var eventToUpdate =_dbContext.Events.FirstOrDefault(e => e.Id == eventId);
            if(eventToUpdate != null)
                eventToUpdate.AvailableTickets = availableTicketsNumber;
            try
            {
                await _dbContext.SaveChangesAsync();       
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await ex.Entries.Single().ReloadAsync();
                throw new Exception(Errors.ConcurencyError + ex.Message);
            }
        });
    }
    public async Task<IEnumerable<Reservation>> GetReservationsForEvent(int eventId)
    {
        return await _dbContext.Reservations.Where(e => e.EventId == eventId).ToListAsync();
    }
    public async Task<IEnumerable<Event>> GetEvents(EventCategory category, Period period)
    {
        return await _dbContext.Events.Where(e => e.Category == category.ToString()
        && e.Date > period.From && e.Date < period.To)
            .ToListAsync();
    } 
}

