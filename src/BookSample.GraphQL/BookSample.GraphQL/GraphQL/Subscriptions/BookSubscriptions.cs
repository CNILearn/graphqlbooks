using BookSample.GraphQL.GraphQL.Subscriptions.Messagse;

namespace BookSample.GraphQL.GraphQL.Subscriptions;

[ExtendObjectType<Subscription>]
public class BookSubscriptions
{
    [Subscribe]
    public BookAddedMessage BookAdded([EventMessage] BookAddedMessage message) => message;
}
