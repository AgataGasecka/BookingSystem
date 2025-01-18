using Booking.Domain.Abstract;
using Booking.Domain.Entities;
using Booking.Domain.UsefulModels;
using Microsoft.EntityFrameworkCore;


namespace Booking.Persistence.Repositories;
public class EventRepository : IEventRepository
{
    private readonly EventBookingDbContext _dbContext;

    public EventRepository(EventBookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CountAllEvents()
    {
        return await _dbContext.Events.CountAsync();
    }

    public async Task ReduceNumberOfAvailableTickets(int eventId, int numberOfTickets)
    {
        var eventItem = _dbContext.Events.FirstOrDefault(e => e.Id == eventId);
        if (eventItem != null)
        {
            eventItem.AvailableTickets -= numberOfTickets;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Event> CreateEvent(Event eventItem)
    {
        var result = await _dbContext.Events.AddAsync(eventItem);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Event> GetEvent(int eventId)
    {
        return await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<IEnumerable<Event>> GetEvents(EventCategory category, Period period)
    {
        return await _dbContext.Events.Where(e => e.Category == category.ToString()
        && e.Date > period.From && e.Date < period.To)
            .ToListAsync();
    }

    public async Task<SalesReport> CreateReport(int eventId)
    {
        var eventItem = await GetEvent(eventId);
        if (eventItem == null)
        {
            throw new Exception("No event for given id");
        }
        var report = new SalesReport()
        {
            SalesReportDictionary = new Dictionary<DateTime, SalesReportRow>()

        };
        for (var i = eventItem.AnnouncementDate; i < eventItem.Date; i = i.AddDays(1))
        {
            var reservationsForThisDay = _dbContext.Reservations.Where(d => d.EventId == eventId && d.ReservationDate.Day == i.Day &&
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
}

