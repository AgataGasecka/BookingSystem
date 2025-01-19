using FluentValidation;

namespace Booking.API.Requests;
public record CreateReservationRequest
{
    public int EventId { get; init; }
    public string OwnerName { get; init; } = string.Empty;
    public int NumberOfTickets { get; init; }
}

public class CreateReservationValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationValidator()
    {
        RuleFor(x => x.EventId).GreaterThan(0).NotEmpty();
        RuleFor(x => x.NumberOfTickets).GreaterThan(0);
    }
}