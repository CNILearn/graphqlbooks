using BookSample.ReviewAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Aspire
builder.AddServiceDefaults();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the ReviewRepository
builder.Services.AddSingleton<ReviewRepository>();

var app = builder.Build();

// Endpoints
app.MapGet("/api/reviews", ([FromQuery] string[] bookIds, [FromQuery] int? take, [FromServices] ReviewRepository repo) =>
{
    return repo.GetReviews(bookIds.ParseLongsSafely(), take);
})
.WithTags("Reviews")
.WithName("GetReviews")
.WithOpenApi();

app.MapGet("/api/ratings", ([FromQuery] string[] bookIds, [FromServices] ReviewRepository repo) =>
{
    return repo.GetRatings(bookIds.ParseLongsSafely());
})
.WithTags("Ratings")
.WithName("GetRatings")
.WithOpenApi();

// Swagger / OpenAPI
app.UseSwagger();
app.UseSwaggerUI();

// Aspire
app.MapDefaultEndpoints();

app.Run();