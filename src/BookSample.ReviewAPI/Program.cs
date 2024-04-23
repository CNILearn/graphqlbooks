using BookSample.ReviewAPI.Data.Repositories;
using BookSample.ReviewAPI.Parameters;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.AddServer(new(){
        Url = "https://localhost:7078"
    });
});

builder.Services.AddSingleton<ReviewRepository>();

var app = builder.Build();

app.MapGet("/api/reviews", (BookIdsParameter bookIdsParameter, [FromQuery] int? take, [FromServices] ReviewRepository repo) =>
{
    return repo.GetReviews(bookIdsParameter.BookIds, take);
})
.WithTags("Reviews")
.WithName("GetReviews")
.WithOpenApi();

app.MapGet("/api/ratings", (BookIdsParameter bookIdsParameter, [FromServices] ReviewRepository repo) =>
{
    return repo.GetRatings(bookIdsParameter.BookIds);
})
.WithTags("Ratings")
.WithName("GetRatings")
.WithOpenApi();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();