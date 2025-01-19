using Booking.API.Requests;
using Booking.Domain;
using Booking.Domain.Abstract;
using Booking.Domain.Entities;
using Booking.Domain.UsefulModels;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Endpoints;
public static class EventEndpoints
{
    public static void AddEventEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/event");
        endpoints.MapGet("/{id}", HandleGetEvent)
            .WithSummary("Gets a specific event by its id ")
            .WithOpenApi();

        endpoints.MapGet("/getFiltered", HandleGetEvents)
            .WithSummary("Filter events by category and date range")
            .WithRequestValidation<GetEventsRequest>()
            .WithOpenApi();

        endpoints.MapPost("/", HandlePostEvent)
            .WithSummary("Add new event")
            .WithRequestValidation<CreateEventRequest>()
            .WithOpenApi();

        endpoints.MapGet("/{id}/getReport", HandleGetReport)
           .WithSummary("Get sales report about event")
           .WithOpenApi();

    }
    

    static async Task<IResult> HandleGetEvent(int id, IEventRepository eventRepository)
    {
        var eventItem = await eventRepository.GetEvent(id);
        if (eventItem == null)
            return Results.NotFound(Errors.NotFound);

        return Results.Ok(eventItem);
    }


    static async Task<IResult> HandleGetEvents(IEventRepository eventRepository, [AsParameters] GetEventsRequest eventRequest)
    {
        var period = new Period()
        {
            From = eventRequest.From,
            To = eventRequest.To,
        };
        var events = await eventRepository.GetEvents(eventRequest.Category, period);
        if (events.Count() == 0)
            return Results.NotFound(Errors.NoContentForParameters);

        var totalCount = events.Count();
        var totalPages = (int)Math.Ceiling((decimal) totalCount / eventRequest.PageSize);
        var eventsPerPage = events.Skip((eventRequest.Page - 1) * eventRequest.PageSize).Take(eventRequest.PageSize);
        
        return Results.Ok(eventsPerPage);
    }


    static async Task<IResult> HandlePostEvent(IEventRepository eventRepository, [FromBody] CreateEventRequest eventRequest)
    {
        var eventToCreate = new Event
        {
            Name = eventRequest.Name,
            Description = eventRequest.Description,
            Date = eventRequest.Date,
            Category = eventRequest.Category.ToString(),
            AnnouncementDate = eventRequest.AnnouncementDate,
            TotalTickets = eventRequest.TotalTickets,
            AvailableTickets = eventRequest.AvailableTickets,
            TicketPrice = eventRequest.TicketPrice
        };
        var createdEvent = await eventRepository.CreateEvent(eventToCreate);
        if (createdEvent == null)
            return Results.Problem(Errors.Failure);
        return Results.Ok(createdEvent);

    }
    static async Task<IResult> HandleGetReport(int id, IEventRepository eventRepository)
    {
        var eventItem = await eventRepository.GetEvent(id);
        if (eventItem == null)
            return Results.NotFound(Errors.NotFound);
        return Results.Ok(eventItem.GenerateSalesReport());
    }
}

