using Bogus;
using BookSample.Data.Models;
using Humanizer;
using System.Text;

namespace BookSampleDemo.Data.MockRepos;

public static class MockBookRepository
{
    private const int _authorCount = 40;
    private const int _publisherCount = 20;
    private const int _bookCount = 20;

    private static readonly Faker<Author> _authorFaker;

    private static readonly Faker<Publisher>  _publisherFaker;

    private static readonly string[] _possibleGenres;

    private static readonly Faker<Genre>  _genreFaker;

    private static readonly Faker<Book>  _bookFaker;

    private static readonly IDictionary<long, Author> _authors;

    private static readonly IDictionary<long, Publisher> _publishers;

    private static readonly IDictionary<string, Genre> _genres;

    private static readonly IDictionary<long, Book> _books;
    
    private static string ToSnakeCase(string name) => name.Titleize().Underscore().ToLowerInvariant();
    
    static MockBookRepository()
    {
        _possibleGenres = [
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
        _authorFaker = new Faker<Author>()
            .RuleFor(x => x.Id, b => b.IndexFaker + 1)
            .RuleFor(x => x.FirstName, b => b.Name.FirstName())
            .RuleFor(x => x.LastName, b => b.Name.LastName());
        _publisherFaker = new Faker<Publisher>()
            .RuleFor(x => x.Id, b => b.IndexFaker + 1)
            .RuleFor(x => x.Name, b => b.Company.CompanyName());
        _genreFaker = new Faker<Genre>()
            .RuleFor(x => x.Name, b => _possibleGenres[b.IndexFaker])
            .RuleFor(x => x.Id, (b, g) => ToSnakeCase(g.Name))
            .RuleFor(x => x.Description, b => b.Lorem.Paragraph(2));
        _authors = _authorFaker.GenerateLazy(_authorCount)
            .ToDictionary(author => author.Id);
        _publishers = _publisherFaker.GenerateLazy(_publisherCount)
            .ToDictionary(publisher => publisher.Id);
        _genres = _genreFaker.GenerateLazy(_possibleGenres.Length)
            .ToDictionary(genre => genre.Id);
        _bookFaker = new Faker<Book>()
            .RuleFor(x => x.Id, b => b.IndexFaker + 1)
            .RuleFor(x => x.Title, b => b.Lorem.Sentence().TrimEnd('.'))
            .RuleFor(x => x.Description, b => b.Lorem.Paragraph())
            .RuleFor(x => x.ISBN, b => b.ISBN())
            .RuleFor(x => x.PublishedAt, b => DateOnly.FromDateTime(b.Date.Past(10).ToUniversalTime()))
            .RuleFor(x => x.CreatedAt, b => b.Date.Past().ToUniversalTime())
            .RuleFor(x => x.LastModifiedAt, (b, book) => book.CreatedAt)
            .RuleFor(x => x.GenreId, b => ToSnakeCase(b.PickRandom(_possibleGenres)))
            .RuleFor(x => x.Genre, (b, book) => _genres[book.GenreId])
            .RuleFor(x => x.AuthorId, b => b.Random.Long(1, _authorCount))
            .RuleFor(x => x.Author, (b, book) => _authors[book.AuthorId])
            .RuleFor(x => x.PublisherId, b => b.Random.Long(1, _publisherCount))
            .RuleFor(x => x.Publisher, (b, book) => _publishers[book.PublisherId]);
        _books = _bookFaker.GenerateLazy(_bookCount)
            .ToDictionary(book => book.Id);
    }

    public static IEnumerable<Book> GetBooks() =>
        _books.Values;

    public static Book? GetBook(long id) =>
        _books.FirstOrDefault(x => x.Key == id).Value;
}

file static class MockExtensions
{
    /// <summary>
    /// Generates a dummy ISBN.
    /// </summary>
    public static string ISBN(this Faker f)
    {
        var sb = new StringBuilder(13 + 4);
        sb.Append("978-");
        sb.Append(f.Random.Number(9));
        sb.Append('-');
        sb.Append(f.Random.Number(1000, 9999));
        sb.Append('-');
        sb.Append(f.Random.Number(1000, 9999));
        sb.Append('-');
        sb.Append(f.Random.Number(9));
        return sb.ToString();
    }
}