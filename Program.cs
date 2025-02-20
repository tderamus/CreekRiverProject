using CreekRiver.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<CreekRiverDbContext>(builder.Configuration["CreekRiverDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// API CALLS TO GET DATABASE DATA

// GET ALL CAMPSITES
app.MapGet("/api/campsites", (CreekRiverDbContext db) =>
{
    if (db.Campsites == null)
    {
        return Results.NotFound();
    }
    var campsites = db.Campsites.Include(c => c.CampsiteType).ToList();
    return Results.Ok(campsites);
});

// GET CAMPSITE BY ID
app.MapGet("/api/campsites/{id}", (CreekRiverDbContext db, int id) =>
{
    if (db.Campsites == null)
    {
        return Results.NotFound();
    }   
    var campsite = db.Campsites.Include(c => c.CampsiteType).SingleOrDefault(c => c.Id == id);
    if (campsite == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(campsite);
});
app.Run();
