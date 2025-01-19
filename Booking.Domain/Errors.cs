namespace Booking.Domain;
public static class Errors
{
    public const string NotFound = "Event with given id does not exist";

    public const string NoContentForParameters = "No events for given category and period";

    public const string Failure = "Adding failed";

    public const string TooLessTickets = "Too less tickets left for this event";

    public const string ConcurencyError = "Concurrency exception occured:";
}

