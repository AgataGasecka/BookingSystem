using Booking.Domain.Abstract;
using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Booking.Persistence.Repositories;
public class ReservationRepository : IReservationRepository
{
    private readonly EventBookingDbContext _dbContext;
    private readonly IEventRepository _eventRepository;
    private readonly AsyncRetryPolicy _retryPolicy;
    public ReservationRepository(EventBookingDbContext dbContext, IEventRepository eventRepository)
    {
        _dbContext = dbContext;
        _eventRepository = eventRepository;
        _retryPolicy = Policy
           .Handle<DbUpdateConcurrencyException>()
           .WaitAndRetryAsync(3, retryAttempt =>
           TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
    public async Task AddReservation(Reservation reservation)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var eventItem = await _eventRepository.GetEvent(reservation.EventId);
            if (eventItem == null)
            {
                throw new ArgumentException("Event with given id not found");
            }
            if (eventItem.AvailableTickets < reservation.NumberOfTickets)
            {
                throw new Exception("Too less tickets left for this reservation");
            }
            reservation.Amount = reservation.NumberOfTickets * eventItem.TicketPrice;
            var result = await _dbContext.Reservations.AddAsync(reservation);
            try
            {
                await _eventRepository.ReduceNumberOfAvailableTickets(reservation.EventId, reservation.NumberOfTickets);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await ex.Entries.Single().ReloadAsync();
                Console.WriteLine($"Concurrency exception occured: {ex.Message}");
                throw;
            }
        });
    }
}

