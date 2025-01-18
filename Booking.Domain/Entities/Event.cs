namespace Booking.Domain.Entities;
public class Event : EventBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime AnnouncementDate { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; } 

    //It wasn't mentioned in the task description,
    // but I determined it is worth to have ticket price
    // so I can compute a reservation amount
    public decimal TicketPrice { get; set; }
    public int TotalTickets { get; set; }
    public int AvailableTickets { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

