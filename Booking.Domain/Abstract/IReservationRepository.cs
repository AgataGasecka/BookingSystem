using Booking.Domain.Entities;

namespace Booking.Domain.Abstract;

public interface IReservationRepository
{
    Task AddReservation(Reservation reservation);
}

