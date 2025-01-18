using Booking.API.Requests;
using Booking.Domain.Abstract;
using Booking.Domain.Entities;
using Booking.Domain.UsefulModels;
using Microsoft.AspNetCore.Http.HttpResults;
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
            return Results.NotFound();
        return Results.Ok(eventItem);
    }


    static async Task<List<Event>> HandleGetEvents(IEventRepository eventRepository, [AsParameters] GetEventsRequest eventRequest)
    {
        var period = new Period()
        {
            From = eventRequest.From,
            To = eventRequest.To,
        };
        var events = await eventRepository.GetEvents(eventRequest.Category, period);
        return events.ToList();
    }


    static async Task<Event> HandlePostEvent(IEventRepository eventRepository, [FromBody] CreateEventRequest eventRequest)
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
        return await eventRepository.CreateEvent(eventToCreate);

    }
    static async Task<SalesReport> HandleGetReport(int id, IEventRepository eventRepository)
    {
        var salesReport = await eventRepository.CreateReport(id);

        return salesReport;
    }
}

