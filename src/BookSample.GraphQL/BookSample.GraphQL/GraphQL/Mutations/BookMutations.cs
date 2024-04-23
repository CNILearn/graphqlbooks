using BookSample.Data.Database;
using BookSample.Data.Models;
using BookSample.GraphQL.GraphQL.Mutations.Inputs;
using BookSample.GraphQL.GraphQL.Subscriptions;
using BookSample.GraphQL.GraphQL.Subscriptions.Messagse;
using BookSample.GraphQL.Mapping;
using HotChocolate.Subscriptions;

namespace BookSample.GraphQL.GraphQL.Mutations;

[ExtendObjectType<Mutation>]
internal class BookMutations
{
    [UseMutationConvention(PayloadFieldName = "createdBook")]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Book>> CreateBookAsync(BookDbContext db, ITopicEventSender sender, CreateBookInput bookInput, CancellationToken cancellationToken)
    {
        var book = bookInput.ToModel();
        db.Books.Add(book);
        await db.SaveChangesAsync(cancellationToken);

        // Publish message for subscription
        BookAddedMessage message = new()
        {
            BookId = book.Id
        };
        _ = sender.SendAsync(nameof(BookSubscriptions.BookAdded), message, cancellationToken);

        return db.Books.Where(x => x.Id == book.Id);
    }

    // Mutation for updating existing book
    // ...


    // Mutation for deleting book
    // ...
}