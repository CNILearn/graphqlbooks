using BookSample.ReviewAPI.Data.Repositories;
using BookSample.ReviewAPI.Parameters;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ReviewRepository>();

var app = builder.Build();

app.MapGet("/api/reviews", (BookIdsParameter bookIdsParameter, [FromQuery] int? take, [FromServices] ReviewRepository repo) =>
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

app.UseSwagger();
app.UseSwaggerUI();

app.Run();