using Booking.Domain.Responses;
using Booking.Domain.UsefulModels;

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

    public List<Reservation> Reservations { get; set; } = new List<Reservation>();

    public Reservation CreateReservation(int numberOfTickets, string ownerName)
    {
        if (!GivenNumberOfTicketsIsAvailable(numberOfTickets))
            throw new Exception("Too less tickets left for this reservation");
        var createdReservation = new Reservation
        {
            Amount = ComputeAmonutForTickets(numberOfTickets),
            EventId = Id,
            NumberOfTickets = numberOfTickets,
            OwnerName = ownerName,
            ReservationDate = DateTime.UtcNow
        };
        Reservations.Add(createdReservation);

        ReduceNumberOfAvailableTickets(numberOfTickets);
        return createdReservation;
    }
    public bool GivenNumberOfTicketsIsAvailable(int numberOfTickets)
    {
        return AvailableTickets > numberOfTickets;
    }

    public decimal ComputeAmonutForTickets(int numberOfTickets)
    {
        return TicketPrice * numberOfTickets;
    }

    public void ReduceNumberOfAvailableTickets(int numberOfTickets)
    {
        AvailableTickets -= numberOfTickets;
    }

    public SalesReport GenerateSalesReport()
    {
        SalesReport report = new SalesReport()
        {
            SalesReportDictionary = new Dictionary<DateTime, SalesReportRow>()
        };
        for (var i = AnnouncementDate; i <= Date; i = i.AddDays(1))
        {
            var reservationsForThisDay = Reservations.Where(d => d.EventId == Id && d.ReservationDate.Day == i.Day &&
            d.ReservationDate.Month == i.Month && d.ReservationDate.Year == i.Year).ToList();

            var reportRow = new SalesReportRow()
            {
                DailyAmountsSum = reservationsForThisDay.Sum(a => a.Amount),
                DailyNumberOfSoldTickets = reservationsForThisDay.Sum(t => t.NumberOfTickets),

            };
            report.TotalAmountSum += reportRow.DailyAmountsSum;
            report.TotalNumberOfSoldTickets += reportRow.DailyNumberOfSoldTickets;
            report.SalesReportDictionary.Add(i, reportRow);

        }
        return report;
    }

    public GetEventResponse CreateResponse()
    {
        return new GetEventResponse()
        {
            AvailableTickets = AvailableTickets,
            Name = Name,
            Category = Category,
            TicketPrice = TicketPrice,
            Date = Date,
            Description = Description
        };
    }
}

