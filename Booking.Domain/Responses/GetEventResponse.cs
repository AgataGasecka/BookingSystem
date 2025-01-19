namespace Booking.Domain.Responses;
public class GetEventResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
    public decimal TicketPrice { get; set; }
    public int AvailableTickets { get; set; }
}

