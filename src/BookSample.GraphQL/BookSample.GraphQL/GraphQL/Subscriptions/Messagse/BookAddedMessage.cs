namespace BookSample.GraphQL.GraphQL.Subscriptions.Messagse;

public class BookAddedMessage
{
    [ID]
    public required long BookId { get; init; }
}