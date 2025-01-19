using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Booking.Domain.UsefulModels;
using FluentValidation;

namespace Booking.API.Requests;
public record GetEventsRequest
{
    [EnumDataType(typeof(EventCategory))]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventCategory Category { get; init; }
    public DateTime From { get; init; }
    public DateTime To { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 3;

}

public class GetEventValidator : AbstractValidator<GetEventsRequest>
{
    public GetEventValidator()
    {
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.From < x.To).NotEmpty();

    }
}

