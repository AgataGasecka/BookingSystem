namespace Booking.Domain.Entities;
public class Reservation : ReservationBase
{
    public int EventId { get; set; }
    public int NumberOfTickets { get; set; }
    // In a task description there was a type decimal mentioned for OwnerName,
    // but I assumed there was a mistake and I changed it to string type
    public string OwnerName { get; set; }
    public decimal Amount { get; set; }
    public DateTime ReservationDate { get; set; }
}

