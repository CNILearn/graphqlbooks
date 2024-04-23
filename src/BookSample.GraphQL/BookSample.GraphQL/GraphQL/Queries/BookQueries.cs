using BookSample.Data.Database;
using BookSample.Data.Models;

namespace BookSample.GraphQL.GraphQL.Queries;

[ExtendObjectType<Query>]
internal class BookQueries
{
    //[UseOffsetPaging]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Book> GetBooks([Service] BookDbContext db) =>
        db.Books;

    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<Book> GetBookById([ID] long bookId, BookDbContext db) =>
        db.Books.Where(x => x.Id == bookId);
}