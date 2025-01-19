using System.Text.Json.Serialization;
using Booking;
using Booking.API.Endpoints;
using Booking.Domain.Abstract;
using Booking.Persistence;
using Booking.Persistence.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EventBookingDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
b => b.MigrationsAssembly(typeof(EventBookingDbContext).Assembly.FullName)));

builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddEventEndpoints();
app.AddReservationEndpoint();

app.UseHttpsRedirection();
app.Run();


