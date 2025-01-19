using Booking.Domain.Entities;
using Booking.Domain.UsefulModels;

namespace Booking.Tests;
[TestClass]
public sealed class EventUnitTests
{
    private Event CreateEventWithoutReservations()
    {
        return new Event()
        {
            AnnouncementDate = DateTime.Now.AddDays(-3),
            AvailableTickets = 500,
            TotalTickets = 700,
            Category = EventCategory.Music.ToString(),
            Date = DateTime.Now.AddDays(20),
            Description = "Classical music concert",
            Name = "Concert",
            TicketPrice = 150,
            Id = 1,
            Reservations = { },
        };
    }

    private Event CreateEventWithReservations()
    {
        return new Event()
        {
            AnnouncementDate = DateTime.Now.AddDays(-5),
            AvailableTickets = 300,
            TotalTickets = 300,
            Category = EventCategory.Party.ToString(),
            Date = DateTime.Now.AddDays(3),
            Description = "Carnival ball with costumes",
            Name = "Carnival ball",
            TicketPrice = 70,
            Id = 2,
            Reservations = { CreateFirstReservation(), CreateSecondReservation() },
        };
    }

    private Reservation CreateFirstReservation()
    {
        return new Reservation()
        {
            Amount = 280,
            EventId = 2,
            NumberOfTickets = 4,
            OwnerName = "Agata",
            ReservationDate = DateTime.Now.AddDays(-1),
            ReservationId = 1
        };
    }

    private Reservation CreateSecondReservation()
    {
        return new Reservation()
        {
            Amount = 630,
            EventId = 2,
            NumberOfTickets = 9,
            OwnerName = "Wojtek",
            ReservationDate = DateTime.Now.AddDays(-3),
            ReservationId = 1
        };
    }


    [TestMethod]
    [DataRow (20, 480)]
    [DataRow(100, 400)]
    [DataRow(50, 450)]
    [DataRow(4, 496)]
    [DataRow(313, 187)]
    [DataRow(499, 1)]
    public void ReduceNumberOfAvailableTicketsTest(int ticketsNumber, int result)
    {
        var testEvent = CreateEventWithoutReservations();
        testEvent.ReduceNumberOfAvailableTickets(ticketsNumber);
        Assert.AreEqual(result, testEvent.AvailableTickets);
    }

    [TestMethod]
    [DataRow(10,1500)]
    [DataRow(77, 11550)]
    [DataRow(18, 2700)]
    [DataRow(0, 0)]
    [DataRow(40, 6000)]
    [DataRow(11, 1650)]
    public void ComputeAmonutForTicketsTest(int ticketsNumber, int result)
    {
        var testEvent = CreateEventWithoutReservations();
        var amount = testEvent.ComputeAmonutForTickets(ticketsNumber);
        Assert.AreEqual(result, amount);
    }

    [TestMethod]
    [DataRow(499, true)]
    [DataRow(510, false)]
    public void GivenNumberOfTicketsIsAvailableTest(int ticketsNumber, bool result)
    {
        var testEvent = CreateEventWithoutReservations();
        var boolResult = testEvent.GivenNumberOfTicketsIsAvailable(ticketsNumber);
        Assert.AreEqual(boolResult, result);
    }
    [TestMethod]
    [DataRow()]

    public void GenerateSalesReportTest()
    {
        var testEvent = CreateEventWithReservations();

        var report = testEvent.GenerateSalesReport();
        Assert.AreEqual(report.TotalAmountSum, 910);
        Assert.AreEqual(report.TotalNumberOfSoldTickets, 13);
        Assert.IsTrue(report.SalesReportDictionary.Count == 9);
        Assert.IsTrue(report.SalesReportDictionary.ElementAt(2).Value.DailyAmountsSum == 630);
        Assert.IsTrue(report.SalesReportDictionary.ElementAt(2).Value.DailyNumberOfSoldTickets == 9);
        Assert.IsTrue(report.SalesReportDictionary.ElementAt(4).Value.DailyAmountsSum == 280);
        Assert.IsTrue(report.SalesReportDictionary.ElementAt(4).Value.DailyNumberOfSoldTickets == 4);
    }
}

