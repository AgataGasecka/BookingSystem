using Booking.API.Requests;
using Booking.Domain.Abstract;
using Booking.Domain.Entities;
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
    static async Task HandlePostReservation(IReservationRepository reservationRepository, [FromBody] CreateReservationRequest reservationRequest)
    {
        var reservationToCreate = new Reservation
        {
            EventId = reservationRequest.EventId,
            OwnerName = reservationRequest.OwnerName,
            NumberOfTickets = reservationRequest.NumberOfTickets,
            ReservationDate = DateTime.UtcNow
        };
        await reservationRepository.AddReservation(reservationToCreate);
    }

}

