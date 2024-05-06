using Bogus;
using BookSample.Data.Database;
using BookSample.Data.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Aspire
builder.AddServiceDefaults();

// EF Core with Aspire component
builder.AddNpgsqlDbContext<BookDbContext>("books");

var host = builder.Build();

using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<BookDbContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

var created = db.Database.EnsureCreated();

if (!created)
    return;

// Seed data if the database was just created
const int authorCount = 40;
const int publisherCount = 20;
const int bookCount = 1000;

var authorFaker = new Faker<Author>()
    .RuleFor(x => x.FirstName, b => b.Name.FirstName())
    .RuleFor(x => x.LastName, b => b.Name.LastName());

var publisherFaker = new Faker<Publisher>()
    .RuleFor(x => x.Name, b => b.Company.CompanyName());

string[] possibleGenres = [
    "Fantasy",
    "Science Fiction",
    "Action & Adventure",
    "Mystery",
    "Horror",
    "Thriller",
    "Historical",
    "Romance",
    "Biography",
    "Food & Drink",
    "History",
    "Travel",
    "True Crime",
    "Humor",
    "Science & Technology",
    "Parenting & Families"
];

static string ToSnakeCase(string name) => name.Titleize().Underscore().ToLowerInvariant();
var genreFaker = new Faker<Genre>()
    .RuleFor(x => x.Name, b => possibleGenres[b.IndexFaker])
    .RuleFor(x => x.Id, (b, g) => ToSnakeCase(g.Name))
    .RuleFor(x => x.Description, b => b.Lorem.Paragraph(2));

var bookFaker = new Faker<Book>()
    .RuleFor(x => x.Title, b => b.Lorem.Sentence().TrimEnd('.'))
    .RuleFor(x => x.Description, b => b.Lorem.Paragraph())
    .RuleFor(x => x.AuthorId, b => b.Random.Long(1, authorCount))
    .RuleFor(x => x.ISBN, b => b.ISBN())
    .RuleFor(x => x.PublishedAt, b => DateOnly.FromDateTime(b.Date.Past(10).ToUniversalTime()))
    .RuleFor(x => x.CreatedAt, b => b.Date.Past().ToUniversalTime())
    .RuleFor(x => x.LastModifiedAt, (b, book) => book.CreatedAt)
    .RuleFor(x => x.GenreId, b => ToSnakeCase(b.PickRandom(possibleGenres)))
    .RuleFor(x => x.AuthorId, b => b.Random.Long(1, authorCount))
    .RuleFor(x => x.PublisherId, b => b.Random.Long(1, publisherCount));

logger.LogInformation("Generating entries...");
try
{
    db.Authors.AddRange(authorFaker.GenerateLazy(authorCount));
    db.Publishers.AddRange(publisherFaker.GenerateLazy(publisherCount));
    db.Genres.AddRange(genreFaker.GenerateLazy(possibleGenres.Length));
    db.SaveChanges();
    db.Books.AddRange(bookFaker.GenerateLazy(bookCount));
    db.SaveChanges();
}
catch (DbUpdateException ex)
{
    logger.LogError(ex.InnerException?.Message ?? ex.Message);
    return;
}

logger.LogInformation("Finished");