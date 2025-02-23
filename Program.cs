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

//**********************************GET API Calls with Get Requests*****************************************

// GET ALL CAMPSITES
app.MapGet("/api/campsites", (CreekRiverDbContext db) =>
{
    var campsites = db.Campsites.Include(c => c.CampsiteType).ToList();
    if (db.Campsites == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(campsites);
});

// GET CAMPSITE BY ID
app.MapGet("/api/campsites/{id}", (CreekRiverDbContext db, int id) =>
{
    var campsite = db.Campsites.Include(c => c.CampsiteType).SingleOrDefault(c => c.Id == id);

    if (db.Campsites == null)
    {
        return Results.NotFound();
    }   
   
    if (campsite == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(campsite);
});

// GET ALL USER PROFILES
app.MapGet("/api/userprofiles", (CreekRiverDbContext db) =>
{
    var userProfiles = db.UserProfiles.Include(up => up.Reservations).ThenInclude(r => r.Campsite).ThenInclude(c => c.CampsiteType).ToList();
    if (userProfiles == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(userProfiles);
});

// GET USER PROFILE BY ID
app.MapGet("/api/userprofiles/{id}", (CreekRiverDbContext db, int id) =>
{
    var userProfile = db.UserProfiles.Include(up => up.Reservations).ThenInclude(r => r.Campsite).ThenInclude(c => c.CampsiteType).SingleOrDefault(up => up.Id == id);
    if (userProfile == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(userProfile);
});

// GET RESERVATIONS
app.MapGet("/api/reservations", (CreekRiverDbContext db) =>
{
    return db.Reservations
        .Include(r => r.UserProfile)
        .Include(r => r.Campsite)
        .ThenInclude(c => c.CampsiteType)
        .OrderBy(res => res.CheckinDate)
        .ToList();
});

// GET RESERVATION BY ID
app.MapGet("/api/reservations/{id}", (CreekRiverDbContext db, int id) =>
{
    var reservation = db.Reservations
        .Include(r => r.UserProfile)
        .Include(r => r.Campsite)
        .ThenInclude(c => c.CampsiteType)
        .SingleOrDefault(r => r.Id == id);

    if (reservation == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(reservation);
});

//**********************************Create API Calls with Post Requests*****************************************

// CREATE CAMPSITE
app.MapPost("/api/campsites", (CreekRiverDbContext db, Campsite campsite) =>
{
    db.Campsites.Add(campsite);
    db.SaveChanges();
    return Results.Created($"/api/campsites/{campsite.Id}", campsite);
});

// CREATE RESERVATION
app.MapPost("/api/reservations", (CreekRiverDbContext db, Reservation newRes) =>
{
    // Check if the UserProfileId exists
    var userProfile = db.UserProfiles.SingleOrDefault(up => up.Id == newRes.UserProfileId);
    if (userProfile == null)
    {
        return Results.NotFound(new { message = "UserProfile not found" });
    }

    // Check if the CampsiteId exists
    var campsite = db.Campsites.SingleOrDefault(c => c.Id == newRes.CampsiteId);
    if (campsite == null)
    {
        return Results.NotFound(new { message = "Campsite not found" });
    }

    db.Reservations.Add(newRes);
    db.SaveChanges();
    return Results.Created($"/api/reservations/{newRes.Id}", newRes);
});

// CREATE USER PROFILE
app.MapPost("/api/userprofiles", (CreekRiverDbContext db, UserProfile newUser) =>
{
    db.UserProfiles.Add(newUser);
    db.SaveChanges();
    return Results.Created($"/api/userprofiles/{newUser.Id}", newUser);
});


//**********************************Update API Calls with PUT Requests*****************************************

// PUT UPDATE CAMPSITE
app.MapPut("/api/campsites/{id}", (CreekRiverDbContext db, int id, Campsite campsite) =>
{
    Campsite campsiteToUpdate = db.Campsites.SingleOrDefault(campsite => campsite.Id == id);
    if (campsiteToUpdate == null)
    {
        return Results.NotFound();
    }
    campsiteToUpdate.Nickname = campsite.Nickname;
    campsiteToUpdate.CampsiteTypeId = campsite.CampsiteTypeId;
    campsiteToUpdate.ImageUrl = campsite.ImageUrl;

    db.SaveChanges();
    return Results.NoContent();
});

// ***********************************Delete API Calls with DELETE Requests*****************************************


// DELETE CAMPSITE
app.MapDelete("/api/campsites/{id}", (CreekRiverDbContext db, int id) =>
{
    Campsite campsite = db.Campsites.SingleOrDefault(campsite => campsite.Id == id);
    if (campsite == null)
    {
        return Results.NotFound();
    }
    db.Campsites.Remove(campsite);
    db.SaveChanges();
    return Results.NoContent();

});

app.Run();
