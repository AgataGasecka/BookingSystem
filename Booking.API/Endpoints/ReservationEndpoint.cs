using Booking.API.Requests;
using Booking.Domain.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Endpoints;
public static class ReservationEndpoint
{
    public static void AddReservationEndpoint(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/reservation");

        endpoints.MapPost("/", HandlePostReservation)
            .WithSummary("Add new reservation for event")
            .WithRequestValidation<CreateReservationRequest>()
            .WithOpenApi();
    }
    static async Task<IResult> HandlePostReservation(IEventRepository eventRepository, [FromBody] CreateReservationRequest reservationRequest)
    {
        var eventItem = await eventRepository.GetEvent(reservationRequest.EventId);
        var reservation = eventItem.CreateReservation(reservationRequest.NumberOfTickets, reservationRequest.OwnerName);
        await eventRepository.UpdateEventReservations(reservation, eventItem.Id, eventItem.AvailableTickets);
        if (reservation == null)
            return Results.Problem("Adding reservation failed");
        return Results.Ok(reservation);
       
    }

}

