using System.Text.Json.Serialization;
using Booking.Domain.UsefulModels;
using FluentValidation;

namespace Booking.API.Requests;
public record CreateEventRequest
{
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime AnnouncementDate { get; init; }
    public DateTime Date { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventCategory Category { get; init; }
    public decimal TicketPrice { get; init; }
    public int TotalTickets { get; init; }
    public int AvailableTickets { get; init; }
}

public class CreateEventValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Date).Must(x => x.Date > DateTime.Now).NotEmpty();
        RuleFor(x => x.AnnouncementDate).Must(x => x.Date >= DateTime.Now).NotEmpty();
        RuleFor(x => x.AnnouncementDate).LessThan(x => x.Date);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.TicketPrice).GreaterThan(0).NotEmpty();
        RuleFor(x => x.TotalTickets).GreaterThan(0).NotEmpty() ;
        RuleFor(x => x.AvailableTickets).LessThanOrEqualTo(x => x.TotalTickets).NotEmpty();

    }
}

