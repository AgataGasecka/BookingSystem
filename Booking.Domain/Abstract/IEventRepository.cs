using Booking.Domain.Entities;
using Booking.Domain.UsefulModels;

namespace Booking.Domain.Abstract;
public interface IEventRepository
{
    Task<IEnumerable<Event>> GetEvents(EventCategory category, Period period);
    Task<Event> GetEvent(int eventId);
    Task<Event> CreateEvent(Event eventItem);
    Task<IEnumerable<Reservation>> GetReservationsForEvent(int eventId);
    Task UpdateEventReservations(Reservation reservation, int eventId, int availableTicketsNumber);
}
