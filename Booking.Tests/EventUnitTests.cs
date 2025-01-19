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
    [DataRow(2, 300)]
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
    [DataRow(400, true)]
    [DataRow(499, true)]
    [DataRow(510, false)]
    [DataRow(1000, false)]
    public void GivenNumberOfTicketsIsAvailableTest(int ticketsNumber, bool result)
    {
        var testEvent = CreateEventWithoutReservations();
        var boolResult = testEvent.GivenNumberOfTicketsIsAvailable(ticketsNumber);
        Assert.AreEqual(boolResult, result);
    }
}

